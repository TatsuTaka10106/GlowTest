using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADGazeSDK;
using UnityEngine.UI;
public class MadIdController : MonoBehaviour
{
    public Text madidLabel;
    public void getMadId(){
        MadIdManager.Instance.setMadIdCallback(
            (madid)=>{
                madidLabel.text = madid;
            }
        );

        MadIdManager.Instance.getMadIdFormConnector();
    }
}
