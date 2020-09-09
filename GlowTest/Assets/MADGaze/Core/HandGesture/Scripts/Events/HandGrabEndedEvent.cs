using UnityEngine.Events;
using UnityEngine;
using MADGazeSDK;

[System.Serializable]
public class HandGrabEndedEvent : UnityEvent<HandGrab.Direction, Vector3>
{
}