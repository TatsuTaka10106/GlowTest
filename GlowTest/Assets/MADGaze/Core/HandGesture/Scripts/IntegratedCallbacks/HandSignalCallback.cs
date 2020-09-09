
using UnityEngine.Events;

[System.Serializable]
public class HandSignalCallback {
   public HandSignalTrackedEvent OnTracked;
   public HandSignalLostEvent OnTrackedLost;
   private HandSignalCallback() {}

   public HandSignalCallback(HandSignalTrackedEvent onTracked, HandSignalLostEvent onTrackedLost){
      this.OnTracked = onTracked;
      this.OnTrackedLost = onTrackedLost;
   }
}