using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitUSBSerial
{
	
    public enum MadSensorType 
    {
            MAD_SENSOR_TYPE_ACCELEROMETER = 1,
            MAD_SENSOR_TYPE_GYROSCOPE = 2,
            MAD_SENSOR_TYPE_MAGNETIC_FIELD = 3,
            MAD_SENSOR_TYPE_PROXIMITY = 4,
            MAD_SENSOR_TYPE_AMBIENT_LIGHT  = 5       
    }

    
    private ISplitSensorConnectionCallback mSplitSensorConnectionCallback;
    private ISplitSensorEventListener mSplitSensorEventListener;
    private ISplitSensorListener mISplitSensorListener;

	private AndroidJavaObject nativeController;
    private static SplitUSBSerial _instance;
    public static SplitUSBSerial Instance
    {
        get
        {   
            if(_instance == null){
                _instance = new SplitUSBSerial();
            }
            return _instance;
        }
    }
       
    public Vector3 camLocalPosition {set;get;}
    public Quaternion camLocalRotation {set;get;}


    class ConnectionCallback : AndroidJavaProxy
    {
        public ConnectionCallback() : base("com.madgaze.unityplugin.ConnectionCallback") {}
        void onConnected(){
            SplitUSBSerial.Instance.onConnected();
        }
          void onDisconnected(){
            SplitUSBSerial.Instance.onDisconnected();
        }
          void onError(int errorCode){
            SplitUSBSerial.Instance.onError(errorCode);
        }
    }

    class SensorListener : AndroidJavaProxy
    {
        public SensorListener() : base("com.madgaze.unityplugin.SensorListener") {}
        
        void onCalibratedGyroscope(float x, float y, float z,float w){
            SplitUSBSerial.Instance.onCalibratedGyroscope(x,y,z,w);
        }
        
        void accelerator(float x, float y, float z){
            
        }
        
        void gyroscope(float x, float y, float z, float rotationX,float rotationY,float rotationZ){
            SplitUSBSerial.Instance.gyroscope(x,y,z,rotationX,rotationY,rotationZ);
        }

        void magnetic(float x, float y, float z){

        }
    }


    private SplitSensorEventListener splitSensorEventListener;
    class SplitSensorEventListener : AndroidJavaProxy
    {
        public SplitSensorEventListener() : base("com.felhr.sensors.SplitSensorEventListener") {}


        void onSplitSensorChanged(AndroidJavaObject madSensorEvent){
             SplitUSBSerial.Instance.onSplitSensorChanged(madSensorEvent);
        }

        void onSplitAccuracyChanged(AndroidJavaObject madSensor, int i){
            SplitUSBSerial.Instance.onSplitAccuracyChanged(madSensor,i);
        }
    }


    public SplitUSBSerial()
    {
                
    }


    public void init(){
        #if UNITY_ANDROID
                Debug.Log ("SplitUSBSerial :  enable sensors");
                //MADGazeManager.callFunction("enableSensor");
                nativeController = MADGazeManager.Instance.getFunction("getSplitDeviceSensor");
                splitSensorEventListener = new SplitSensorEventListener();
        #endif
    }

	    public void setSplitSensorEventListener(ISplitSensorEventListener listener){
                mSplitSensorEventListener = listener;
        }
        public void setSplitSensorListener(ISplitSensorListener listener){
            Debug.Log ("SplitUSBSerial :  setSplitSensorListener");
                mISplitSensorListener = listener;

                #if UNITY_ANDROID
                    if(nativeController!=null){
                        var sensorListener = new SensorListener();
                        Debug.Log ("SplitUSBSerial :  setSplitSensorListener start");
                        nativeController.Call("setExternalSensorListener",sensorListener);
                        Debug.Log ("SplitUSBSerial :  setSplitSensorListener end");
                    }
                #endif
        
        }


    	public void setConnectionCallback(ISplitSensorConnectionCallback callback){
                #if UNITY_ANDROID
                    if(nativeController!=null){
                        var ConnectionCallback = new ConnectionCallback();
                        nativeController.Call("setConnectionCallback",ConnectionCallback);
                    }
                #endif
                mSplitSensorConnectionCallback = callback;
        }

    


		public void startSensor(int index){
             #if UNITY_ANDROID
                    if(nativeController!=null){
                        nativeController.Call("startSensor",index);
                    }
             #endif
        }

        public void stopSensor(int index){
             #if UNITY_ANDROID
                    if(nativeController!=null){
                        nativeController.Call("stopSensor",index);
                    }
             #endif
        }

        public bool isConnected(){
             #if UNITY_ANDROID
                    if(nativeController!=null){
                        return nativeController.Call<bool>("isConnected");
                    }
             #endif
             return false;
        }

        public void stopSensorsCapturing(){
        	 #if UNITY_ANDROID
                    if(nativeController!=null){
                        nativeController.Call("stopSensorsCapturing");
                    }
             #endif
    	}
    	
    	public void startSensorsCapturing(){
        
        	 #if UNITY_ANDROID
                    if(nativeController!=null){
                        nativeController.Call("startSensorsCapturing");
                    }
             #endif
    	}	

        public void registerSensorListener(MadSensorType sensorType){
            #if UNITY_ANDROID
                    if(nativeController!=null){
                        nativeController.Call("registerSensorListener", splitSensorEventListener, (int) sensorType);
                    }
             #endif
            
        }


        //================================================ ConnectionCallback ================================================//
        public void onConnected(){
            if(mSplitSensorConnectionCallback!=null){
               mSplitSensorConnectionCallback.onConnected();
            }
        }

        public void onDisconnected(){
           if(mSplitSensorConnectionCallback!=null){
               mSplitSensorConnectionCallback.onDisconnected();
            } 
        }
        
        public void onError(int errorCode){
            if(mSplitSensorConnectionCallback!=null){
               mSplitSensorConnectionCallback.onError(errorCode);
            }
        }
        //================================================ ConnectionCallback ================================================//


        //================================================ SplitSensorEventListener ================================================//
        public void onSplitSensorChanged(AndroidJavaObject madSensorEvent){
            float[] value;
            int sensorType = 0;
            
            value = madSensorEvent.Get<float[]>("values");
            //Debug.Log ("onSplitSensorChanged : value[ "+value[0]+", "+value[1]+", "+value[2]+" ]");

            AndroidJavaObject sensor = madSensorEvent.Get<AndroidJavaObject>("sensor");
            
            if(sensor!=null){
                sensorType = sensor.Get<int>("mSensorType");
            }


            MadSensorType madSensorType = (MadSensorType) sensorType;
            //Debug.Log ("onSplitSensorChanged : sensorType = "+madSensorType.ToString());
            
            if(mSplitSensorEventListener!=null){
                mSplitSensorEventListener.onSplitSensorChanged(value, madSensorType);
            }
        }

        public void onSplitAccuracyChanged(AndroidJavaObject madSensor, int i){
            if(mSplitSensorEventListener!=null){
                mSplitSensorEventListener.onSplitAccuracyChanged(i);
            }
        }
        //================================================ SplitSensorEventListener ================================================//


        //=============================================== SplitSensorListener ===============================================//
        public void onCalibratedGyroscope(float x, float y, float z,float w){
            if(mISplitSensorListener!=null){
                mISplitSensorListener.onCalibratedGyroscope(x, y, z, w);
            }
        }

        public void gyroscope(float x, float y, float z, float rotationX,float rotationY,float rotationZ){
            if(mISplitSensorListener!=null){
                mISplitSensorListener.gyroscope(x, y, z, rotationX, rotationY, rotationZ);
            }
        }
        //=============================================== SplitSensorListener ===============================================//


        private Matrix4x4 calculatedModelMatrix = new Matrix4x4();
        public Quaternion getCalibratedGyroscopeMatrix(){
             
             #if UNITY_ANDROID
                    if(nativeController!=null){

                        float[] M = nativeController.Call<float[]>("getCalibratedGyroscopeMatrix");
                        calculatedModelMatrix.SetColumn(0, new Vector4(M[0], M[1], M[2], M[3]));
                        calculatedModelMatrix.SetColumn(1, new Vector4(M[4], M[5], M[6], M[7]));
                        calculatedModelMatrix.SetColumn(2, new Vector4(M[8], M[9], M[10], M[11]));
                        calculatedModelMatrix.SetColumn(3, new Vector4(M[12], M[13], M[14], M[15]));
                        calculatedModelMatrix.SetRow(2, -calculatedModelMatrix.GetRow(2));
                        Quaternion rotation = QuaternionFromMatrix(calculatedModelMatrix);
                        return rotation;
                    }
             #endif

            return Quaternion.identity;
        }

        //get rotation quaternion from matrix
        public Quaternion QuaternionFromMatrix(Matrix4x4 m)
        {
        // Trap the case where the matrix passed in has an invalid rotation submatrix.
        if (m.GetColumn(2) == Vector4.zero) {
            Debug.Log("QuaternionFromMatrix got zero matrix.");
            return Quaternion.identity;
        }
        return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
        }

        

        public void updateCamTransform (){
            #if UNITY_ANDROID
                if(nativeController!=null){

                float[] M = nativeController.Call<float[]>("getCalibratedGyroscopeMatrix");
                Debug.Log("SplitUSBSerial: updateCamTransform");
                if(M!=null){
                    Debug.Log("SplitUSBSerial: updateCamTransform: matrix != null");
                    calculatedModelMatrix.SetColumn(0, new Vector4(M[0], M[1], M[2], M[3]));
                    calculatedModelMatrix.SetColumn(1, new Vector4(M[4], M[5], M[6], M[7]));
                    calculatedModelMatrix.SetColumn(2, new Vector4(M[8], M[9], M[10], M[11]));
                    calculatedModelMatrix.SetColumn(3, new Vector4(M[12], M[13], M[14], M[15]));

                    Matrix4x4 transformationMatrix = LHMatrixFromRHMatrix (calculatedModelMatrix);
                    Matrix4x4 pose = transformationMatrix.inverse;

                    Vector3 arPosition = PositionFromMatrix (pose);
                    Quaternion arRotation = QuaternionFromMatrix (pose);

                    camLocalPosition = arPosition;
                    camLocalRotation = arRotation;
                    Debug.Log("SplitUSBSerial: updateCamTransform: matrix != null");
                }
            }
            #endif
    }


    public static Matrix4x4 LHMatrixFromRHMatrix(Matrix4x4 rhm)
    {
        Matrix4x4 lhm = new Matrix4x4();;

        // Column 0.
        lhm[0, 0] =  rhm[0, 0];
        lhm[1, 0] =  rhm[1, 0];
        lhm[2, 0] = -rhm[2, 0];
        lhm[3, 0] =  rhm[3, 0];

        // Column 1.
        lhm[0, 1] =  rhm[0, 1];
        lhm[1, 1] =  rhm[1, 1];
        lhm[2, 1] = -rhm[2, 1];
        lhm[3, 1] =  rhm[3, 1];

        // Column 2.
        lhm[0, 2] = -rhm[0, 2];
        lhm[1, 2] = -rhm[1, 2];
        lhm[2, 2] =  rhm[2, 2];
        lhm[3, 2] = -rhm[3, 2];

        // Column 3.
        lhm[0, 3] =  rhm[0, 3];
        lhm[1, 3] =  rhm[1, 3];
        lhm[2, 3] = -rhm[2, 3];
        lhm[3, 3] =  rhm[3, 3];

        return lhm;
    }

 //get position from transform matrix
    public static Vector3 PositionFromMatrix(Matrix4x4 m)
    {
        return m.GetColumn(3);
    }



}
