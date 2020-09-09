using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MADGazeSDK{
    
public class MADSDKIntegratorGrab : BaseMADSDKIntegrator
{

    Vector3 lastPosition;
    public MADSDKIntegratorGrab(){
        lastPosition = new Vector3(0,0,0);
    }

   public override void OnStart()
    {
          //reg Grab Event
        MADUnityIntegrator.EventGrab += this.OnMADHGGrabEvent;  
    }

    public override void OnDestroy()
    {
      //unreg Grab Event
        MADUnityIntegrator.EventGrab -= this.OnMADHGGrabEvent;    
    }

 //Grab event callback
    void OnMADHGGrabEvent(Grab grab)
    {
        if(HandGestureManager.Instance.isEnabled<HandGrabController>()){
        
        var grabStatus = grab.status;

        switch(grabStatus)
        {
            case Grab.GrabStatus.START: 
            
                HandGestureManager.Instance.sendMessage<HandGrabController>(
                HandGrab.Action.STARTED,
                grab.index == 0 ? HandCursor.Direction.LEFT : HandCursor.Direction.RIGHT, 
                new Vector3(grab.x,Screen.height - grab.y, 0)
                );
                
                lastPosition.Set(grab.x,Screen.height - grab.y, 0); 
                
                break;
            case Grab.GrabStatus.HOLDING:
               
                HandGestureManager.Instance.sendMessage<HandGrabController>(
                HandGrab.Action.MOVED,
                grab.index == 0 ? HandCursor.Direction.LEFT : HandCursor.Direction.RIGHT, 
                new Vector3(grab.x, Screen.height - grab.y, 0),
                lastPosition
                );
                
                lastPosition.Set(grab.x,Screen.height - grab.y, 0); 

                break;
            case Grab.GrabStatus.RELEASE:

                HandGestureManager.Instance.sendMessage<HandGrabController>(
                HandGrab.Action.ENDED,
                grab.index == 0 ? HandCursor.Direction.LEFT : HandCursor.Direction.RIGHT, 
                new Vector3(grab.x, Screen.height - grab.y, 0)
                );
                
                lastPosition.Set(grab.x,Screen.height - grab.y, 0); 
            break;
            case Grab.GrabStatus.CANCEL:
                
                HandGestureManager.Instance.sendMessage<HandGrabController>(
                HandGrab.Action.CANCELLED);
                break;
            default:
            
                Debug.Log("HandGestureSample: DemoHandGesture: onGrab: OTHER DEFAULT CASE");
                break;
        }
    }
    }


}
}
