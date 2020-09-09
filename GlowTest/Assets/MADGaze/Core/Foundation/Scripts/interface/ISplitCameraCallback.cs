using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISplitCameraCallback
{
	void onConnected();
    void onDisconnected();
	void onError(int errorCode);
}
