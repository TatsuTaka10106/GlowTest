using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISplitSensorEventListener 
{
	void onSplitSensorChanged(float[] value, SplitUSBSerial.MadSensorType madSensorType);
    void onSplitAccuracyChanged(int i);
}
