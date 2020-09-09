using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using MADGazeSDK;
namespace MADGazeSDK {
   public class HandCursorController : HandGestureController {

      List<HandCursorTrackedEvent> _handGestureCursorTrackedEvent;
      List<HandCursorMovedEvent> _handGestureCursorMovedEvent;
      List<HandCursorClickedEvent> _handGestureCursorClickedEvent;
      List<HandCursorLostEvent> _handGestureCursorLostEvent;

      public HandCursorController(){
         _handGestureCursorTrackedEvent = new List<HandCursorTrackedEvent>();
         _handGestureCursorMovedEvent = new List<HandCursorMovedEvent>();
         _handGestureCursorClickedEvent = new List<HandCursorClickedEvent>();
         _handGestureCursorLostEvent = new List<HandCursorLostEvent>();

         _handGestureCursorTrackedEvent.Add(new HandCursorTrackedEvent());
         _handGestureCursorMovedEvent.Add(new HandCursorMovedEvent());
         _handGestureCursorClickedEvent.Add(new HandCursorClickedEvent());
         _handGestureCursorLostEvent.Add(new HandCursorLostEvent());
      }

      protected override void notifyCore(){
         Log("notifyCore");
         //NotifyCore of Enable Issue
         MADUnityIntegrator.Instance.regClickListener(this.Enabled);
      }

      public void registerCallback(
         UnityAction<HandCursor.Direction, Vector3> onTracked, 
         UnityAction<HandCursor.Direction, Vector3, Vector3> onMoved,
         UnityAction<HandCursor.Direction, Vector3, Vector3> onClicked,
         UnityAction onTrackedLost){
         Log("registerCallback");

         if (onTracked != null)
            _handGestureCursorTrackedEvent.First().AddListener(onTracked);
         if (onMoved != null)
            _handGestureCursorMovedEvent.First().AddListener(onMoved);
         if (onClicked != null)
            _handGestureCursorClickedEvent.First().AddListener(onClicked);
         if (onTrackedLost != null)
            _handGestureCursorLostEvent.First().AddListener(onTrackedLost);
      }

      public void unregisterCallback(
         UnityAction<HandCursor.Direction, Vector3> onTracked, 
         UnityAction<HandCursor.Direction, Vector3, Vector3> onMoved,
         UnityAction<HandCursor.Direction, Vector3, Vector3> onClicked,
         UnityAction onTrackedLost){
         Log("unregisterCallback");
            
         if (onTracked != null)
            _handGestureCursorTrackedEvent.First().RemoveListener(onTracked);
         if (onMoved != null)
            _handGestureCursorMovedEvent.First().RemoveListener(onMoved);
         if (onClicked != null)
            _handGestureCursorClickedEvent.First().RemoveListener(onClicked);
         if (onTrackedLost != null)
            _handGestureCursorLostEvent.First().RemoveListener(onTrackedLost);
      }
      
      public override void unregisterAllCallbacks(){
         Log("unregisterAllCallbacks");
         if (_handGestureCursorTrackedEvent.Count > 1)
            _handGestureCursorTrackedEvent.RemoveRange(1, _handGestureCursorTrackedEvent.Count - 1);
         if (_handGestureCursorMovedEvent.Count > 1)
            _handGestureCursorMovedEvent.RemoveRange(1, _handGestureCursorMovedEvent.Count - 1);
         if (_handGestureCursorClickedEvent.Count > 1)
            _handGestureCursorClickedEvent.RemoveRange(1, _handGestureCursorClickedEvent.Count - 1);
         if (_handGestureCursorLostEvent.Count > 1)
            _handGestureCursorLostEvent.RemoveRange(1, _handGestureCursorLostEvent.Count - 1);
      }

      public override void unregisterAllInspectorCallbacks(){
         _handGestureCursorTrackedEvent.First().RemoveAllListeners();
         _handGestureCursorMovedEvent.First().RemoveAllListeners();
         _handGestureCursorClickedEvent.First().RemoveAllListeners();
         _handGestureCursorLostEvent.First().RemoveAllListeners();
      }

      public void registerCallbackFromInspector(HandCursorCallback callback, bool enableOnStartup){
         Log("registerCallbackFromInspector");
         if (enableOnStartup)
            this.Enabled = true;
         
         if (callback.OnTracked != null)
            _handGestureCursorTrackedEvent.Add(callback.OnTracked);
         if (callback.OnMoved != null)
            _handGestureCursorMovedEvent.Add(callback.OnMoved);
         if (callback.OnClicked != null)
            _handGestureCursorClickedEvent.Add(callback.OnClicked);
         if (callback.OnTrackedLost != null)
            _handGestureCursorLostEvent.Add(callback.OnTrackedLost);
      }  

      public void unregisterCallbackFromInspector(HandCursorCallback callback){
         Log("unregisterCallbackFromInspector");
         if (callback.OnTracked != null)
            _handGestureCursorTrackedEvent.Remove(callback.OnTracked);
         if (callback.OnMoved != null)
            _handGestureCursorMovedEvent.Remove(callback.OnMoved);
         if (callback.OnClicked != null)
            _handGestureCursorClickedEvent.Remove(callback.OnClicked);
         if (callback.OnTrackedLost != null)
            _handGestureCursorLostEvent.Remove(callback.OnTrackedLost);
      }

      public override void handleMessage(params object[] args){
         if (args.Length == 0) return;
         HandCursor.Action action = (HandCursor.Action) args[0];
         if (action == HandCursor.Action.TRACKED && args.Length == 3) {
            notifyTracked((HandCursor.Direction)args[1], (Vector3)args[2]);
            return;
         }
         else if (action == HandCursor.Action.MOVED && args.Length == 4) {
            notifyMoved((HandCursor.Direction)args[1], (Vector3)args[2], (Vector3)args[3]);
            return;
         }
         else if (action == HandCursor.Action.CLICKED && args.Length == 4) {
            notifyClicked((HandCursor.Direction)args[1], (Vector3)args[2], (Vector3)args[3]);
            return;
         }
         else if (action == HandCursor.Action.LOST && args.Length == 1) {
            notifyTrackedLost();
            return;
         } else
         {
            LogError("handleMessage: Invalid Arguments");
         }
      }

      void notifyTracked(HandCursor.Direction a, Vector3 b){
         Log("notifyTracked");
         foreach (HandCursorTrackedEvent trackedEvent in _handGestureCursorTrackedEvent) {
            trackedEvent.Invoke(a, b);
         }
      }
      void notifyMoved(HandCursor.Direction a, Vector3 b, Vector3 c){
         Log("notifyMoved");
         foreach (HandCursorMovedEvent trackedEvent in _handGestureCursorMovedEvent) {
            trackedEvent.Invoke(a, b, c);
         }
      }

      void notifyClicked(HandCursor.Direction a, Vector3 b, Vector3 c){
         Log("notifyClicked");
         foreach (HandCursorClickedEvent trackedEvent in _handGestureCursorClickedEvent) {
            trackedEvent.Invoke(a, b, c);
         }
      }

      void notifyTrackedLost(){
         Log("notifyTrackedLost");
         foreach (HandCursorLostEvent trackedEvent in _handGestureCursorLostEvent) {
            trackedEvent.Invoke();
         }
      }

      public override void onDestroy(){
         Log("onDestroy");
         this.Enabled = false;
         _handGestureCursorTrackedEvent?.Clear();
         _handGestureCursorMovedEvent?.Clear();
         _handGestureCursorClickedEvent?.Clear();
         _handGestureCursorLostEvent?.Clear();
         _handGestureCursorTrackedEvent = null;
         _handGestureCursorMovedEvent = null;
         _handGestureCursorClickedEvent = null;
         _handGestureCursorLostEvent = null;
      }
   }
}