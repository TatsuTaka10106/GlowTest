using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADGazeSDK;
public class MADGazeARCamera : MonoBehaviour
{

    private Camera cam;
    
    public GameObject targetObject;

    void Start()
    {
        cam = GetComponent<Camera>();

        MADGazeARManager.Instance.setCameraPositionCallback((Vector3 position, Quaternion rotation)=>{
                //cam.projectionMatrix = MADSlamManager.Instance.getProjectMatrix();
                transform.position = position;
                transform.rotation = rotation;
        });

        MADGazeARManager.Instance.setModelPositionCallback((Vector3 position, Quaternion rotation,Vector3 localScale)=>{
                if(targetObject!=null){
                    targetObject.transform.position = position;
                    targetObject.transform.rotation = rotation;
                    targetObject.transform.localScale = localScale;
                }
                
        });

        MADGazeARManager.Instance.setModelTrackedCallback((bool tracked)=>{
            if(targetObject!=null){
                targetObject.SetActive(tracked);
            }
        });

        targetObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MADGazeARManager.Instance.onUpdateObject(cam, targetObject.transform);
    }
}
