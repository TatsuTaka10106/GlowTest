using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using MADGazeSDK;
namespace MADGazeSDK {
   public class HandTrackingController : HandGestureController {
      List<HandTrackingEvent> _handGestureTrackingEvent;
      
      public HandTrackingController(){
         _handGestureTrackingEvent = new List<HandTrackingEvent>();
         _handGestureTrackingEvent.Add(new HandTrackingEvent());
      }

      protected override void notifyCore(){
         Log("notifyCore");
         //NotifyCore of Enable Issue
         MADUnityIntegrator.Instance.regDetectedListener(this.Enabled);
      }

      public void registerCallback(UnityAction<TrackedHand, TrackedHand> onTracking) {
         Log("registerCallback");

         if (onTracking != null)
            _handGestureTrackingEvent.First().AddListener(onTracking);
      }
      public void unregisterCallback(UnityAction<TrackedHand, TrackedHand> onTracking) {
         Log("unregisterCallback");

         if (onTracking != null)
            _handGestureTrackingEvent.First().RemoveListener(onTracking);
      }
      
      public override void unregisterAllCallbacks(){
         Log("unregisterAllCallbacks");
         if (_handGestureTrackingEvent.Count > 1)
            _handGestureTrackingEvent.RemoveRange(1, _handGestureTrackingEvent.Count - 1);
      }

      public override void unregisterAllInspectorCallbacks(){
         _handGestureTrackingEvent.First().RemoveAllListeners();
      }

      public void registerCallbackFromInspector(HandTrackingCallback callback, bool enableOnStartup){
         Log("registerCallbackFromInspector");
         if (enableOnStartup)
            this.Enabled = true;
         if (callback.OnTracking != null)
            _handGestureTrackingEvent.Add(callback.OnTracking);
      }
      public void unregisterCallbackFromInspector(HandTrackingCallback callback){
         Log("unregisterCallbackFromInspector");
         if (callback.OnTracking != null)
            _handGestureTrackingEvent.Remove(callback.OnTracking);
      }

       public override void handleMessage(params object[] args){
         if (args.Length == 0) return;
         TrackedHand.Action action = (TrackedHand.Action) args[0];
         if (action == TrackedHand.Action.TRACKING && args.Length == 3) {
            notifyTracking((TrackedHand)args[1], (TrackedHand)args[2]);
            return;
         } else
         {
            LogError("handleMessage: Invalid Arguments");
         }
      }

      void notifyTracking(TrackedHand a, TrackedHand b){
         Log("notifyTracking");
         foreach (HandTrackingEvent trackedEvent in _handGestureTrackingEvent) {
            trackedEvent.Invoke(a, b);
         }
      }
      public override void onDestroy(){
         Log("onDestroy");
         this.Enabled = false;
         _handGestureTrackingEvent?.Clear();
         _handGestureTrackingEvent = null;
      }

   }
}