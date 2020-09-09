using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMADGazeMonoBehaviour : MonoBehaviour {
	
   		//split camera
		public virtual void onSplitCameraConnected(string key){}
		public virtual void onSplitCameraDisconnected(string key){}
		public virtual void onSplitCameraError(string key){}

		//Sensor
		public virtual void onCalibratedGyroscope(float x, float y, float z, float w){}
		public virtual void accelerator(float x, float y, float z){}
        public virtual void gyroscope(float x, float y, float z, float rotationX,float rotationY,float rotationZ){}

		public virtual void onError(string errorCode){}

       
		public void closeApp(){
			 
			 #if UNITY_ANDROID
			  using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				 using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					  jo.Call("closeApp");
				}
			}
			#endif
		}


		public void setSensorListener(AndroidJavaProxy callback){
			 #if UNITY_ANDROID
			  using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				 using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					  jo.Call("setSensorListener",callback);
				}
			}
			#endif
		}


				
}
