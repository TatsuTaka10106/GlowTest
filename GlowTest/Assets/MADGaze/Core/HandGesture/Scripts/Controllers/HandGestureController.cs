using UnityEngine;

namespace MADGazeSDK {
   public abstract class HandGestureController {
      private bool enabled;
      public bool Enabled {
         get { return this.enabled; }
         set { this.enabled = value; notifyCore(); Log("setEnabled: " + value); }
      }

      protected void Log(string msg) {
         if (!HandGestureManager.Instance.DEBUG_MODE) return;
         Debug.Log("[" + this.GetType().Name + "] " + msg);
      }

      protected void LogError(string msg){
         if (!HandGestureManager.Instance.DEBUG_MODE) return;
         Debug.LogError("[" + this.GetType().Name + "] " + msg);
      }

      public HandGestureController(){ this.enabled = false;  }
      protected abstract void notifyCore();
      public abstract void handleMessage(params object[] args);
      public abstract void unregisterAllCallbacks();
      public abstract void unregisterAllInspectorCallbacks();
      public abstract void onDestroy();  

   }
}