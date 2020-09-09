using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace MADGazeSDK {
   public class MadIdManager {
      private static MadIdManager instance = null;  
      public static MadIdManager Instance
      {
         get
         { 
            if (instance == null) 
                instance = new MadIdManager();
            return instance; 
         }
      }


      AndroidJavaObject nativeController;
      Action<string> mIMadIdCallback;
      public MadIdManager()
   {
		init();
   }
	
	protected void init(){
        #if UNITY_ANDROID
            if(MADGazeManager.Instance!=null){
                nativeController = MADGazeManager.Instance.getFunction("getMadIdManager");
            }
            
        #endif
    }


        public void getMadIdFormConnector(){
                #if UNITY_ANDROID
                    if(nativeController!=null){
                        nativeController.Call("getMadIdFormConnector");
                    }
                #endif    
        }
	    public void setMadIdCallback(Action<string> action){
                #if UNITY_ANDROID
                    if(nativeController!=null){
                        var callback1 = new MadIdCallback();
                        nativeController.Call("setCallback",callback1);
                    }
                #endif               
                mIMadIdCallback = action;
    	}

	    public void onMadIdReceived(string id){
    		if(mIMadIdCallback!=null){
                mIMadIdCallback(id);
            }
    	}

        class MadIdCallback : AndroidJavaProxy
    	{
        	public MadIdCallback() : base("com.madgaze.unityplugin.manager.MadIdManager$Callback") {}
         	async void onMadIdReceived(string id){
				await MADCallbackManager.Instance.EnqueueAsync(()=>MadIdManager.Instance.onMadIdReceived(id));
         	}
        }
   }
}