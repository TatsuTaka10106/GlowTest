using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MADGazeMenuItem : EditorWindow
{  
    [MenuItem("MAD Gaze/Change API Key")]
    private static void OpenTheWindow()
    {
        Selection.SetActiveObjectWithContext(MADGazeSDKController.Settings, null);
    }
}