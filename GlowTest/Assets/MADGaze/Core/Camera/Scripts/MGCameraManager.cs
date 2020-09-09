using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGCameraManager : MonoBehaviour , ISplitCameraCallback
{
    public bool enableCameraOnStartup = true;
	public bool showCameraPreview = true;
	

    public void initCallback()
    {

        SplitCamera.Instance.setDefaultCameraCallback(this); 
    }

   public void onConnected(){
	   Debug.Log("MGCameraManager: onConnected");
		//Connected Camera 
        if(enableCameraOnStartup){
			Debug.Log("MGCameraManager: startPreview");
		    SplitCamera.Instance.startPreview();
        }
	}
	
	public void onDisconnected(){
		//onDisconnected Camera 	
	}
	
	public void onError(int errorCode){
		//onError Camera errorCode 

	}
}
