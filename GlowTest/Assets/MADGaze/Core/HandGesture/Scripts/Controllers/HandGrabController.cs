using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

namespace MADGazeSDK {
   public class HandGrabController : HandGestureController {
      List<HandGrabStartedEvent> _handGestureGrabStartedEvent;
      List<HandGrabMovedEvent> _handGestureGrabMovedEvent;
      List<HandGrabEndedEvent> _handGestureGrabEndedEvent;
      List<HandGrabCancelledEvent> _handGestureGrabCancelledEvent;

      public HandGrabController(){
         _handGestureGrabStartedEvent = new List<HandGrabStartedEvent>();
         _handGestureGrabMovedEvent = new List<HandGrabMovedEvent>();
         _handGestureGrabEndedEvent = new List<HandGrabEndedEvent>();
         _handGestureGrabCancelledEvent = new List<HandGrabCancelledEvent>();
         _handGestureGrabStartedEvent.Add(new HandGrabStartedEvent());
         _handGestureGrabMovedEvent.Add(new HandGrabMovedEvent());
         _handGestureGrabEndedEvent.Add(new HandGrabEndedEvent());
         _handGestureGrabCancelledEvent.Add(new HandGrabCancelledEvent());
      }

      protected override void notifyCore(){
         Log("notifyCore");
         //NotifyCore of Enable Issue
         MADUnityIntegrator.Instance.regGrabListener(this.Enabled);

      }
      public void registerCallback(UnityAction<HandGrab.Direction, Vector3> onStarted, UnityAction<HandGrab.Direction, Vector3, Vector3> onMoved, UnityAction<HandGrab.Direction, Vector3> onEnded, UnityAction onCancelled){
         Log("registerCallback");
         if (onStarted != null)
            _handGestureGrabStartedEvent.First().AddListener(onStarted);
         if (onMoved != null)
            _handGestureGrabMovedEvent.First().AddListener(onMoved);
         if (onEnded != null)
            _handGestureGrabEndedEvent.First().AddListener(onEnded);
         if (onCancelled != null)
            _handGestureGrabCancelledEvent.First().AddListener(onCancelled);
      }

      public void unregisterCallback(UnityAction<HandGrab.Direction, Vector3> onStarted, UnityAction<HandGrab.Direction, Vector3, Vector3> onMoved, UnityAction<HandGrab.Direction, Vector3> onEnded, UnityAction onCancelled){
         Log("unregisterCallback");
         if (onStarted != null)
            _handGestureGrabStartedEvent.First().RemoveListener(onStarted);
         if (onMoved != null)
            _handGestureGrabMovedEvent.First().RemoveListener(onMoved);
         if (onEnded != null)
            _handGestureGrabEndedEvent.First().RemoveListener(onEnded);
         if (onCancelled != null)
            _handGestureGrabCancelledEvent.First().RemoveListener(onCancelled);
      }

      public override void unregisterAllCallbacks(){
         Log("unregisterAllCallbacks");
         if (_handGestureGrabStartedEvent.Count > 1)
            _handGestureGrabStartedEvent.RemoveRange(1, _handGestureGrabStartedEvent.Count - 1);
         if (_handGestureGrabMovedEvent.Count > 1)
            _handGestureGrabMovedEvent.RemoveRange(1, _handGestureGrabMovedEvent.Count - 1);
         if (_handGestureGrabEndedEvent.Count > 1)
            _handGestureGrabEndedEvent.RemoveRange(1, _handGestureGrabEndedEvent.Count - 1);
         if (_handGestureGrabCancelledEvent.Count > 1)
            _handGestureGrabCancelledEvent.RemoveRange(1, _handGestureGrabCancelledEvent.Count - 1);
      }

      public override void unregisterAllInspectorCallbacks(){
         _handGestureGrabStartedEvent.First().RemoveAllListeners();
         _handGestureGrabMovedEvent.First().RemoveAllListeners();
         _handGestureGrabEndedEvent.First().RemoveAllListeners();
         _handGestureGrabCancelledEvent.First().RemoveAllListeners();
      }

      public void registerCallbackFromInspector(HandGrabCallback callback, bool enableOnStartup){
         Log("registerCallbackFromInspector");
         if (enableOnStartup)
            this.Enabled = true;

         if (callback.OnStarted != null)
            _handGestureGrabStartedEvent.Add(callback.OnStarted);
         if (callback.OnMoved != null)
            _handGestureGrabMovedEvent.Add(callback.OnMoved);
         if (callback.OnEnded != null)
            _handGestureGrabEndedEvent.Add(callback.OnEnded);
         if (callback.OnCancelled != null)
            _handGestureGrabCancelledEvent.Add(callback.OnCancelled);
      }  

      public void unregisterCallbackFromInspector(HandGrabCallback callback){
         Log("unregisterCallbackFromInspector");

         if (callback.OnStarted != null)
            _handGestureGrabStartedEvent.Remove(callback.OnStarted);
         if (callback.OnMoved != null)
            _handGestureGrabMovedEvent.Remove(callback.OnMoved);
         if (callback.OnEnded != null)
            _handGestureGrabEndedEvent.Remove(callback.OnEnded);
         if (callback.OnCancelled != null)
            _handGestureGrabCancelledEvent.Remove(callback.OnCancelled);
      }

      public override void handleMessage(params object[] args){
         if (args.Length == 0) return;
         HandGrab.Action handSignalAction = (HandGrab.Action) args[0];
         if (handSignalAction == HandGrab.Action.STARTED && args.Length == 3) {
            notifyStarted((HandGrab.Direction) args[1], (Vector3) args[2]);
            return;
         }
         else if (handSignalAction == HandGrab.Action.MOVED && args.Length == 4) {
            notifyMoved((HandGrab.Direction) args[1], (Vector3) args[2], (Vector3) args[3]);
            return;
         } 
         else if (handSignalAction == HandGrab.Action.ENDED && args.Length == 3) {
            notifyEnded((HandGrab.Direction) args[1], (Vector3) args[2]);
            return;
         } else if (handSignalAction == HandGrab.Action.CANCELLED && args.Length == 1) {
            notifyCancelled();
            return;
         } else
         {
            LogError("handleMessage: Invalid Arguments");
         }
      }

      void notifyStarted(HandGrab.Direction a, Vector3 b){
         Log("notifyStarted");
         foreach (HandGrabStartedEvent trackedEvent in _handGestureGrabStartedEvent) {
            trackedEvent.Invoke(a, b);
         }      
      }
      void notifyMoved(HandGrab.Direction a, Vector3 b, Vector3 c){
         Log("notifyMoved");
         foreach (HandGrabMovedEvent trackedEvent in _handGestureGrabMovedEvent) {
            trackedEvent.Invoke(a, b, c);
         }      
      }

      void notifyEnded(HandGrab.Direction a, Vector3 b){
         Log("notifyEnded");
         foreach (HandGrabEndedEvent trackedEvent in _handGestureGrabEndedEvent) {
            trackedEvent.Invoke(a, b);
         }
      }
      void notifyCancelled(){
         Log("notifyCancelled");
         foreach (HandGrabCancelledEvent trackedEvent in _handGestureGrabCancelledEvent) {
            trackedEvent.Invoke();
         }
      }

      public override void onDestroy(){
         Log("onDestroy");
         this.Enabled = false;
         _handGestureGrabStartedEvent?.Clear();
         _handGestureGrabMovedEvent?.Clear();
         _handGestureGrabEndedEvent?.Clear();
         _handGestureGrabCancelledEvent?.Clear();
         _handGestureGrabStartedEvent = null;
         _handGestureGrabMovedEvent = null;
         _handGestureGrabEndedEvent = null;
         _handGestureGrabCancelledEvent = null;
      }
   }
}