using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MADHandGestureManager : MonoBehaviour
{
    public bool autoStartCamera = true;
    
    void Start()
    {
        if(autoStartCamera){
            if(SplitCamera.Instance.isDeviceConnected()){
                SplitCamera.Instance.startPreview();
            }
        }
    }
}
