using UnityEngine.Events;
using UnityEngine;
using MADGazeSDK;

[System.Serializable]
public class HandCursorTrackedEvent : UnityEvent<HandCursor.Direction, Vector3>
{
}
