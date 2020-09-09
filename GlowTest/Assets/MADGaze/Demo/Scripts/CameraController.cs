using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CameraController : MonoBehaviour, ISplitCameraCallback
{
    public Text HintText;
    
    private bool isRecorded;

    void Start()
    {
        isRecorded = false;

        SplitCamera.Instance.setCameraCallback(this);
         
        SplitCamera.Instance.setRecordVideoCallback(
            (string path) => {
                if(isRecorded){
                    isRecorded = false;
                    Debug.Log("RecordVideo: path : "+path); 
                    HintText.text = "RecordVideo path : "+ path;
                }
                
            },
            (int errorCode) => {
                Debug.Log(string.Format("RecordVideo Error: Code={0}", errorCode)); 
                HintText.text = string.Format("RecordVideo Error: Code={0}", errorCode);
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onConnected(){
        HintText.text = "Connected";        
    }

    public void onDisconnected(){
        HintText.text = "Disconnected";
    }

    public void onError(int code){
        if(code == -1){
            HintText.text ="Camera: There is no connecting MAD Gaze Cameras";
        }else{
            HintText.text = string.Format("Error: Code={0}", code);
        }
    }

    public void startPreview(){
        if(!SplitCamera.Instance.isDeviceConnected()){   
            SplitCamera.Instance.startPreview();
        }
    }

    public void stopPreview(){
        HintText.fontSize = 42;
        if(SplitCamera.Instance.isDeviceConnected()){
            SplitCamera.Instance.stopPreview();
        }
    }

    public void takePhoto(){
         
          if(SplitCamera.Instance.isDeviceConnected()){
            SplitCamera.Instance.takePicture(
                 (string path)=>{
                    HintText.text = "TakePhoto path : "+path;
                    Debug.Log("TakePhoto path : "+path); 
                 },
                 (int errorCode)=>{
                    HintText.text = string.Format("TakePhoto Error: Code={0}", errorCode);
                    Debug.Log(string.Format("TakePhoto Error: Code={0}", errorCode)); 
                 });
         }
    }

    public void toggleVideoRecording(){
        if (SplitCamera.Instance.isRecording()) {
            //Stop Video Recording
            SplitCamera.Instance.stopRecording();
            HintText.text = "Stop Recording";
            Debug.Log("Stop Recording"); 
            isRecorded = true;
        } else {
            //Start Video Recording
            SplitCamera.Instance.startRecording();
            HintText.text = "Start Recording";
            Debug.Log("Start Recording"); 
        }
    }
    

}
