using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace MADGazeSDK {
   public class MadConnect {
      private static MadConnect instance = null;  
      public static MadConnect Instance
      {
         get
         { 
            if (instance == null) 
                instance = new MadConnect();
            return instance; 
         }
      }


      AndroidJavaObject nativeController;
      Action<string> mIMadIdCallback;

    private AppPurchaseStatusCallback mAppPurchaseStatusCallback;
    private Action<bool,string> mActionAppPurchaseStatusCallbackSuccess;
    private Action<string> mActionAppPurchaseStatusCallbackFail;
    
    private SensorCalibrationGyroCallback mSensorCalibrationGyroCallback;
    private Action mActionSensorCalibrationGyroCallbackSuccess;
    private Action<string> mActionSensorCalibrationGyroCallbackFail;
    
    private MadIdCallback mMadIdCallback;
    private Action<string> mActionMadIdCallbackSuccess;
    private Action<string> mActionMadIdCallbackFail;


    private CalibrateGyroCallback mCalibrateGyroCallback;
    private Action mActionCalibrateGyroCallbackSuccess;
    private Action mActionCalibrateGyroCallbackFail;

    public MadConnect()
    {
		init();
    }
	
	protected void init(){
        #if UNITY_ANDROID
            nativeController = MADGazeManager.Instance.getFunction("getMadConnectManager");
        #endif
    }
 
        public void calibrateGyroscope(Action onSuccess, Action<string> onFail){
                #if UNITY_ANDROID
                    if(nativeController!=null){

                        mActionSensorCalibrationGyroCallbackSuccess = onSuccess;
                        mActionSensorCalibrationGyroCallbackFail = onFail;

                        if(mSensorCalibrationGyroCallback==null){
                            mSensorCalibrationGyroCallback = new SensorCalibrationGyroCallback();
                        }
                        nativeController.Call("calibrateGyroscope",mSensorCalibrationGyroCallback);
                    }
                #endif    
        }


        public bool isGyroCalibrated(){
                #if UNITY_ANDROID
                    if(nativeController!=null){
                        return nativeController.Call<bool>("isGyroCalibrated");
                    }
                #endif
                return false;    
        }


        public void requestAppPurchaseStatus(Action<bool,string> onSuccess, Action<string> onFail){
            #if UNITY_ANDROID
                    if(nativeController!=null){

                        mActionAppPurchaseStatusCallbackSuccess = onSuccess;
                        mActionAppPurchaseStatusCallbackFail = onFail;

                        if(mAppPurchaseStatusCallback==null){
                            mAppPurchaseStatusCallback = new AppPurchaseStatusCallback();
                        }
                        nativeController.Call("requestAppPurchaseStatus", MADCallbackManager.SDK_API_KEY, mAppPurchaseStatusCallback);
                    }
                #endif    
        }

         public void requestMadId(Action<string> onSuccess, Action<string> onFail){
            #if UNITY_ANDROID
                    if(nativeController!=null){
                        mActionMadIdCallbackSuccess = onSuccess;
                        mActionMadIdCallbackFail = onFail;

                        if(mMadIdCallback==null){
                            mMadIdCallback = new MadIdCallback();
                        }
                        nativeController.Call("requestMadId", mMadIdCallback);
                    }
                #endif    
        }


        public void startGyroCalibration(int delayTime,Action onSuccess, Action onFail){
            #if UNITY_ANDROID
                    if(nativeController!=null){

                        mActionCalibrateGyroCallbackSuccess = onSuccess;
                        mActionCalibrateGyroCallbackFail = onFail;

                        if(mCalibrateGyroCallback==null){
                            mCalibrateGyroCallback = new CalibrateGyroCallback();
                        }
                        nativeController.Call("startGyroCalibration", delayTime, mCalibrateGyroCallback);
                    }
                #endif    
        }

        class AppPurchaseStatusCallback : AndroidJavaProxy
    	{
        	public AppPurchaseStatusCallback() : base("com.madgaze.unityplugin.manager.MadConnectManager$AppPurchaseStatusCallback") {}
             async void onSuccess(bool purchased, string id){
                await MADCallbackManager.Instance.EnqueueAsync(()=>MadConnect.Instance.onAppPurchaseStatusSuccess(purchased,id));
             }
            async void onFail(string errorMsg){
                await MADCallbackManager.Instance.EnqueueAsync(()=>MadConnect.Instance.onAppPurchaseStatusFail(errorMsg));
            }
        }
        public void onAppPurchaseStatusSuccess(bool purchased,string id){
            if(mActionAppPurchaseStatusCallbackSuccess!=null){
                mActionAppPurchaseStatusCallbackSuccess(purchased,id);
            }
        }

        public void onAppPurchaseStatusFail(string errorMsg){
             if(mActionAppPurchaseStatusCallbackFail!=null){
                mActionAppPurchaseStatusCallbackFail(errorMsg);
            }
        }

        class SensorCalibrationGyroCallback : AndroidJavaProxy
    	{
        	public SensorCalibrationGyroCallback() : base("com.madgaze.unityplugin.manager.MadConnectManager$SensorCalibrationGyroCallback") {}
         	async void onSuccess(){
                await MADCallbackManager.Instance.EnqueueAsync(()=>MadConnect.Instance.onSensorCalibrationGyroSuccess());
             }
            async void onFail(string errorMsg){
                await MADCallbackManager.Instance.EnqueueAsync(()=>MadConnect.Instance.onSensorCalibrationGyroFail(errorMsg));
            }
        }

        public void onSensorCalibrationGyroSuccess(){
                if(mActionSensorCalibrationGyroCallbackSuccess!=null){
                    mActionSensorCalibrationGyroCallbackSuccess();
                }
        }

        public void onSensorCalibrationGyroFail(string errorMsg){
                if(mActionSensorCalibrationGyroCallbackFail!=null){
                    mActionSensorCalibrationGyroCallbackFail(errorMsg);
                }
        }


        class MadIdCallback : AndroidJavaProxy
    	{
        	public MadIdCallback() : base("com.madgaze.unityplugin.manager.MadConnectManager$MadIdCallback") {}
         	async void onSuccess(string id){
                await MADCallbackManager.Instance.EnqueueAsync(()=>MadConnect.Instance.onMadIdSuccess(id));
            }
            async void onFail(string errorMsg){
                await MADCallbackManager.Instance.EnqueueAsync(()=>MadConnect.Instance.onMadIdFail( errorMsg));
            }
        }

        public void onMadIdSuccess(string id){
                if(mActionMadIdCallbackSuccess!=null){
                    mActionMadIdCallbackSuccess(id);
                }
        }

        public void onMadIdFail(string errorMsg){
                if(mActionMadIdCallbackFail!=null){
                    mActionMadIdCallbackFail(errorMsg);
                }
        }


         class CalibrateGyroCallback : AndroidJavaProxy
    	{
        	public CalibrateGyroCallback() : base("com.madgaze.unityplugin.manager.MadConnectManager$CalibrateGyroCallback") {}
         	async void calibrateSuccess(){
                await MADCallbackManager.Instance.EnqueueAsync(()=>MadConnect.Instance.onCalibrateGyroSuccess());
             }
            async void calibrateFail(){
                await MADCallbackManager.Instance.EnqueueAsync(()=>MadConnect.Instance.onCalibrateGyroFail());
            }
        }

        public void onCalibrateGyroSuccess(){
            if(mActionCalibrateGyroCallbackSuccess!=null){
                mActionCalibrateGyroCallbackSuccess();
            }
        }

        public void onCalibrateGyroFail(){
            if(mActionCalibrateGyroCallbackFail!=null){
                mActionCalibrateGyroCallbackFail();
            }
        }



   }
}