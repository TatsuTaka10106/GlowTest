using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using MADGazeSDK;

public class MADGazeHandGesture : MonoBehaviour
{
    public bool enableDebugMode;
    public bool showSkeleton;

    public bool enableCursorOnStartup, enableSignalOnStartup, enableGrabOnStartup, enableRawTrackingOnStartup;
    public bool enableCursorControl, enableSignalControl, enableGrabControl, enableRawTrackingControl;
    public HandSignalCallback handSignalCallback;
    public HandCursorCallback handCursorCallback;
    public HandGrabCallback handGrabCallback;
    public HandTrackingCallback handTrackingCallback;

    MADSDKIntegratorHandCursor mMADSDKIntegratorHandCursor;
    MADSDKIntegratorHandSignal mMADSDKIntegratorHandSignal;
    MADSDKIntegratorTrackedHand mMADSDKIntegratorTrackedHand;
    MADSDKIntegratorGrab mMADSDKIntegratorGrab;

    void Start()
    {
        Init();
    }

    void Init(){
        HandGestureManager.Instance.DEBUG_MODE = enableDebugMode;
        HandGestureManager.Instance.SHOW_SKELETON = showSkeleton;
        if (enableSignalControl){
            HandGestureManager.Instance.Controller<HandSignalController>().registerCallbackFromInspector(handSignalCallback, enableSignalOnStartup);
        }
        if (enableCursorControl){
            HandGestureManager.Instance.Controller<HandCursorController>().registerCallbackFromInspector(handCursorCallback, enableCursorOnStartup);
        }
        if (enableGrabControl){
            HandGestureManager.Instance.Controller<HandGrabController>().registerCallbackFromInspector(handGrabCallback, enableGrabOnStartup);
        }
        if (enableRawTrackingControl){
            HandGestureManager.Instance.Controller<HandTrackingController>().registerCallbackFromInspector(handTrackingCallback, enableGrabOnStartup);
        }

        mMADSDKIntegratorHandSignal = new MADSDKIntegratorHandSignal();
        mMADSDKIntegratorHandCursor = new MADSDKIntegratorHandCursor();
        mMADSDKIntegratorTrackedHand = new MADSDKIntegratorTrackedHand();
        mMADSDKIntegratorGrab = new MADSDKIntegratorGrab();


        mMADSDKIntegratorHandSignal.OnStart();
        mMADSDKIntegratorHandCursor.OnStart();
        mMADSDKIntegratorTrackedHand.OnStart();
        mMADSDKIntegratorGrab.OnStart();

    }
     void OnDestroy(){
        if (enableSignalControl){
            HandGestureManager.Instance.Controller<HandSignalController>().unregisterCallbackFromInspector(handSignalCallback);
            handSignalCallback = null;
        }
        if (enableCursorControl){
            HandGestureManager.Instance.Controller<HandCursorController>().unregisterCallbackFromInspector(handCursorCallback);
            handCursorCallback = null;
        }
        if (enableGrabControl){
            HandGestureManager.Instance.Controller<HandGrabController>().unregisterCallbackFromInspector(handGrabCallback);
            handGrabCallback = null;
        }
        if (enableRawTrackingControl){
            HandGestureManager.Instance.Controller<HandTrackingController>().unregisterCallbackFromInspector(handTrackingCallback);
            handTrackingCallback = null;
        }
        HandGestureManager.Instance.Destroy();


        mMADSDKIntegratorHandSignal.OnDestroy();
        mMADSDKIntegratorHandCursor.OnDestroy();
        mMADSDKIntegratorTrackedHand.OnDestroy();
        mMADSDKIntegratorGrab.OnDestroy();
    }
}
