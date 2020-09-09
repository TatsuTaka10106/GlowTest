using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MADGazeSDK;

public class MADHandGesture
{
    private const string JAVA_GESTURE_LISTENER_CLICK = "com.madgaze.handgesture.listener.MADHandGestureBehaviourClickListener";
    private const string JAVA_GESTURE_LISTENER_GRAP = "com.madgaze.handgesture.listener.MADHandGestureBehaviourGrapListener";
    private const string JAVA_GESTURE_LISTENER_HOLD = "com.madgaze.handgesture.listener.MADHandGestureBehaviourHoldPostListener";
    private const string JAVA_GESTURE_LISTENER_DETECTED = "com.madgaze.handgesture.listener.MADHandGestureDetectListener";
    private const string JAVA_GESTURE_CALLBACK_ONREADY = "com.madgaze.handgesture.listener.MADHandGestureReadyCallback";
    private const string JAVA_ERROR_CALLBACK = "com.madgaze.handgesture.listener.ErrorCallback";

    AndroidJavaObject nativeController;
    AndroidJavaObject nativeMADHandGesture;
    private static MADHandGesture _instance;
    public static MADHandGesture Instance
    {
        get
        {   
            if(_instance == null){
                _instance = new MADHandGesture();
            }
            return _instance;
        }
    }


	private IBehaviourClickListener mIBehaviourClickListener;
    private BehaviourClickListener nativeBehaviourClickListener;

    private IBehaviourGrapListener mIBehaviourGrapListener;
    private BehaviourGrapListener nativeBehaviourGrapListener;
    
    private IBehaviourHoldPostListener mIBehaviourHoldPostListener;
    private BehaviourHoldPostListener nativeBehaviourHoldPostListener;

    private IHandGestureDetectedListener mIHandGestureDetectedListener;
    private HandGestureDetectedListener navtiveHandGestureDetectedListener;

    private IHandGestureErrorCallback mIHandGestureErrorCallback;

    public const int CAMERA = 10001;
    public const int PERMISSION = -101;
    public const int GRANT_PERMISSION_FAILED = -102;
    public const int BINDSERVICE_FAILED = -201;
    public const int NOT_SUPPORT_GESUTRE = -999;

    private Hand mHand;
    public MADHandGesture() { }

    public void init()
    {
        #if UNITY_ANDROID
        nativeController = MADGazeManager.Instance.getFunction("getSpliteCameraMananger");

        if (nativeController != null)
        {
            //nativeController.Call("enableHandGesture"); 
            nativeMADHandGesture = nativeController.Call<AndroidJavaObject>("getUnityMADHandGesture");
        }

        mHand = new Hand();
        #endif
    }

	#region Click
	class BehaviourClickListener : AndroidJavaProxy
    {
        public BehaviourClickListener() : base(JAVA_GESTURE_LISTENER_CLICK) { }
        void OnClick(int index, int x, int y) => Instance.behaviourOnClick(index, x, y);
    }

    public void behaviourOnClick(int index, int x, int y)
    {
        if (mIBehaviourClickListener != null)
        {
            mIBehaviourClickListener.onClick(index, x, y);
        }
    }

    public void addBehavioursClick(IBehaviourClickListener listener)
    {
        if (nativeMADHandGesture != null)
        {
            mIBehaviourClickListener = listener;
            if(listener !=null){
                if(nativeBehaviourClickListener==null){
                    nativeBehaviourClickListener = new BehaviourClickListener();
                }
                Debug.Log("Unity:MADHandGesture: addBehavioursClick");
                nativeMADHandGesture.Call("addGestureBehavioursClickListener", nativeBehaviourClickListener);
            }else{
                Debug.Log("Unity:MADHandGesture: remove addBehavioursClick");
                nativeMADHandGesture.Call("addGestureBehavioursClickListener", null);
            }
            
        }
    }
	#endregion // Click

	#region Grap
	class BehaviourGrapListener : AndroidJavaProxy
    {
        public BehaviourGrapListener() : base(JAVA_GESTURE_LISTENER_GRAP) { }
        void OnGrabStart(int index, int x, int y) => Instance.behaviourOnGrabStart(index, x, y);
        void OnGrabHolding(int index, int x, int y, int dx, int dy) => Instance.behaviourOnGrabHolding(index, x, y, dx, dy);
        void OnGrabReplease(int index, int x, int y, int dx, int dy) => Instance.behaviourOnGrabRelease(index, x, y, dx, dy);
        void OnGrabCancel(int index, int x, int y, int dx, int dy) => Instance.behaviourOnGrabCancel(index, x, y, dx, dy);
    }

    public void behaviourOnGrabStart(int index, int x, int y)
    {
        if (mIBehaviourGrapListener != null)
        {
            mIBehaviourGrapListener.onGrabStart(index, x, y);
        }
    }
    public void behaviourOnGrabHolding(int index, int x, int y, int dx, int dy)
    {
        if (mIBehaviourGrapListener != null)
        {
            mIBehaviourGrapListener.onGrabHolding(index, x, y, dx, dy);
        }
    }
    public void behaviourOnGrabRelease(int index, int x, int y, int dx, int dy)
    {
        if (mIBehaviourGrapListener != null)
        {
            mIBehaviourGrapListener.onGrabRelease(index, x, y, dx, dy);
        }
    }
    public void behaviourOnGrabCancel(int index, int x, int y, int dx, int dy)
    {
        if (mIBehaviourGrapListener != null)
        {
            mIBehaviourGrapListener.onGrabCancel(index, x, y, dx, dy);
        }
    }


    public void addBehavioursGrab(IBehaviourGrapListener listener)
    {
        if (nativeMADHandGesture != null)
        {
            
            mIBehaviourGrapListener = listener;
            if(listener !=null){
            if(nativeBehaviourGrapListener==null){
                nativeBehaviourGrapListener = new BehaviourGrapListener();
            }
                Debug.Log("Unity:MADHandGesture: addBehavioursGrab");
                nativeMADHandGesture.Call("addGestureBehavioursGrapListener", nativeBehaviourGrapListener);
            }else{
                Debug.Log("Unity:MADHandGesture: remove addBehavioursGrab");
                nativeMADHandGesture.Call("addGestureBehavioursGrapListener", null);
            }
        }
    }
    #endregion // Grab

    #region Hold Gesture
    class BehaviourHoldPostListener : AndroidJavaProxy
    {
        public BehaviourHoldPostListener() : base(JAVA_GESTURE_LISTENER_HOLD) { }

        void OnHold(int index, string handType, int x, int y) => Instance.behaviourOnHold(index, handType, x, y);

        void OnHoldCancel(int index, string handType, int x, int y) => Instance.behaviourOnHoldCancel(index, handType, x, y);
    }

    public void behaviourOnHold(int index, string handType, int x, int y)
    {
        if (mIBehaviourHoldPostListener != null)
        {
            mIBehaviourHoldPostListener.onHold(index, handType, x, y);
        }
    }

    public void behaviourOnHoldCancel(int index, string handType, int x, int y)
    {
        if (mIBehaviourHoldPostListener != null)
        {
            mIBehaviourHoldPostListener.onHoldCancel(index, handType, x, y);
        }
    }

    public void addBehavioursHoldPost(IBehaviourHoldPostListener listener)
    {
        if (nativeMADHandGesture != null)
        {
            mIBehaviourHoldPostListener = listener;
              if(listener !=null){
                if(nativeBehaviourHoldPostListener!=null){
                    nativeBehaviourHoldPostListener = new BehaviourHoldPostListener();
                }
                Debug.Log("Unity:MADHandGesture: addBehavioursHoldPos");
                nativeMADHandGesture.Call("addGestureBehavioursHoldPostListener", nativeBehaviourHoldPostListener);
            }else{
                Debug.Log("Unity:MADHandGesture: remove addBehavioursHoldPos");
                nativeMADHandGesture.Call("addGestureBehavioursHoldPostListener", null);
            }
            
        }
    }
    #endregion // Hold Gesture

    #region Detector
    class HandGestureDetectedListener : AndroidJavaProxy
    {
        public HandGestureDetectedListener() : base(JAVA_GESTURE_LISTENER_DETECTED) { }
        void onHandDetected(AndroidJavaObject hand, bool isHand) => Instance.onHandDetected(hand, isHand);
    }

    public void onHandDetected(AndroidJavaObject hand, bool isHand)
    {
        //Debug.Log("DemoHandGesture: onHandDetected : isHand = " + isHand);
        if (mHand != null)
        {
            mHand.updateData(hand);
        }

        if (mIHandGestureDetectedListener != null)
        {
            mIHandGestureDetectedListener.onHandDetected(mHand, isHand);
        }
    }

    public void addHandGestureDetectedListener(IHandGestureDetectedListener listener)
    {
        if (nativeMADHandGesture != null)
        {

            mIHandGestureDetectedListener = listener;

            if(listener != null){
                if(navtiveHandGestureDetectedListener == null){
                    navtiveHandGestureDetectedListener = new HandGestureDetectedListener();
                }
                nativeMADHandGesture.Call("addGestureDetectListener", navtiveHandGestureDetectedListener);   
                //nativeMADHandGesture.Call("addGestureDetectListener", new HandGestureDetectedListener());
            }else{
                Debug.Log("Unity:MADHandGesture: remove HandGestureListener");
                nativeMADHandGesture.Call("addGestureDetectListener", null);
            }

            
        }
    }
	#endregion // Detector

	#region Hand Gesture Ready
	IHandGestureReadyCallback mIHandGestureReadyCallback;
    HandGestureReadyCallback nativeHandGestureReadyCallback;
    class HandGestureReadyCallback : AndroidJavaProxy
    {
        public HandGestureReadyCallback() : base(JAVA_GESTURE_CALLBACK_ONREADY) { }
        void onReady() => Instance.onHandGestureReady();
    }

    public void onHandGestureReady()
    {
        if (mIHandGestureReadyCallback != null)
        {
            mIHandGestureReadyCallback.onHandGestureReady();
        }
    }

    public void setIHandGestureReadyCallback(IHandGestureReadyCallback callback)
    {
        if (nativeMADHandGesture != null)
        {
            Debug.Log("Unity:MADHandGesture: setOnHandGestureReadyCallback");
            
            mIHandGestureReadyCallback = callback;

            if(nativeHandGestureReadyCallback == null){
                nativeHandGestureReadyCallback = new HandGestureReadyCallback();
            }
            nativeMADHandGesture.Call("setOnHandGestureReadyCallback", nativeHandGestureReadyCallback);
             // nativeMADHandGesture.Call("setOnHandGestureReadyCallback", new HandGestureReadyCallback());
         
        }
    }
    #endregion // Hand Gesture Ready

    #region Error
    class ErrorCallback : AndroidJavaProxy
    {
        public ErrorCallback() : base(JAVA_ERROR_CALLBACK) { }
        void onError(int errorCode, String errorMessage) => Instance.onErrorCallback(errorCode, errorMessage);
    }

    public void onErrorCallback(int errorCode, String errorMessage)
    {
        if (mIHandGestureErrorCallback != null)
        {
            mIHandGestureErrorCallback.onErrorCallback(errorCode, errorMessage);
        }
    }

    public void setErrorCallback(IHandGestureErrorCallback callback)
    {
        mIHandGestureErrorCallback = callback;

        if (nativeController != null)
        {
            // Debug.Log("MADHandGesture: setErrorCallback");
            var javaCallback = new ErrorCallback();
            nativeController.Call("setErrorCallback", javaCallback);
        }
    }
	#endregion // Error

	#region Internal service handler
	public void startHandGesture()
    {
        if (nativeController != null)
        {
                // Debug.Log("Unity:MADHandGesture: startHandGesture");
                nativeController.Call("startHandGesture");
        }
    }

    public void stopHandGesture()
    {
        if (nativeController != null)
        {
            // Debug.Log("MADHandGesture: stopHandGesture");
            nativeController.Call("stopHandGesture");
        }
    }

     public bool isServiceConnected(){
       if (nativeController != null)
        {
            // Debug.Log("MADHandGesture: isServiceConnected");
            return nativeController.Call<bool>("isServiceConnected");
        }
        return false;
    }

    public bool isHandGestureRunning(){
       if (nativeController != null)
        {
            //Debug.Log("MADHandGesture: isHandGestureRunning");
            return nativeController.Call<bool>("isHandGestureRunning");
        }
        return false;
    }

      public void showHandSkeleton(bool isShow){
       if (nativeController != null)
        {
            // Debug.Log("MADHandGesture: showHandSkeleton");
            nativeMADHandGesture.Call("showHandSkeleton",isShow);
        }
    
    }

   public byte[] getPreviewResult(){	
			if(nativeController!=null){
				AndroidJavaObject bufferObject = nativeController.Call<AndroidJavaObject>("getHandGesturePreivewByte");
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
    #endregion // Internal service handler
}