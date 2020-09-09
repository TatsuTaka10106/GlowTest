using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class SplitCamera
{

	AndroidJavaObject nativeController;
    private static SplitCamera _instance;
    public static SplitCamera Instance
    {
        get
        {   
            if(_instance == null){
                _instance = new SplitCamera();
            }
            return _instance;
        }
    }
	// public class SplitCameraDefaultCallback : ISplitCameraCallback {
	// 	public void onConnected(){}
    // 	public void onDisconnected(){}
    // 	public void onError(int code){}

	// }
	private ISplitCameraCallback mSplitCameraCallback;
	private ISplitCameraCallback mSplitDefaultCameraCallback;

	public SplitCamera()
   {
		init();
   }
	
	protected void init(){
        #if UNITY_ANDROID
			if(MADGazeManager.Instance!=null){
            	nativeController = MADGazeManager.Instance.getFunction("getSpliteCameraController");
			}
        #endif
    }


	public AndroidJavaObject getCameraCallback(){
		if(nativeController!=null){
			  AndroidJavaObject cameracallback = nativeController.Call<AndroidJavaObject>("getCustomSplitCameraCallback");
			  return cameracallback;
		}
		return null;
	}

	public void startPreview(){
		if(nativeController!=null){
			 nativeController.Call("startPreview");
		}
	}

	public void stopPreview(){
		if(nativeController!=null){
			nativeController.Call("stopPreview");
		}
	}
	
		public bool isDeviceConnected(){
			if(nativeController!=null){
				 return nativeController.Call<bool>("isDeviceConnected");
			}
			return false;
		}
	
		public int getPreviewWidth(){
			if(nativeController!=null){
				return nativeController.Call<int>("getPreviewWidth");
			}else{
				return 0;
			}
    	}

    	public int getPreviewHeight(){
     		if(nativeController!=null){
				return nativeController.Call<int>("getPreviewHeight");
			}else{
				return 0;
			}
    	}

 			public void setCameraCallback(ISplitCameraCallback callback){
                // #if UNITY_ANDROID
                //     if(nativeController!=null){
                //         var splitCameraCallback = new SplitCameraCallback();
                //         nativeController.Call("setCameraCallback",splitCameraCallback);
                //     }
                // #endif               
                mSplitCameraCallback = callback;
    		}

			public void setDefaultCameraCallback(ISplitCameraCallback callback){
                 #if UNITY_ANDROID
                    if(nativeController!=null){
                        var splitCameraCallback = new SplitCameraCallback();
                        nativeController.Call("setCameraCallback",splitCameraCallback);
                    }
                #endif 
				mSplitDefaultCameraCallback = callback;
    		}

		public byte[] getPreviewResult(){	
			if(nativeController!=null){
				AndroidJavaObject bufferObject = nativeController.Call<AndroidJavaObject>("getByte");
				if(bufferObject!=null){
					#if UNITY_2019_1_OR_NEWER
							sbyte[]	 buffer = AndroidJNIHelper.ConvertFromJNIArray<sbyte[]>(bufferObject.GetRawObject());
							byte[] bytes = (byte[]) (Array)buffer;
							return bytes;
					#else
							byte[] bytes = AndroidJNIHelper.ConvertFromJNIArray<byte[]>(bufferObject.GetRawObject());
							return bytes;
        			#endif	
				}
			}
			return null;
		}

		class SplitCameraCallback : AndroidJavaProxy
    	{
        	public SplitCameraCallback() : base("com.madgaze.unityplugin.SplitCameraCallback") {}
         	async void onConnected(){
				await MADCallbackManager.Instance.EnqueueAsync(()=>SplitCamera.Instance.onConnected());
         	}

    	 	async void onDisconnected(){
				await MADCallbackManager.Instance.EnqueueAsync(()=>SplitCamera.Instance.onDisconnected());
    	 	}

    	 	async void onError(int errorCode){
				await MADCallbackManager.Instance.EnqueueAsync(()=>SplitCamera.Instance.onError(errorCode));
    	 	}
    	}


    	public void onConnected(){
			if(mSplitDefaultCameraCallback!=null){
				mSplitDefaultCameraCallback.onConnected();
			}

    		if(mSplitCameraCallback!=null){
    			mSplitCameraCallback.onConnected();
    		}
    	}
    	
    	public void onDisconnected(){
			if(mSplitDefaultCameraCallback!=null){
				mSplitDefaultCameraCallback.onDisconnected();
			}
			if(mSplitCameraCallback!=null){
    			mSplitCameraCallback.onDisconnected();
    		}
    	}
	
		public void onError(int errorCode){
			if(mSplitDefaultCameraCallback!=null){
				mSplitDefaultCameraCallback.onError(errorCode);
			}

			if(mSplitCameraCallback!=null){
    			mSplitCameraCallback.onError(errorCode);
    		}
		}

		


	#region TakePictureCallback
	 
	TakePictureCallback nativeTakePictureCallback;
	public void takePicture(UnityAction<string> onImageSaved, UnityAction<int> onError){
		takePictureOnImageSaved = onImageSaved;
		takePictureOnError = onError;
		
		if(nativeController!=null){
			if(nativeTakePictureCallback==null){
				nativeTakePictureCallback = new TakePictureCallback();
			}
			nativeController.Call("takePicture", nativeTakePictureCallback);
		}
	}

	private UnityAction<string> takePictureOnImageSaved;
	private UnityAction<int> takePictureOnError;

    class TakePictureCallback : AndroidJavaProxy
    {
        public TakePictureCallback() : base("com.madgaze.smartglass.otg.TakePictureCallback") { }
        //void onImageSaved(string path) => Instance?.takePictureOnImageSaved?.Invoke(path);
		async void onImageSaved(string path){
			await MADCallbackManager.Instance.EnqueueAsync(()=>SplitCamera.Instance?.takePictureOnImageSaved?.Invoke(path));
		}
		
		async void onError(int errorCode){
			await MADCallbackManager.Instance.EnqueueAsync(()=>SplitCamera.Instance?.takePictureOnError?.Invoke(errorCode));
		}
    }

	#endregion // TakePictureCallback

	#region RecordVideoCallback

	private RecordVideoCallback nativeRecordVideoCallback;
	public void startRecording(){
		if(nativeController!=null){
			nativeController.Call("startRecording");
		}
	}

	public bool isRecording(){
		if(nativeController!=null){
			return nativeController.Call<bool>("isRecording");
		}
		return false;
	}

	public void stopRecording(){
		if(nativeController!=null){
			nativeController.Call("stopRecording");
		}
	}
	
	 public void setRecordVideoCallback(UnityAction<string> onVideoSaved, UnityAction<int> onError)
    {
        recordVideoOnVideoSaved = onVideoSaved;
		recordVideoOnError = onError;
		
		if(nativeController!=null){
			if(nativeRecordVideoCallback==null){
				nativeRecordVideoCallback = new RecordVideoCallback();
			}
			nativeController.Call("setRecordVideoCallback", nativeRecordVideoCallback);
		}
    }

	private UnityAction<string> recordVideoOnVideoSaved;
	private UnityAction<int> recordVideoOnError;

    class RecordVideoCallback : AndroidJavaProxy
    {
        public RecordVideoCallback() : base("com.madgaze.smartglass.otg.RecordVideoCallback") { }
    		async void onVideoSaved(string path){
				await MADCallbackManager.Instance.EnqueueAsync(()=>SplitCamera.Instance?.recordVideoOnVideoSaved?.Invoke(path));
			}
			async void onError(int errorCode){
				await MADCallbackManager.Instance.EnqueueAsync(()=>SplitCamera.Instance?.recordVideoOnError?.Invoke(errorCode));
			}
    }
	#endregion // RecordVideoCallback
}
