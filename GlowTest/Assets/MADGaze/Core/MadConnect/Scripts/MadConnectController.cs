using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MADGazeSDK;
public class MadConnectController : MonoBehaviour
{
   public Text madidLabel;
   public Text appPurchaseLabel;
   public Text sensorLabel;
    public void requestMadId(){
        MadConnect.Instance.requestMadId(
            (id)=>{
                //success   
                if(madidLabel!=null){
                    madidLabel.text = id;
                } 
            },
            (errorMsg) =>{
                //fail
                if(madidLabel!=null){
                    madidLabel.text = "fail";
                } 
            }
        );
    }


    public void requestAppPurchaseStatus(){
        MadConnect.Instance.requestAppPurchaseStatus(
            (purchased, userID)=>{
                Debug.LogWarning("requestAppPurchaseStatus: success: purchased "+purchased+" userID = "+userID);
                //success   
                if(appPurchaseLabel!=null){
                    if(purchased){
                        //purchased: true
                        appPurchaseLabel.text = userID;
                    } else{
                        //purchased: false
                        appPurchaseLabel.text = userID;
                    }
                    
                } 
            },
            (errorMsg) =>{
                Debug.Log("requestAppPurchaseStatus: fail: errorMsg = "+errorMsg);
                //fail
                if(appPurchaseLabel!=null){
                    appPurchaseLabel.text = errorMsg;
                }
            }
        );
    }

    public void calibrateGyroscopeConnector(){
        if(MadConnect.Instance.isGyroCalibrated()){
            sensorLabel.text = "success";
        } else{
            MadConnect.Instance.calibrateGyroscope(
                ()=>{
                    //success    
                    sensorLabel.text = "success";
                },
                (errorMsg) =>{
                    //fail
                    sensorLabel.text = "fail";
                }
            );
        }
    }


    public void startGyroCalibration(){
        if(sensorLabel!=null)
            sensorLabel.text = "loading...";
            
             MadConnect.Instance.startGyroCalibration(5,
                ()=>{
                    //success
                    if(sensorLabel!=null)    
                    sensorLabel.text = "success";
                },
                () =>{
                    //fail
                    if(sensorLabel!=null)
                    sensorLabel.text = "fail";
                }
            );
    }
}
