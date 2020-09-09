using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using MADGazeSDK;
public class MADSensorManager : MonoBehaviour, ISplitSensorConnectionCallback{
    
    private bool isEnableSensor = false;

    public static MADSensorManager instance;
    public static MADSensorManager Instance     
    {       
        get         
        {           
            if (instance ==  null){
                instance = GameObject.FindObjectOfType(typeof(MADSensorManager)) as MADSensorManager;            
            }

            if(instance == null){
                Debug.Log ("MADSensorManager: not find MADSensorManager Object");  
            }

            return instance;        
        }   
    }

    void Start () {    
        isEnableSensor = false;
        initSensor();
    }

    void Update()
    {
       SplitUSBSensor.Instance.updateCamMatrix();
    }

    private void initSensor(){
        isEnableSensor = false;

        SplitUSBSensor.Instance.init();
        SplitUSBSensor.Instance.setConnectionCallback(this);
        
    }
    
    public void onConnected(){
        isEnableSensor = true;
        SplitUSBSensor.Instance.startSensorsCapturing();
    }

    public void onDisconnected(){
        Debug.Log ("MADSensorManager: sensor onDisconnected");  
        isEnableSensor = false;

        SplitUSBSensor.Instance.stopSensorsCapturing();
    }

    public void onError(int errorCode){
         isEnableSensor = false;
        //  if(this.OnErrorEvent != null)
        //     this.OnErrorEvent(this, errorCode);
        Debug.Log ("MADSensorManager: sensor onError : "+errorCode);  
    }

}

namespace MADSensorEvent
{
    public class BaseMADSensorEvent{
        public float X {get;set;}
        public float Y {get;set;}
        public float Z {get;set;}
        public BaseMADSensorEvent(float x, float y, float z){
            X = x;
            Y = y;
            Z = z;
        } 
    }

       
}
