
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MADGazeSDK {
public class TrackedHand {
    public enum Direction {
        LEFT,
        RIGHT
    }

    public enum FeaturePointsType {
       THUMB_FINGER_TIP = 4,
       FORE_FINGER_TIP = 8,
       MIDDLE_FINGER_TIP = 12,
       RING_FINGER_TIP = 16,
       LITTLE_FINGER_TIP = 20, 
       PALM_CENTER = 21
    }

    public enum Action {
        TRACKING
    }

    public Direction direction;
    Dictionary<FeaturePointsType, Vector3> data;
    private TrackedHand(){}
    public TrackedHand(Direction direction, Dictionary<FeaturePointsType, Vector3> data){
        this.direction = direction;
        this.data = data;
    }
    public Vector3 getPosition(FeaturePointsType type){
        if (data != null && data.ContainsKey(type))
            return data[type];
        return new Vector3(0.0f, 0.0f, 0.0f);
    }

    public static TrackedHand parse(HandData handData){
      //TODO:
      if(handData != null){
        Direction direction = handData.isLeftHand ? Direction.LEFT : Direction.RIGHT;
        Dictionary<FeaturePointsType, Vector3> data = new Dictionary<FeaturePointsType, Vector3>();
        for (int i = 0; i < handData.fingers.Count; i++){
          Finger finger = handData.fingers[i];
          //FeaturePointsType[] typeArray = Enum.GetValues(typeof(FeaturePointsType));
          FeaturePointsType[] typeArray = (FeaturePointsType[])Enum.GetValues(typeof(FeaturePointsType));
          if(i < typeArray.Length){
              data.Add(typeArray[i], new Vector3(finger.point.X, Screen.height - finger.point.Y, 0.0f));
          }
          // if (Enum.IsDefined(typeof(FeaturePointsType), i)){
          //   data.Add((FeaturePointsType)i, new Vector3(finger.point.X, Screen.height - finger.point.Y, 0.0f));
          // }
        }
         data.Add(FeaturePointsType.PALM_CENTER, new Vector3(handData.palmCenterPoint.X, Screen.height - handData.palmCenterPoint.Y, 0));
        return new TrackedHand(direction, data);
      }
      return null;      
    }
  }
}
