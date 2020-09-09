
using UnityEngine.Events;

[System.Serializable]
public class HandCursorCallback {
   public HandCursorTrackedEvent OnTracked;
   public HandCursorClickedEvent OnClicked;
   public HandCursorMovedEvent OnMoved;
   public HandCursorLostEvent OnTrackedLost;

   private HandCursorCallback() {}

   public HandCursorCallback(HandCursorTrackedEvent onTracked, HandCursorClickedEvent onClicked, HandCursorMovedEvent onMoved, HandCursorLostEvent onTrackedLost){
      this.OnTracked = onTracked;
      this.OnClicked = onClicked;
      this.OnMoved = onMoved;
      this.OnTrackedLost = onTrackedLost;
   }
}