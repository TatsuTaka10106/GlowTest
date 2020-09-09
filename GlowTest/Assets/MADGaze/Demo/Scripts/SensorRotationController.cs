using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorRotationController : MonoBehaviour
{
    private Quaternion initialRotation = Quaternion.identity;
    private Quaternion mLastSensorRotation;
    void Update()
    {
        mLastSensorRotation = SplitUSBSensor.Instance.getCameraRotation();

        if(mLastSensorRotation != null){
            this.transform.localRotation = initialRotation * mLastSensorRotation;
        }
    }

    public void resetInitialRotation(){
        Quaternion rotation = new Quaternion(0, mLastSensorRotation.y, 0, mLastSensorRotation.w);
        initialRotation = Quaternion.Inverse(rotation);
    }

}
