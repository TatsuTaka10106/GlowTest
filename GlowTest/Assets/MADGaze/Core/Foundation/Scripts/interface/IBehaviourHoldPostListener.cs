using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviourHoldPostListener
{
	void onHold(int index, string handType,int x, int y);
    void onHoldCancel(int index, string handType,int x, int y);
}
