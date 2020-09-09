using UnityEngine.Events;
using UnityEngine;
using MADGazeSDK;

[System.Serializable]
public class HandCursorClickedEvent : UnityEvent<HandCursor.Direction, Vector3, Vector3>
{
}
