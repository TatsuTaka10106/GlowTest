using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using MADGazeSDK;

[System.Serializable]
public class HandSignalTrackedEvent : UnityEvent<HandSignal.Type, HandSignal.Direction, Vector3, Vector3>
{
}