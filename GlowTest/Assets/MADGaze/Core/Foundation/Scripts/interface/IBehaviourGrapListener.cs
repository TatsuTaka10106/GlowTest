using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviourGrapListener
{
	void onGrabStart(int index, int x, int y);
    void onGrabHolding(int index, int x, int y, int dx, int dy);
    void onGrabRelease(int index, int x, int y, int dx, int dy);
    void onGrabCancel(int index, int x, int y, int dx, int dy);
}
