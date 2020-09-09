
using UnityEngine.Events;
using MADGazeSDK;

[System.Serializable]
public class HandTrackingCallback {

   public HandTrackingEvent OnTracking;
   private HandTrackingCallback() {}
   public HandTrackingCallback(HandTrackingEvent onTracking){
      this.OnTracking = onTracking;
   }

}