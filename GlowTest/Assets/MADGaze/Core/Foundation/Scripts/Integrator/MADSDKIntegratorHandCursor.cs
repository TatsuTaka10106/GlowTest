using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MADGazeSDK{
public class MADSDKIntegratorHandCursor : BaseMADSDKIntegrator
{

bool test;
    public MADSDKIntegratorHandCursor(){

    }

    public override void OnStart()
    {
        //reg click event 
        MADUnityIntegrator.EventClick += this.OnMADHGClickEvent;
        //reg Hand Detected event 
        MADUnityIntegrator.EventHandDetected += this.OnMADHGHandDetectedEvent;
     
    }

    public override void OnDestroy()
    {
        //unreg click event 
        MADUnityIntegrator.EventClick -= this.OnMADHGClickEvent;
        //unreg Hand Detected event 
        MADUnityIntegrator.EventHandDetected -= this.OnMADHGHandDetectedEvent;
       
    }

    //Click event callback
    void OnMADHGClickEvent(Click click)
    {
        if(HandGestureManager.Instance.isEnabled<HandCursorController>()){
            HandData handData = lastHandData[click.index];
            Vector3 vector2 = new Vector3(0, 0, 0);
       
            if(handData!=null){
                vector2.Set(handData.fingers[0].point.X, Screen.height - handData.fingers[0].point.Y, 0);
            }

            if(handData.isLeftHand){
                HandGestureManager.Instance.sendMessage<HandCursorController>(
                    HandCursor.Action.CLICKED,
                    HandCursor.Direction.LEFT, 
                    new Vector3(click.x, Screen.height - click.y, 0),
                    vector2);
            }else{
                HandGestureManager.Instance.sendMessage<HandCursorController>(
                    HandCursor.Action.CLICKED,
                    HandCursor.Direction.RIGHT, 
                    new Vector3(click.x, Screen.height - click.y, 0),
                    vector2);
            } 
        }
    }

    bool haveLeftHandSignal = false;
    bool haveRightHandSignal = false;

    int leftHandStatus = 0;
    int rightHandStatus = 0;

    Vector3[] lastPalmCenter = new Vector3[2];
    HandData[] lastHandData = new HandData[2];
    //Hand Detected event callback
    void OnMADHGHandDetectedEvent(HandDetected handDetected)
    {
        if(HandGestureManager.Instance.isEnabled<HandCursorController>()){
        if (handDetected != null)
        {
            Hand hand = handDetected.hand;
            if (hand != null)
            {
                int count = hand.handDatas.Count;

                leftHandStatus = 0;
                rightHandStatus = 0;

                if (count > 0)
                {


                    for (int i = 0; i < count; i++)
                    {

                        HandData handData = hand.handDatas[i];
                        int index = handData.isLeftHand ? 0:1;
                        lastHandData[index] = handData;

                        HandType handType = handData.handType;

                        Vector3 vector1 = new Vector3(0, 0, 0);
                        Vector3 vector2 = new Vector3(0, 0, 0);

                        vector1.Set(handData.palmCenterPoint.X, Screen.height - handData.palmCenterPoint.Y, 0);
                        vector2.Set(0, 0, 0);

                        if (hand.handDatas[i].isLeftHand)
                        {
                            //left Hand
                            leftHandStatus = 1;
                         
                            if(haveLeftHandSignal == false){
                                HandGestureManager.Instance.sendMessage<HandCursorController>(
                                    HandCursor.Action.TRACKED,
                                    HandCursor.Direction.LEFT, 
                                    vector1
                                );
                            }else{
                                HandGestureManager.Instance.sendMessage<HandCursorController>(
                                    HandCursor.Action.MOVED,
                                    HandCursor.Direction.LEFT, 
                                    vector1,
                                    lastPalmCenter[i]
                                );
                            }
                            lastPalmCenter[i] = vector1;
                        }
                        else
                        {

                            rightHandStatus = 1;

                            
                            if(haveRightHandSignal == false){
                                HandGestureManager.Instance.sendMessage<HandCursorController>(
                                    HandCursor.Action.TRACKED,
                                    HandCursor.Direction.RIGHT, 
                                    vector1
                                );
                            }else{
                                HandGestureManager.Instance.sendMessage<HandCursorController>(
                                    HandCursor.Action.MOVED,
                                    HandCursor.Direction.RIGHT, 
                                    vector1,
                                    lastPalmCenter[i]
                                );
                            }
                            lastPalmCenter[i] = vector1;

                        }


                        if (leftHandStatus == 1)
                        {
                            haveLeftHandSignal = true;
                        }
                        else
                        {
                            if (haveLeftHandSignal)
                            { 
                               haveLeftHandSignal = false;
                            }
                        }

                        if (rightHandStatus == 1)
                        {
                            haveRightHandSignal = true;
                        }
                        else
                        {
                            if (haveRightHandSignal)
                            {
                                haveRightHandSignal = false;
                            }
                        }
                    }
                }
                else
                {
                    //no hand
                            if (haveLeftHandSignal || haveRightHandSignal)
                            {
                               //Hand Cursor
                                 HandGestureManager.Instance.sendMessage<HandCursorController>(
                                        HandCursor.Action.LOST
                                    );

                                    lastPalmCenter[0].Set(0,0,0);
                                    lastPalmCenter[1].Set(0,0,0);

                                    haveLeftHandSignal = false;
                                    haveRightHandSignal = false;
                            }
                }
            }
        }
    }
}
}
}
