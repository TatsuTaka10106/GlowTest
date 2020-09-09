
using UnityEngine.Events;

[System.Serializable]
public class HandGrabCallback {
   public HandGrabStartedEvent OnStarted;
   public HandGrabMovedEvent OnMoved;
   public HandGrabEndedEvent OnEnded;
   public HandGrabCancelledEvent OnCancelled;
   private HandGrabCallback() {}

   public HandGrabCallback(HandGrabStartedEvent onStarted, HandGrabMovedEvent onMoved, HandGrabEndedEvent onEnded, HandGrabCancelledEvent onCancelled){
      this.OnStarted = onStarted;
      this.OnMoved = onMoved;
      this.OnEnded = onEnded;
      this.OnCancelled = onCancelled;
   }
}