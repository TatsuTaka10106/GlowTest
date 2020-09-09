using UnityEngine.Events;
using UnityEngine;
using MADGazeSDK;

[System.Serializable]
public class HandGrabStartedEvent : UnityEvent<HandGrab.Direction, Vector3>
{
}