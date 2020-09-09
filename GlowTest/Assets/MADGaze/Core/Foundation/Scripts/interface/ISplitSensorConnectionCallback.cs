using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISplitSensorConnectionCallback
{
         void onConnected();
         void onDisconnected();
         void onError(int errorCode);
}
