using UnityEngine.Events;
using UnityEngine;
using MADGazeSDK;

[System.Serializable]
public class HandCursorMovedEvent : UnityEvent<HandCursor.Direction, Vector3, Vector3>
{
}
