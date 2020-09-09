using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MADGazeManager {
 
 	private static MADGazeManager _instance;
 
    public static MADGazeManager Instance
    {
        get
        {          
			if (_instance == null){
        		_instance = new MADGazeManager();
			}
            return _instance;
        }
    }

	public AndroidJavaObject getFunction(string functionName){
		AndroidJavaObject nativeController = null;
		#if UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			if(jc!=null){
	   			AndroidJavaObject currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity"); 
				if(currentActivity!=null){
					nativeController = currentActivity.Call<AndroidJavaObject>(functionName); 
				}
			}
		 #endif
		return nativeController;
	}

	public void callFunction(string functionName){
		#if UNITY_ANDROID
	
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		if(jc!= null){
	   		AndroidJavaObject currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity"); 
	   		if(currentActivity!=null){
				currentActivity.Call(functionName); 
			}
		}
		#endif
	}

}