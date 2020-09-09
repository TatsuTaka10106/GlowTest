using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MADGazeSDK;


[CustomEditor(typeof(MADGazeSDKSettings), true)]
    public class MADGazeSDKSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.HelpBox("With API Key, you can retrieve the Current User Id and Purchase Status of your application to protect your intellectual property from unauthorised uses.", MessageType.Info);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("APIKey"), new GUIContent("API Key"), true);
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.HelpBox("To obtain the API key, you can visit at http://console.madgaze.com/, or study the usage at http://sdk.madgaze.com/", MessageType.Warning);
        }
    }