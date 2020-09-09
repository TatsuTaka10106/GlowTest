using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MADGazeSDK;

public class HandGestureSample : MonoBehaviour
{

   private GUIStyle guiStyle = new GUIStyle(); //create a new variable
   
   Vector2 handPosition = new Vector2(0,0);
   private string eventClickStr="";
   private string eventDetectedStr="";
   private string eventGrabStr="";
   private string eventHoldStr="";

    void Start()
    {
        //reg click event 
        MADUnityIntegrator.EventClick += this.OnMADHGClickEvent;
        //reg Hand Detected event 
        MADUnityIntegrator.EventHandDetected += this.OnMADHGHandDetectedEvent;
        //reg Grab Event
        MADUnityIntegrator.EventGrab += this.OnMADHGGrabEvent;
        //reg Hold Event
        // DemoHandGesture.Instance.EventHold += this.OnMADHGHoldEvent;
    }

    void OnDestroy()
    {
        //unreg click event 
        MADUnityIntegrator.EventClick -= this.OnMADHGClickEvent;
        //unreg Hand Detected event 
        MADUnityIntegrator.EventHandDetected -= this.OnMADHGHandDetectedEvent;
        //unreg Grab Event
        MADUnityIntegrator.EventGrab -= this.OnMADHGGrabEvent;
        //unreg Hold Event
        // DemoHandGesture.Instance.EventHold -= this.OnMADHGHoldEvent;
    }


    void Update () {
            
    }
 
    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 50;
        myStyle.normal.textColor = Color.white;

        if(!String.IsNullOrEmpty(eventClickStr))
        GUI.Label(new Rect(10,60,500,500), eventClickStr,myStyle);

        if(!String.IsNullOrEmpty(eventGrabStr))
        GUI.Label(new Rect(10,270,500,500), eventGrabStr,myStyle);
        if(!String.IsNullOrEmpty(eventHoldStr))
        GUI.Label(new Rect(10,440,500,500), eventHoldStr,myStyle);
        if(!String.IsNullOrEmpty(eventDetectedStr))
        GUI.Label(new Rect(10,660,500,500), eventDetectedStr,myStyle);        

    }




    //Click event callback
    void OnMADHGClickEvent(Click click)
    {
        Debug.Log("HandGestureSample: DemoHandGesture: onClick: hand[" +click.index+"] position["+click.x+", "+click.y+"]");
        string logTime = getLogTime();
        eventClickStr = "OnClick: hand[" +click.index+"] position["+click.y+", "+click.y+"] logTime: "+logTime;
    }


    //Hand Detected event callback
    void OnMADHGHandDetectedEvent(HandDetected handDetected)
    {
        if(handDetected!=null){
            Debug.Log("OnHandDetected");
            eventDetectedStr = "";
                Hand hand = handDetected.hand;
                if(hand!=null){
                int count = hand.handDatas.Count;
                eventDetectedStr +="HandDetectedEvent: count = "+count+"\n";
                for(int i =0; i < count; i ++){

                    // Debug.Log("MouseTargetV3D: DemoHandGesture: onHandDetected: isLeftHand = [" +hand.handDatas[i].isLeftHand); 
                    // Debug.Log("MouseTargetV3D: DemoHandGesture: onHandDetected: handType = " +hand.handDatas[i].handType.ToString());   
                    // Debug.Log("MouseTargetV3D: DemoHandGesture: onHandDetected: palmCenterPoint = [" +hand.handDatas[i].palmCenterPoint.X+", "+hand.handDatas[i].palmCenterPoint.Y+"]");    
                    // Debug.Log("MouseTargetV3D: DemoHandGesture: onHandDetected: firstFingerPoint = [" +hand.handDatas[i].firstFingerPoint.X+", "+hand.handDatas[i].firstFingerPoint.Y+"]"); 
                    
                    eventDetectedStr+="Hand["+i+"]\n";
                    eventDetectedStr+="isLeftHand [" +hand.handDatas[i].isLeftHand+"]\n";
                    eventDetectedStr+="isPinch [" +hand.handDatas[i].isPinch+"]\n";
                    eventDetectedStr+="handType [" +hand.handDatas[i].handType.ToString()+"]\n";
                    eventDetectedStr+="palmCenterPoint [" +hand.handDatas[i].palmCenterPoint.X+", "+hand.handDatas[i].palmCenterPoint.Y+"]\n";
                    eventDetectedStr+="firstFingerPoint = [" +hand.handDatas[i].firstFingerPoint.X+", "+hand.handDatas[i].firstFingerPoint.Y+"]\n";

                    if(hand.handDatas[i].isLeftHand){
                        //left Hand    
                    } else{
                        //right Hand
                        handPosition = new Vector2 (
                                hand.handDatas[i].palmCenterPoint.X , 
                                Screen.height - hand.handDatas[i].palmCenterPoint.Y
                        );
                    }
                }
                }
            }
    }


    //Grab event callback
    void OnMADHGGrabEvent(Grab grab)
    {
        string logTime = getLogTime();
        var grabStatus = grab.status;
        switch(grabStatus)
        {
            case Grab.GrabStatus.START: 
                Debug.Log("HandGestureSample: DemoHandGesture: onGrab: START - Hand["+grab.index+"] position["+grab.x+", "+grab.y+"] logTime : "+logTime);
                eventGrabStr ="GrabEvent: onGrab: START - Hand["+grab.index+"] position["+grab.x+", "+grab.y+"] logTime : "+logTime+" \n";
                break;
            case Grab.GrabStatus.HOLDING:
                Debug.Log("HandGestureSample: DemoHandGesture: onGrab: HOLDING - Hand["+grab.index+"] position["+grab.x+", "+grab.y+"]  different: ["+grab.dx+", "+grab.dy+"] logTime : "+logTime);
                eventGrabStr ="GrabEvent: onGrab: HOLDING - Hand["+grab.index+"] position["+grab.x+", "+grab.y+"]  different: ["+grab.dx+", "+grab.dy+"] logTime : "+logTime+" \n";
                break;
            case Grab.GrabStatus.RELEASE:
                Debug.Log("HandGestureSample: DemoHandGesture: onGrab: RELEASE - Hand["+grab.index+"] position["+grab.x+", "+grab.y+"]  different: ["+grab.dx+", "+grab.dy+"] logTime : "+logTime);
                eventGrabStr ="GrabEvent: onGrab: RELEASE - Hand["+grab.index+"] position["+grab.x+", "+grab.y+"]  different: ["+grab.dx+", "+grab.dy+"] logTime : "+logTime+" \n";
                break;
            case Grab.GrabStatus.CANCEL:
                Debug.Log("HandGestureSample: DemoHandGesture: onGrab: CANCEL - Hand["+grab.index+"] position["+grab.x+", "+grab.y+"]  different: ["+grab.dx+", "+grab.dy+"] logTime : "+logTime);
                eventGrabStr ="GrabEvent: onGrab: CANCEL - Hand["+grab.index+"] position["+grab.x+", "+grab.y+"]  different: ["+grab.dx+", "+grab.dy+"] logTime : "+logTime+" \n";
                break;
            default:
                Debug.Log("HandGestureSample: DemoHandGesture: onGrab: OTHER DEFAULT CASE");
                eventGrabStr ="GrabEvent: onGrab: onGrab: OTHER DEFAULT CASE logTime : "+logTime+" \n";
                break;
        }
    }

    public string getLogTime()
    {
        string niceTime = ((float)DateTime.Now.Hour + ":" + ((float)DateTime.Now.Minute) + ":" + ((float)DateTime.Now.Second));
        return niceTime;
    }
}