using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;

using MADGazeSDK;

namespace MADGazeSDK {
   public class HandSignalController : HandGestureController {
      List<HandSignalTrackedEvent> _handGestureSignalTrackedEvent;
      List<HandSignalLostEvent> _handGestureSignalLostEvent;

      public HandSignalController(){
         _handGestureSignalTrackedEvent = new List<HandSignalTrackedEvent>();
         _handGestureSignalLostEvent = new List<HandSignalLostEvent>();
         _handGestureSignalTrackedEvent.Add(new HandSignalTrackedEvent());
         _handGestureSignalLostEvent.Add(new HandSignalLostEvent());
      }
      
      protected override void notifyCore(){
         Log("notifyCore");
         //NotifyCore of Enable Issue
           MADUnityIntegrator.Instance.regDetectedListener(this.Enabled);
      }

      public void registerCallback(UnityAction<HandSignal.Type, HandSignal.Direction, Vector3, Vector3> onTracked, UnityAction onTrackedLoss){
         Log("registerCallback");
         if (onTracked != null)
            _handGestureSignalTrackedEvent[0].AddListener(onTracked);
         if (onTrackedLoss != null)
            _handGestureSignalLostEvent[0].AddListener(onTrackedLoss);
      }

      public void unregisterCallback(UnityAction<HandSignal.Type, HandSignal.Direction, Vector3, Vector3> onTracked, UnityAction onTrackedLoss){
         Log("unregisterCallback");
         if (onTracked != null)
            _handGestureSignalTrackedEvent[0].RemoveListener(onTracked);
         if (onTrackedLoss != null)
            _handGestureSignalLostEvent[0].RemoveListener(onTrackedLoss);
      }
      
      public override void unregisterAllCallbacks(){
         Log("unregisterAllCallbacks");
         if (_handGestureSignalTrackedEvent.Count > 1)
            _handGestureSignalTrackedEvent.RemoveRange(1, _handGestureSignalTrackedEvent.Count - 1);
         if (_handGestureSignalLostEvent.Count > 1)
            _handGestureSignalLostEvent.RemoveRange(1, _handGestureSignalLostEvent.Count - 1);
      }

      public override void unregisterAllInspectorCallbacks(){
         _handGestureSignalTrackedEvent[0].RemoveAllListeners();
         _handGestureSignalLostEvent[0].RemoveAllListeners();
      }

      public void registerCallbackFromInspector(HandSignalCallback callback, bool enableOnStartup){
         Log("registerCallbackFromInspector");
         if (enableOnStartup)
            this.Enabled = true;

         if (callback.OnTracked != null)
            _handGestureSignalTrackedEvent.Add(callback.OnTracked);
         if (callback.OnTrackedLost != null)
            _handGestureSignalLostEvent.Add(callback.OnTrackedLost);
      }  

      public void unregisterCallbackFromInspector(HandSignalCallback callback){
         Log("unregisterCallbackFromInspector");
         if (callback.OnTracked != null)
            _handGestureSignalTrackedEvent.Remove(callback.OnTracked);
         if (callback.OnTrackedLost != null)
            _handGestureSignalLostEvent.Remove(callback.OnTrackedLost);
      }

      public override void handleMessage(params object[] args){
         if (args.Length == 0) return;
         HandSignal.Action handSignalAction = (HandSignal.Action) args[0];
         if (handSignalAction == HandSignal.Action.TRACKED && args.Length == 5) {
            notifyTracked((HandSignal.Type) args[1], (HandSignal.Direction) args[2], (Vector3) args[3], (Vector3) args[4]);
            return;
         }
         else if (handSignalAction == HandSignal.Action.LOST && args.Length == 1) {
            notifyTrackedLost();
            return;
         } else
         {
            LogError("handleMessage: Invalid Arguments");
         }
      }

      void notifyTracked(HandSignal.Type a, HandSignal.Direction b, Vector3 c, Vector3 d){
         Log("notifyTracked");
         foreach (HandSignalTrackedEvent trackedEvent in _handGestureSignalTrackedEvent) {
            trackedEvent.Invoke(a, b, c, d);
         }
      }

      void notifyTrackedLost(){
         Log("notifyTrackedLost");
         foreach (HandSignalLostEvent trackedEvent in _handGestureSignalLostEvent) {
            trackedEvent.Invoke();
         }
      }

      public override void onDestroy(){
         Log("onDestroy");
         this.Enabled = false;
         _handGestureSignalTrackedEvent?.Clear();
         _handGestureSignalLostEvent?.Clear();
         _handGestureSignalTrackedEvent = null;
         _handGestureSignalLostEvent = null;
      }
   }
}