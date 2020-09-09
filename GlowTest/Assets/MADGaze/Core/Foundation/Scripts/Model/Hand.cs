using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADGazeSDK;
using System;
public class Hand
{
    private AndroidJavaObject nativeHand;
    public List<HandData> handDatas;
    public Hand(){
    	//Debug.Log("DemoHandGesture: Hand init");   
		handDatas = new List<HandData>();
    }
	//ArrayList<Hand.Data> data = new ArrayList();
    private AndroidJavaObject nativeDataList;
    public void updateData(AndroidJavaObject native){
    	// Debug.Log("DemoHandGesture: Hand: updateData");   
    	
    	
    	handDatas.Clear();

        #if UNITY_ANDROID
        	nativeHand = native;
        	if(nativeHand!=null){
            	nativeDataList = nativeHand.Call<AndroidJavaObject>("getData");
            	if(nativeDataList!=null){

            		int size = nativeDataList.Call<int>("size");
            		//Debug.Log("DemoHandGesture: Hand: updateData nativeDataList size = "+size);  
            		for(int i =0; i < size; i++){
						AndroidJavaObject nativeHandData = nativeDataList.Call<AndroidJavaObject>("get",i);
						handDatas.Add(new HandData(nativeHandData));
            		}
				}
            }
        #endif
    }




}


   namespace MADGazeSDK
{

	public class HandData {
		public HandType handType{get;set;}
		public Point palmCenterPoint{get;set;}
		public Point firstFingerPoint{get;set;}
		public List<Finger> fingers{get;set;}
		public bool isLeftHand{get;set;}
		public bool isPinch{get;set;}

		public HandData(AndroidJavaObject handData){
			
			fingers = new List<Finger>();
			palmCenterPoint = new Point();
			firstFingerPoint = new Point();

			update(handData);
		}

		public void update(AndroidJavaObject handData){
			//getHandType
			AndroidJavaObject nativeHandType = handData.Call<AndroidJavaObject>("getHandType");
			string typeName = nativeHandType.Call<string>("name");
			Debug.Log("HandData: update typeName = "+typeName);  
			//handType = (HandType)Enum.Parse(typeof(HandType), typeName);
			if( System.Enum.TryParse(typeName, out HandType type) ){
				handType = type;
			}else{
				handType = HandType.UNKNOWN;
			}

			//getPalmCenterPoint
			AndroidJavaObject nativePalmCenterPoint = handData.Call<AndroidJavaObject>("getPalmCenterPoint");
			int centerPointX = nativePalmCenterPoint.Call<int>("getX");
			int centerPointY = nativePalmCenterPoint.Call<int>("getY");
			palmCenterPoint  = new Point(centerPointX, centerPointY);

			//getFirstFingerPoint
			AndroidJavaObject nativeFirstFingerPoint = handData.Call<AndroidJavaObject>("getFirstFingerPoint");
			int fFingerPointX = nativeFirstFingerPoint.Call<int>("getX");
			int fFingerPointY = nativeFirstFingerPoint.Call<int>("getY");
			firstFingerPoint  = new Point(fFingerPointX, fFingerPointY);			
			//getLeftHand
			isLeftHand = handData.Call<bool>("getLeftHand");
			isPinch = handData.Call<bool>("isPinch");
			AndroidJavaObject nativeFingers = handData.Call<AndroidJavaObject>("getFingers");
			int fingerSize = nativeFingers.Call<int>("size");

			fingers.Clear();

			for(int i =0;i < fingerSize;i++){
				AndroidJavaObject nativeF = nativeFingers.Call<AndroidJavaObject>("get",i);
				AndroidJavaObject nativePoint = nativeF.Call<AndroidJavaObject>("getPoint");
				int px = nativePoint.Call<int>("getX");
				int py = nativePoint.Call<int>("getY");
				Point point = new Point(px, py);
				Finger finger = new Finger(point);
				fingers.Add(finger);
			}
		}
	}

	public enum HandType {
		ONE,
        TWO,
        THREE,
		FOUR,
        FIVE,
        YEAH,
        OK,
		FIST,
		UNKNOWN
	}

	public class Finger{
		public Point point{get;set;}
		public Finger(Point p){
			point = p;
		}
	}

	public class Point {
		public int X { get; set; }
		public int Y { get; set; }
		public Point() {
			X = 0;
			Y = 0;
        }
		public Point(int x, int y) {
			X = x;
			Y = y;
        }
	}


}