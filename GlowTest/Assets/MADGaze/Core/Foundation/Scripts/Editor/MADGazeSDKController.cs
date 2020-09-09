using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADGazeSDK;

public class MADGazeSDKController  {
 
   public static string API_KEY {
        get { 
            return Settings.APIKey;
        }
    }

    static MADGazeSDKSettings settings;
    public static MADGazeSDKSettings Settings
    {
        get
        {
            if (!settings)
            {
                settings = Resources.Load<MADGazeSDKSettings>("MAD Gaze/SDK Settings");
            }
            return settings;
        }
    }

}
