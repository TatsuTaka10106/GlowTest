using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using MADGazeSDK;
[CustomEditor(typeof(MADGazeHandGesture))]
public class HandGestureEditor : Editor
{
    SerializedObject _target;
    MADGazeHandGesture PrefabTarget {
        get {return (MADGazeHandGesture)target;}
    }
    
    void OnEnable(){
        _target = new SerializedObject((MADGazeHandGesture)target);
        
        
    }
    public override void OnInspectorGUI() {
        #if UNITY_2019_1_OR_NEWER
       
        PrefabTarget.enableDebugMode = EditorGUILayout.Toggle ("Enable Debug Mode", PrefabTarget.enableDebugMode);
        PrefabTarget.showSkeleton = EditorGUILayout.Toggle ("Show Skeleton", PrefabTarget.showSkeleton);

     

        buildHandSignalControlGUI();
        EditorGUIUtils.GUILine();
        buildMovingCursorControlGUI();
        EditorGUIUtils.GUILine();
        buildGrabControlGUI();
        EditorGUIUtils.GUILine();
        buildRawTrackingControlGUI();

        #else

        EditorGUILayout.HelpBox("Hand Gesutre Prefab is only accessible on Unity 2019 or newer version. However you can still access Hand Gesture features by code.\n\nCheck https://sdk.madgaze.com/ for more information.", MessageType.Error);

        #endif
    }

    void buildRawTrackingControlGUI(){
        if (PrefabTarget.enableRawTrackingControl){
            if (GUILayout.Button("Hand Tracking Control (RAW): ON")){
            PrefabTarget.enableRawTrackingControl = false;
            }
        } else {
            if (GUILayout.Button("Hand Tracking Control (RAW): OFF")){
            PrefabTarget.enableRawTrackingControl = true;
            }
        }
        if (PrefabTarget.enableRawTrackingControl) {
            PrefabTarget.enableRawTrackingOnStartup = EditorGUILayout.Toggle ("Enable on Startup", PrefabTarget.enableRawTrackingOnStartup);
            EditorGUIUtils.BuildCustomControls(_target, "handTrackingCallback");   
        }
    }
    void buildHandSignalControlGUI(){
        if (PrefabTarget.enableSignalControl){
            if (GUILayout.Button("Hand Signal Control: ON")){
            PrefabTarget.enableSignalControl = false;
            }
        } else {
            if (GUILayout.Button("Hand Signal Control: OFF")){
            PrefabTarget.enableSignalControl = true;
            }
        }
        if (PrefabTarget.enableSignalControl) {
            PrefabTarget.enableSignalOnStartup = EditorGUILayout.Toggle ("Enable on Startup", PrefabTarget.enableSignalOnStartup);
            EditorGUIUtils.BuildCustomControls(_target, "handSignalCallback");   
        }
    }

    void buildMovingCursorControlGUI(){
        if (PrefabTarget.enableCursorControl){
             if (GUILayout.Button("Hand Cursor Control: ON")){
                PrefabTarget.enableCursorControl = false;
             }
        } else {
             if (GUILayout.Button("Hand Cursor Control: OFF")){
                PrefabTarget.enableCursorControl = true;
             }
        }
        if (PrefabTarget.enableCursorControl) {
            PrefabTarget.enableCursorOnStartup = EditorGUILayout.Toggle ("Enable on Startup", PrefabTarget.enableCursorOnStartup);
            EditorGUIUtils.BuildCustomControls(_target, "handCursorCallback");   
        }
    }

    void buildGrabControlGUI(){
        if (PrefabTarget.enableGrabControl){
             if (GUILayout.Button("Hand Grab Control: ON")){
                PrefabTarget.enableGrabControl = false;
             }
        } else {
             if (GUILayout.Button("Hand Grab Control: OFF")){
                PrefabTarget.enableGrabControl = true;
             }
        }
        if (PrefabTarget.enableGrabControl) {
            PrefabTarget.enableGrabOnStartup = EditorGUILayout.Toggle ("Enable on Startup", PrefabTarget.enableGrabOnStartup);
            EditorGUIUtils.BuildCustomControls(_target, "handGrabCallback");   
        }
    }

      
    class EditorGUIUtils{
        public static void GUILine( int i_height = 1 )
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height );
            rect.height = i_height;
            EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
        }

        public static void BuildCustomControls(SerializedObject target, String propertyName){
            SerializedProperty sprop = target.FindProperty(propertyName);
            // EditorGUIUtility.LookLikeControls();
            EditorGUILayout.PropertyField(sprop);
            target.ApplyModifiedProperties();
        }
    }
   


}
