using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADGazeSDK;
public class SlamController : MonoBehaviour
{
   
	private bool isShowGrid = false;
	private bool isShowPreview;

	public MADGazeCamera MADGazeCamera;
	public void Start () {
		//registerCallback();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

 	public void switchPreview(){
		if(MADGazeCamera!=null) {
        	MADGazeCamera.showCameraPreview = !MADGazeCamera.showCameraPreview;
		}
 	}

    public void switchCamera(){
			
			if(SplitCamera.Instance.isDeviceConnected()){
					SplitCamera.Instance.stopPreview();
				} else{
					SplitCamera.Instance.startPreview();
				}
 	}

     public void resetModelPosition(){
		MADGazeARManager.Instance.resetTargetPosition((isSuccess)=>{
                if(isSuccess){
                    Debug.Log("Place : true");
                } else{
                    Debug.Log("Place : false");
                }
        });
 	}


  void registerCallback(){
         HandGestureManager.Instance.Controller<HandSignalController>().registerCallback(
            (HandSignal.Type gestureType, HandSignal.Direction direction, Vector3 palmCenter, Vector3 secondaryPoint)=>{
                //OnTracked
                Debug.Log("HandGestureController: HandSignalController OnTracked - gestureType = " + gestureType.ToString() + " direction = " + direction.ToString() + " palmCenter [ " + palmCenter.x + ", " + palmCenter.y + ", " + palmCenter.z + " ]");
                if(gestureType != HandSignal.Type.UNKNOWN){
                    // handSignalControllerString="OnTracked:\nGestureType: "+gestureType.ToString()+"\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nSecondaryPoint: ["+secondaryPoint.x+", "+secondaryPoint.y+"]";    
                } else{
                    // handSignalControllerString="OnTracked:\nGestureType: \nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nSecondaryPoint: ["+secondaryPoint.x+", "+secondaryPoint.y+"]";    
                }
                
            },
            ()=>{
                //Hand Lost
                // Debug.Log("HandGestureController: HandSignalController Lost");
                // handSignalControllerString="Lost";
            }
        );

        
        HandGestureManager.Instance.Controller<HandCursorController>().registerCallback(
            (HandCursor.Direction direction, Vector3 palmCenter)=>{
                //OnTracked
                 Debug.Log("GameScript: HandCursorController OnTracked - direction = "+direction.ToString()+" palmCenter [ " + palmCenter.x + ", " + palmCenter.y + ", " + palmCenter.z + " ]");
                //  handCursorControllerString="OnTracked:\nDirection:"+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]";
                 
            }, 
            (HandCursor.Direction direction, Vector3 palmCenter, Vector3 lastPalmCenter)=>{
                //OnMoved
                Debug.Log("GameScript: HandCursorController OnMoved - direction = "+direction.ToString()+" palmCenter [ " + palmCenter.x + ", " + palmCenter.y + ", " + palmCenter.z + " ]");
                // handCursorControllerString="OnMoved:\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nLastPalmCenter: ["+lastPalmCenter.x+", "+lastPalmCenter.y+"]";
                // updateGrabObject(palmCenter);
            },
            (HandCursor.Direction direction, Vector3 palmCenter, Vector3 intersectionPoint)=>{
                //Clicked
                Debug.Log("GameScript: HandCursorController Clicked - direction = "+direction.ToString()+" palmCenter [ " + palmCenter.x + ", " + palmCenter.y + ", " + palmCenter.z + " ] intersectionPoint [ " + intersectionPoint.x + ", " + intersectionPoint.y + ", " + intersectionPoint.z + " ]");
                //handCursorControllerClickString="OnClicked:\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nIntersectionPoint: ["+intersectionPoint.x+", "+intersectionPoint.y+"] LastUpdate : "+getLogTime()+"";
                // handCursorControllerClickString="OnClicked:\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nIntersectionPoint: ["+intersectionPoint.x+", "+intersectionPoint.y+"]";
                
                // updateClickObject(intersectionPoint);
            },
            ()=>{
                //Hand Lost
                Debug.Log("GameScript: HandCursorController ");
                // handCursorControllerString="Lost";
                // handCursorControllerClickString = "";
            }
        );
        HandGestureManager.Instance.Controller<HandTrackingController>().registerCallback((TrackedHand hand1, TrackedHand hand2)=>{
            //  handTrackingControllerString = "";
             if(hand1!=null){
                Vector3 thumberFinger = hand1.getPosition(TrackedHand.FeaturePointsType.THUMB_FINGER_TIP);
                Debug.Log("GameScript: HandTrackingController hand1 direction = "+hand1.direction+" thumberFinger [ "+thumberFinger.x+", "+thumberFinger.y +"]"); 
                // handTrackingControllerString += "Hand1:\nDirection: "+hand1.direction.ToString() +"\nThumberFinger: [ "+thumberFinger.x+", "+thumberFinger.y+" ]";
           } else{
                // handTrackingControllerString += "";
                Debug.Log("GameScript: HandTrackingController hand1 null" ); 
           }

           if(hand2!=null){
                Vector3 thumberFinger = hand2.getPosition(TrackedHand.FeaturePointsType.THUMB_FINGER_TIP);
                Debug.Log("GameScript: HandTrackingController hand2 direction = "+hand2.direction+" thumberFinger [ "+thumberFinger.x+", "+thumberFinger.y+", "+thumberFinger.z+" ]" ); 
                // handTrackingControllerString += "Hand2:\nDirection: "+hand2.direction.ToString() +"\nThumberFinger: [ "+thumberFinger.x+", "+thumberFinger.y+"]";
                

                
           }else{
                Debug.Log("GameScript: HandTrackingController hand2 null" ); 
                // handTrackingControllerString +="";
           }

        //    if(string.IsNullOrEmpty(handTrackingControllerString)){
            //    handTrackingControllerString = "Lost";
        //    }
        });

        
        HandGestureManager.Instance.Controller<HandGrabController>().registerCallback(
            (HandGrab.Direction direction, Vector3 palmCenter)=>{
                //onStarted
                Debug.Log("HandGestureController: HandGrabController  onStarted: direction = "+direction+" palmCenter [ "+palmCenter.x+", "+palmCenter.y+", "+palmCenter.z+" ]" ); 
                // handGrabControllerString="Onstarted:\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]";
            }, 
            (HandGrab.Direction direction, Vector3 palmCenter, Vector3 lastPalmCenter)=>{
                //onMoved
                Debug.Log("HandGestureController: HandGrabController onMoved:  direction = "+direction+" palmCenter [ "+palmCenter.x+", "+palmCenter.y+", "+palmCenter.z+" ]" ); 
                // handGrabControllerString="OnMoved:\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nLastPalmCenter: ["+lastPalmCenter.x+", "+lastPalmCenter.y+"]";
                // updateGrabObject(palmCenter);
            }, 
            (HandGrab.Direction direction, Vector3 palmCenter)=>{
                //onEnded
                Debug.Log("HandGestureController: HandGrabController onEnded:  direction = "+direction+" palmCenter [ "+palmCenter.x+", "+palmCenter.y+", "+palmCenter.z+" ]" ); 
                // handGrabControllerString="onEnded:\nDirection:"+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]";
            },
            ()=>{
                //onCancelled
                // handGrabControllerString="onCancelled";
            }
        );
    }

     
}
