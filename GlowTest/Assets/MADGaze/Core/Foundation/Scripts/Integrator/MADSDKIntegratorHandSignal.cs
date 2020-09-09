using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MADGazeSDK{
public class MADSDKIntegratorHandSignal : BaseMADSDKIntegrator
{

    public MADSDKIntegratorHandSignal(){

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

   

    bool haveLeftHandSignal = false;
    bool haveRightHandSignal = false;

    int leftHandStatus = 0;
    int rightHandStatus = 0;
    bool isFirst;
    //Hand Detected event callback
    void OnMADHGHandDetectedEvent(HandDetected handDetected)
    {
        if(HandGestureManager.Instance.isEnabled<HandSignalController>()){
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
                        HandType handType = handData.handType;
                        HandSignal.Type signalType = HandSignal.Type.UNKNOWN;

                        Vector3 vector1 = new Vector3(0, 0, 0);
                        Vector3 vector2 = new Vector3(0, 0, 0);

                        vector1.Set(handData.palmCenterPoint.X, Screen.height - handData.palmCenterPoint.Y, 0);
                        vector2.Set(0, 0, 0);

                        switch (handType)
                        {
                            case HandType.ONE:
                                signalType = HandSignal.Type.SIGNAL_ONE;
                                vector2.Set(handData.fingers[0].point.X, Screen.height - handData.fingers[0].point.Y, 0);
                                break;
                            case HandType.YEAH:
                                signalType = HandSignal.Type.SINGAL_TWO;
                                break;
                            case HandType.THREE:
                                signalType = HandSignal.Type.SIGNAL_THREE;
                                break;
                            case HandType.FOUR:
                                signalType = HandSignal.Type.SIGNAL_FOUR;
                                break;
                            case HandType.FIVE:
                                signalType = HandSignal.Type.SIGNAL_FIVE;
                                break;
                            case HandType.OK:
                                signalType = HandSignal.Type.SIGNAL_OK;
                                vector2.Set(handData.fingers[0].point.X, Screen.height - handData.fingers[0].point.Y, 0);
                                break;
                            case HandType.FIST:
                                signalType = HandSignal.Type.SIGNAL_FIST;
                                break;
                            default:
                                signalType = HandSignal.Type.UNKNOWN;
                                break;
                        }

                        if (hand.handDatas[i].isLeftHand)
                        {
                            //left Hand
                            leftHandStatus = 1;
                            
                          
                              //Hand Signal 
                            HandGestureManager.Instance.sendMessage<HandSignalController>(
                                HandSignal.Action.TRACKED,
                                signalType,
                                HandSignal.Direction.LEFT,
                                vector1, vector2);

                        }
                        else
                        {

                            rightHandStatus = 1;

                              //Hand Signal 
                            HandGestureManager.Instance.sendMessage<HandSignalController>(
                                HandSignal.Action.TRACKED,
                                signalType,
                                HandSignal.Direction.RIGHT,
                                vector1, vector2);

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
                    if (haveLeftHandSignal || haveRightHandSignal) {
                        HandGestureManager.Instance.sendMessage<HandSignalController>(
                            HandSignal.Action.LOST);

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
