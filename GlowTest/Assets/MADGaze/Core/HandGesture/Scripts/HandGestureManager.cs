using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace MADGazeSDK {
   public class HandGestureManager {
      private static HandGestureManager instance = null;  

      HandSignalController m_HandSignalController;
      HandCursorController m_HandCursorController;
      HandGrabController m_HandGrabController;

      HandTrackingController m_HandTrackingController;

      public bool DEBUG_MODE = false;
      public bool SHOW_SKELETON = false;
      public static HandGestureManager Instance
      {
         get
         { 
            if (instance == null){ 
               instance = new HandGestureManager();
               // instance.init();
            }
            return instance; 
         }
      }
      

      // public void init(){
      //    // Debug.Log("HandGestureManager:init");
      //    //start preview
      //    // if(SplitCamera.Instance.isDeviceConnected()){
      //    //    Debug.Log("HandGestureManager:startPreview");
      //    //    SplitCamera.Instance.startPreview();
      //    // }
      // }
      
      HandGestureManager(){
         m_HandSignalController = new HandSignalController();
         m_HandCursorController = new HandCursorController();
         m_HandGrabController = new HandGrabController();
         m_HandTrackingController = new HandTrackingController();
         
      }

      public T Controller<T>() where T : HandGestureController
      {  
         Type itemType = typeof(T);
         if (itemType == typeof(HandSignalController))
            return (T) (object) m_HandSignalController;
         else if (itemType == typeof(HandCursorController))
            return (T) (object) m_HandCursorController;
         else if (itemType == typeof(HandGrabController))
            return (T) (object) m_HandGrabController;
         else if (itemType == typeof(HandTrackingController))
            return (T) (object) m_HandTrackingController;

         return null;
      } 

      public void sendMessage<T> (params object[] args) where T : HandGestureController
      {
         Type itemType = typeof(T);
         if (itemType == typeof(HandSignalController)){
            m_HandSignalController.handleMessage(args);
            return;
         }
         else if (itemType == typeof(HandCursorController)){
            m_HandCursorController.handleMessage(args);
            return;
         }
         else if (itemType == typeof(HandGrabController)){
            m_HandGrabController.handleMessage(args);
            return;
         }
         else if (itemType == typeof(HandTrackingController)){
            m_HandTrackingController.handleMessage(args);
            return;
         }
      }
      public void SetEnabled<T>(bool enabled) where T : HandGestureController
      {  
         Type itemType = typeof(T);
         if (itemType == typeof(HandSignalController))
            m_HandSignalController.Enabled = enabled;
         else if (itemType == typeof(HandCursorController))
            m_HandCursorController.Enabled = enabled;
         else if (itemType == typeof(HandGrabController))
            m_HandGrabController.Enabled = enabled;
         else if (itemType == typeof(HandTrackingController))
            m_HandTrackingController.Enabled = enabled;
      }  
      public bool isEnabled<T>() where T : HandGestureController
      {  
         Type itemType = typeof(T);
         if (itemType == typeof(HandSignalController))
            return m_HandSignalController.Enabled;
         else if (itemType == typeof(HandCursorController))
            return m_HandCursorController.Enabled;
         else if (itemType == typeof(HandGrabController))
            return m_HandGrabController.Enabled;
         else if (itemType == typeof(HandTrackingController))
            return m_HandTrackingController.Enabled;
         return false;
      }

      public void Destroy(){
         if (m_HandSignalController != null) {
            m_HandSignalController.onDestroy();
            m_HandSignalController = null;
         }
         if (m_HandCursorController != null) {
            m_HandCursorController.onDestroy();
            m_HandCursorController = null;
         }
         if (m_HandGrabController != null) {
            m_HandGrabController.onDestroy();
            m_HandGrabController = null;
         }         
         if (m_HandTrackingController != null) {
            m_HandTrackingController.onDestroy();
            m_HandTrackingController = null;
         }         
         if (instance != null)
            instance = null;
      }
   }
}