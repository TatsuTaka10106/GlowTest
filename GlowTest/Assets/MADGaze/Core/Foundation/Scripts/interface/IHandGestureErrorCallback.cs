using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandGestureErrorCallback
{
	void onErrorCallback(int errorCode, string errorMessage);
}
