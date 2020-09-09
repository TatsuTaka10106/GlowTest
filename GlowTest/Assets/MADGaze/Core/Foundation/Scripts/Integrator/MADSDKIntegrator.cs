using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MADGazeSDK;
public class MADSDKIntegrator : MonoBehaviour
{

    MADSDKIntegratorHandCursor mMADSDKIntegratorHandCursor;
    MADSDKIntegratorHandSignal mMADSDKIntegratorHandSignal;
    MADSDKIntegratorTrackedHand mMADSDKIntegratorTrackedHand;
    MADSDKIntegratorGrab mMADSDKIntegratorGrab;

    void Start()
    {

        mMADSDKIntegratorHandSignal = new MADSDKIntegratorHandSignal();
        mMADSDKIntegratorHandCursor = new MADSDKIntegratorHandCursor();
        mMADSDKIntegratorTrackedHand = new MADSDKIntegratorTrackedHand();
        mMADSDKIntegratorGrab = new MADSDKIntegratorGrab();


        mMADSDKIntegratorHandSignal.OnStart();
        mMADSDKIntegratorHandCursor.OnStart();
        mMADSDKIntegratorTrackedHand.OnStart();
        mMADSDKIntegratorGrab.OnStart();
    
    }

    void OnDestroy()
    {

        mMADSDKIntegratorHandSignal.OnDestroy();
        mMADSDKIntegratorHandCursor.OnDestroy();
        mMADSDKIntegratorTrackedHand.OnDestroy();
        mMADSDKIntegratorGrab.OnDestroy();
    
    }

}
