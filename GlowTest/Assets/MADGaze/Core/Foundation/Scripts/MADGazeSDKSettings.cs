using System;
using UnityEngine;

namespace MADGazeSDK {
    [CreateAssetMenu(menuName = "MAD Gaze/Settings")]
    public class MADGazeSDKSettings : ScriptableObject
    {
        [HideInInspector, SerializeField]
        [TextArea(1, 10)]
        public string APIKey;
        
    }
}