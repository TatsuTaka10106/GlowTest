using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISplitSensorListener
{
	    void onCalibratedGyroscope(float x, float y, float z,float w);
		void gyroscope(float x, float y, float z, float rotationX,float rotationY,float rotationZ);
       
}
