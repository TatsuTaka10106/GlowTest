using UnityEngine.Events;
using UnityEngine;
using MADGazeSDK;

[System.Serializable]
public class HandGrabMovedEvent : UnityEvent<HandGrab.Direction, Vector3, Vector3>
{
}