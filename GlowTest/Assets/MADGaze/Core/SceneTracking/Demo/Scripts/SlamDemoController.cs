using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADGazeSDK;

public class SlamDemoController : MonoBehaviour
{
	public MADGazeCamera MADGazeCamera;
    public void Start () {

	}
    public void toggleCameraPreview(){
        MADGazeCamera.showCameraPreview = !MADGazeCamera.showCameraPreview;
    }

    public void toggleCameraOnOff(){
		if (SplitCamera.Instance.isDeviceConnected()) {
			SplitCamera.Instance.stopPreview();
        } else {
			SplitCamera.Instance.startPreview();
        }
 	}

    public void resetTargetPosition(){
		MADGazeARManager.Instance.resetTargetPosition((isSuccess)=>{
                if(isSuccess){
                    Debug.Log("Place : true");
                } else{
                    Debug.Log("Place : false");
                }
        });
 	}

}
