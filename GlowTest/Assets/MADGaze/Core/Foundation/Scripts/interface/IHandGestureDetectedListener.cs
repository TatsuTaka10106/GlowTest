using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADGazeSDK;
public interface IHandGestureDetectedListener
{
	void onHandDetected(Hand hand, bool isHand);
}
