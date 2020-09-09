using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MADGazeSDK;
using System;

public class HandGestureController : MonoBehaviour, ISplitCameraCallback
{
    
    public GameObject HandGestureContronllerGroup;
    public GameObject handGestureCanvas;
    private Text LabelCamera, LabelService, LabelGesture, LabelShowSkeleton;
    private Text LabelHandSignalController, LabelHandTrackingController;
    private Text LabelHandGrabController, LabelHandCursorController;
    private string cameraStatus;
    private string serviceStatus;
    private string gestureStatus;
    private string showSkeletonStatus;


    private string handSignalControllerString;
    private string handCursorControllerString,handCursorControllerClickString;
    private string handTrackingControllerString;

    private string handGrabControllerString;

    public GameObject clickObject;
    public GameObject grabObject;

    public Material[] cubeMaterials;
    private int clickIndex;
    void Start()
    {    

        fillDefaultValue();

        findLabelByName();
        
        registerCallback();

        SplitCamera.Instance.setCameraCallback(this);  

        clickIndex = 0;
    }

    void fillDefaultValue(){
        cameraStatus = "Camera Status: Disconnected";
        serviceStatus = "Service Status: Disconnected";
        gestureStatus = "Gesture Status: Not Ready";
        showSkeletonStatus = "Show Skeleton: OFF";
   }

    void findLabelByName(){
        Debug.Log("findLabelByName");
        LabelCamera = findLabel("LabelCamera");
        if(LabelCamera==null){
            Debug.Log("LabelCamera = null");
        }else{
            Debug.Log("LabelCamera found!");
        }
        LabelService = findLabel("LabelService");
        LabelGesture = findLabel("LabelGesture");
        LabelShowSkeleton = findLabel("LabelShowSkeleton");


        LabelHandSignalController = findLabel("LabelHandSignalController");
        LabelHandTrackingController = findLabel("LabelHandTrackingController");
        LabelHandGrabController = findLabel("LabelHandGrabController");
        LabelHandCursorController = findLabel("LabelHandCursorController");
    }

    private Text findLabel(string name){
        GameObject obj = FindInChildrenIncludingInactive(HandGestureContronllerGroup,name);
        
        if(obj!=null){
            return obj.GetComponent<Text>();
        }

        return null;
    }
      public static GameObject FindInChildrenIncludingInactive(GameObject go, string name)
    {

        for (int i=0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).gameObject.name == name) return go.transform.GetChild(i).gameObject;
            GameObject found = FindInChildrenIncludingInactive(go.transform.GetChild(i).gameObject, name);
            if (found != null) return found;
        }

        return null;  //couldn't find crap
    }
    void registerCallback(){
         HandGestureManager.Instance.Controller<HandSignalController>().registerCallback(
            (HandSignal.Type gestureType, HandSignal.Direction direction, Vector3 palmCenter, Vector3 secondaryPoint)=>{
                //OnTracked
                // Debug.Log("HandGestureController: HandSignalController OnTracked - gestureType = " + gestureType.ToString() + " direction = " + direction.ToString() + " palmCenter [ " + palmCenter.x + ", " + palmCenter.y + ", " + palmCenter.z + " ]");
                if(gestureType != HandSignal.Type.UNKNOWN){
                    handSignalControllerString="OnTracked:\nGestureType: "+gestureType.ToString()+"\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nSecondaryPoint: ["+secondaryPoint.x+", "+secondaryPoint.y+"]";    
                } else{
                    handSignalControllerString="OnTracked:\nGestureType: \nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nSecondaryPoint: ["+secondaryPoint.x+", "+secondaryPoint.y+"]";    
                }
                
            },
            ()=>{
                //Hand Lost
                // Debug.Log("HandGestureController: HandSignalController Lost");
                handSignalControllerString="Lost";
            }
        );

        
        HandGestureManager.Instance.Controller<HandCursorController>().registerCallback(
            (HandCursor.Direction direction, Vector3 palmCenter)=>{
                //OnTracked
                //  Debug.Log("GameScript: HandCursorController OnTracked - direction = "+direction.ToString()+" palmCenter [ " + palmCenter.x + ", " + palmCenter.y + ", " + palmCenter.z + " ]");
                 handCursorControllerString="OnTracked:\nDirection:"+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]";
                 
            }, 
            (HandCursor.Direction direction, Vector3 palmCenter, Vector3 lastPalmCenter)=>{
                //OnMoved
                Debug.Log("GameScript: HandCursorController OnMoved - direction = "+direction.ToString()+" palmCenter [ " + palmCenter.x + ", " + palmCenter.y + ", " + palmCenter.z + " ]");
                handCursorControllerString="OnMoved:\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nLastPalmCenter: ["+lastPalmCenter.x+", "+lastPalmCenter.y+"]";
                // updateGrabObject(palmCenter);
            },
            (HandCursor.Direction direction, Vector3 palmCenter, Vector3 intersectionPoint)=>{
                //Clicked
                // Debug.Log("GameScript: HandCursorController Clicked - direction = "+direction.ToString()+" palmCenter [ " + palmCenter.x + ", " + palmCenter.y + ", " + palmCenter.z + " ] intersectionPoint [ " + intersectionPoint.x + ", " + intersectionPoint.y + ", " + intersectionPoint.z + " ]");
                //handCursorControllerClickString="OnClicked:\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nIntersectionPoint: ["+intersectionPoint.x+", "+intersectionPoint.y+"] LastUpdate : "+getLogTime()+"";
                handCursorControllerClickString="OnClicked:\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nIntersectionPoint: ["+intersectionPoint.x+", "+intersectionPoint.y+"]";
                
                updateClickObject(intersectionPoint);
            },
            ()=>{
                //Hand Lost
                // Debug.Log("GameScript: HandCursorController ");
                handCursorControllerString="Lost";
                handCursorControllerClickString = "";
            }
        );
        HandGestureManager.Instance.Controller<HandTrackingController>().registerCallback((TrackedHand hand1, TrackedHand hand2)=>{
             handTrackingControllerString = "";
             if(hand1!=null){
                Vector3 thumberFinger = hand1.getPosition(TrackedHand.FeaturePointsType.THUMB_FINGER_TIP);
                // Debug.Log("GameScript: HandTrackingController hand1 direction = "+hand1.direction+" thumberFinger [ "+thumberFinger.x+", "+thumberFinger.y ]" ); 
                handTrackingControllerString += "Hand1:\nDirection: "+hand1.direction.ToString() +"\nThumberFinger: [ "+thumberFinger.x+", "+thumberFinger.y+" ]";
           } else{
                handTrackingControllerString += "";
                //Debug.Log("GameScript: HandTrackingController hand1 null" ); 
           }

           if(hand2!=null){
                Vector3 thumberFinger = hand2.getPosition(TrackedHand.FeaturePointsType.THUMB_FINGER_TIP);
                //Debug.Log("GameScript: HandTrackingController hand2 direction = "+hand2.direction+" thumberFinger [ "+thumberFinger.x+", "+thumberFinger.y+", "+thumberFinger.z+" ]" ); 
                handTrackingControllerString += "Hand2:\nDirection: "+hand2.direction.ToString() +"\nThumberFinger: [ "+thumberFinger.x+", "+thumberFinger.y+"]";
                

                
           }else{
                // Debug.Log("GameScript: HandTrackingController hand2 null" ); 
                handTrackingControllerString +="";
           }

           if(string.IsNullOrEmpty(handTrackingControllerString)){
               handTrackingControllerString = "Lost";
           }
        });

        
        HandGestureManager.Instance.Controller<HandGrabController>().registerCallback(
            (HandGrab.Direction direction, Vector3 palmCenter)=>{
                //onStarted
                // Debug.Log("HandGestureController: HandGrabController  onStarted: direction = "+direction+" palmCenter [ "+palmCenter.x+", "+palmCenter.y+", "+palmCenter.z+" ]" ); 
                handGrabControllerString="Onstarted:\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]";
            }, 
            (HandGrab.Direction direction, Vector3 palmCenter, Vector3 lastPalmCenter)=>{
                //onMoved
                //Debug.Log("HandGestureController: HandGrabController onMoved:  direction = "+direction+" palmCenter [ "+palmCenter.x+", "+palmCenter.y+", "+palmCenter.z+" ]" ); 
                handGrabControllerString="OnMoved:\nDirection: "+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]\nLastPalmCenter: ["+lastPalmCenter.x+", "+lastPalmCenter.y+"]";
                updateGrabObject(palmCenter);
            }, 
            (HandGrab.Direction direction, Vector3 palmCenter)=>{
                //onEnded
                // Debug.Log("HandGestureController: HandGrabController onEnded:  direction = "+direction+" palmCenter [ "+palmCenter.x+", "+palmCenter.y+", "+palmCenter.z+" ]" ); 
                handGrabControllerString="onEnded:\nDirection:"+direction.ToString() +"\nPalmCenter: ["+palmCenter.x+", "+palmCenter.y+"]";
            },
            ()=>{
                //onCancelled
                handGrabControllerString="onCancelled";
            }
        );
    }

    public void onConnected(){
        cameraStatus="Camera Status: Connected";
    }

    public void onDisconnected(){
        cameraStatus = "Camera Status: Disconnected";
    }

    public void onError(int code){
        if(code == -1){
            cameraStatus ="Camera Status: Please connect your MAD Gaze Device";
        } else {
            cameraStatus = string.Format("Error: Code={0}", code);
        }
        
    }


    // Update is called once per frame
    void Update()
    {        
        if(LabelCamera!=null){

            gestureStatus = "Gesture Status: "+(MADHandGesture.Instance.isHandGestureRunning() ? "Ready":"Not Ready");
            serviceStatus = "Service Status: "+(MADHandGesture.Instance.isServiceConnected() ? "Connected":"Disconnected");
            showSkeletonStatus = "Show Skeleton: "+(HandGestureManager.Instance.SHOW_SKELETON ? "ON":"OFF");

            LabelCamera.text = cameraStatus;
            LabelService.text = serviceStatus;
            LabelGesture.text = gestureStatus+"";
            LabelShowSkeleton.text = showSkeletonStatus;
        }

    if (!string.IsNullOrEmpty(handSignalControllerString)){
        LabelHandSignalController.text =  "\nHandSignalController:\n"+handSignalControllerString;
    }else{
        LabelHandSignalController.text = "";
    }
    
     if (!string.IsNullOrEmpty(handTrackingControllerString)){
        LabelHandTrackingController.text = "\nHandTrackingController:\n"+handTrackingControllerString;
    }else{
        LabelHandTrackingController.text="";
    }

    if (!string.IsNullOrEmpty(handGrabControllerString)){
        LabelHandGrabController.text = "\nHandGrabControllerString:\n"+handGrabControllerString;
    }else{
        LabelHandGrabController.text = "";
    }

    if (!string.IsNullOrEmpty(handCursorControllerString)){
        LabelHandCursorController.text = "\nHandCursorController:\n"+handCursorControllerString+"\n\n"+handCursorControllerClickString;
    }else{
        LabelHandCursorController.text = "";
    }


// if (Input.GetMouseButtonDown(0))
//         {
//             Vector3 point = new Vector3(Input.mousePosition.x,Input.mousePosition.y,4f);
//             point = Camera.main.ScreenToWorldPoint(point);
//             GameObject go = Instantiate(clickObject);
//             go.transform.position = point;
//             Destroy(go, 0.5f);
//         }

    }

    // public string getLogTime()
    // {
    //     string niceTime = ((float)DateTime.Now.Hour + ":" + ((float)DateTime.Now.Minute) + ":" + ((float)DateTime.Now.Second));
    //     return niceTime;
    // }
    

    
    private void updateClickObject(Vector3 intersectionPoint){        
                    if(clickObject!=null && intersectionPoint!=null){
                        Vector3 point = new Vector3(intersectionPoint.x,intersectionPoint.y,6f);
                        point = Camera.main.ScreenToWorldPoint(point);
                        GameObject go = Instantiate(clickObject);
                        
                        go.transform.position = point;
                        if(cubeMaterials!=null && cubeMaterials.Length > 0 ){
                            if(clickIndex >= cubeMaterials.Length){
                                clickIndex = 0;
                            }
                            go.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = cubeMaterials[clickIndex];
                            clickIndex++;
                        }
                        Destroy(go, 20.0f);
                }
    }

    private void updateGrabObject(Vector3 palmCenter){        
                    if(grabObject!=null && palmCenter!=null){
                        Vector3 point = new Vector3(palmCenter.x,palmCenter.y, 6f);
                        point = Camera.main.ScreenToWorldPoint(point);
                        grabObject.transform.position = point;
                }
    }

    public void toggleMenu(){
		if(handGestureCanvas!=null){
			handGestureCanvas.SetActive(!handGestureCanvas.activeSelf);
		}
	}


}
