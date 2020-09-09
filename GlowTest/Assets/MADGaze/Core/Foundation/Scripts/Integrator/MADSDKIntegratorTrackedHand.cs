using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MADGazeSDK{
    
public class MADSDKIntegratorTrackedHand : BaseMADSDKIntegrator
{

    public MADSDKIntegratorTrackedHand(){

    }

   public override void OnStart()
    {
        //reg Hand Detected event 
        MADUnityIntegrator.EventHandDetected += this.OnMADHGHandDetectedEvent;
     
    }

    public override void OnDestroy()
    {
       //unreg Hand Detected event 
        MADUnityIntegrator.EventHandDetected -= this.OnMADHGHandDetectedEvent;
       
    }

    //Hand Detected event callback
    void OnMADHGHandDetectedEvent(HandDetected handDetected)
    {
        if(HandGestureManager.Instance.isEnabled<HandTrackingController>()){
        if (handDetected != null)
        {
            Hand hand = handDetected.hand;
            if (hand != null)
            {
                int count = hand.handDatas.Count;
                if (count > 0)
                {
                    HandData leftHandData = null;
                    HandData rightHandData = null;

                    for (int i = 0; i < count; i++)
                    {
                        HandData handData = hand.handDatas[i];
                          if (handData.isLeftHand)
                            {
                                leftHandData = handData;
                            } else{
                                rightHandData = handData;
                            }   
                    }
                          HandGestureManager.Instance.sendMessage<HandTrackingController>(
                            TrackedHand.Action.TRACKING,
                            TrackedHand.parse(leftHandData),
                            TrackedHand.parse(rightHandData));
                }
                else
                {
                          HandGestureManager.Instance.sendMessage<HandTrackingController>(
                            TrackedHand.Action.TRACKING,
                            TrackedHand.parse(null),
                            TrackedHand.parse(null));
                }
            }
        }
    }
    }
}
}
