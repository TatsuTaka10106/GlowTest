using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MADGazeSDK {
    internal class SDKLoader
    {
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            new GameObject(nameof(MADCallbackManager), typeof(MADCallbackManager));
        }
    }
}
