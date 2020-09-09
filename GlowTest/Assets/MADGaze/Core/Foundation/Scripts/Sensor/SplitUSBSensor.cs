using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitUSBSensor
{
    
    private AndroidJavaObject nativeController;
    private static SplitUSBSensor _instance;
    public static SplitUSBSensor Instance
    {
        get
        {   
            if(_instance == null){
                _instance = new SplitUSBSensor();
            }
            return _instance;
        }
    }

    public void init(){
        #if UNITY_ANDROID
                
                if(MADGazeManager.Instance!=null){
                    nativeController = MADGazeManager.Instance.getFunction("getSensor");
                }
                //splitSensorEventListener = new SplitSensorEventListener();
        #endif

                camLocalPosition = Vector3.zero;
                camLocalRotation = Quaternion.identity;
    }




    // private Matrix4x4 calculatedModelMatrix = new Matrix4x4();
    public Vector3 camLocalPosition {set;get;}
    public Quaternion camLocalRotation {set;get;}
    public Matrix4x4 projectionMatrix = new Matrix4x4();
    public bool isRotation = false;


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
        if(m!=null){
            return m.GetColumn(3);
        }else{
            return Vector3.zero;
        }
    }

     //get rotation quaternion from matrix
        public Quaternion QuaternionFromMatrix(Matrix4x4 m)
        {
        // Trap the case where the matrix passed in has an invalid rotation submatrix.
        if (m==null || m.GetColumn(2) == Vector4.zero) {
            Debug.Log("QuaternionFromMatrix got zero matrix.");
            return Quaternion.identity;
        }
        return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
        }


 public Vector3 ExtractScale(Matrix4x4 matrix)
    {
        if(matrix!=null){
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;

            return scale;
        }else{
            return Vector3.zero;
        }
    }

     public void updateViewMatrixTransform (Transform camTransform){
        if(nativeController!=null && camTransform!=null){
            float[] M = nativeController.Call<float[]>("getViewMatrix");
            Matrix4x4 camMatrix = new Matrix4x4();
            camMatrix.SetColumn(0, new Vector4(M[0], M[1], M[2], M[3]));
            camMatrix.SetColumn(1, new Vector4(M[4], M[5], M[6], M[7]));
            camMatrix.SetColumn(2, new Vector4(M[8], M[9], M[10], M[11]));
            camMatrix.SetColumn(3, new Vector4(M[12], M[13], M[14], M[15]));

            Matrix4x4 transformationMatrix = LHMatrixFromRHMatrix (camMatrix);
            Matrix4x4 pose = transformationMatrix.inverse;
            // Matrix4x4 pose = transformationMatrix;
            Vector3 arPosition = PositionFromMatrix (pose);
            Quaternion arRotation = QuaternionFromMatrix (pose);

            // camTransform.position = arPosition;
            camTransform.rotation = arRotation;
        }
     }


    private Matrix4x4 mCamMatrix;
    public void updateCamMatrix (){
        if(nativeController!=null){
            float[] M = nativeController.Call<float[]>("getViewMatrix");
            Matrix4x4 camMatrix = new Matrix4x4();
            camMatrix.SetColumn(0, new Vector4(M[0], M[1], M[2], M[3]));
            camMatrix.SetColumn(1, new Vector4(M[4], M[5], M[6], M[7]));
            camMatrix.SetColumn(2, new Vector4(M[8], M[9], M[10], M[11]));
            camMatrix.SetColumn(3, new Vector4(M[12], M[13], M[14], M[15]));

            Matrix4x4 transformationMatrix = LHMatrixFromRHMatrix (camMatrix);
            mCamMatrix = transformationMatrix.inverse;
        }
     }

    private Matrix4x4 getCamMatrix4x4(){
        return mCamMatrix;
    }
    public Quaternion getCameraRotation(){
        Quaternion arRotation = QuaternionFromMatrix (mCamMatrix);
        return arRotation;
    }


    private ISplitSensorConnectionCallback mSplitSensorConnectionCallback;
    class ConnectionCallback : AndroidJavaProxy
    {
        public ConnectionCallback() : base("com.madgaze.unityplugin.ConnectionCallback") {}
        async void onConnected(){
            await MADCallbackManager.Instance.EnqueueAsync(()=>SplitUSBSensor.Instance.onConnected());
        }
        async void onDisconnected(){
            await MADCallbackManager.Instance.EnqueueAsync(()=>SplitUSBSensor.Instance.onDisconnected());
        }
        async void onError(int errorCode){
            await MADCallbackManager.Instance.EnqueueAsync(()=>SplitUSBSensor.Instance.onError(errorCode));
        }
    }

    public void setConnectionCallback(ISplitSensorConnectionCallback callback){
                #if UNITY_ANDROID
                    if(nativeController!=null){
                        var connectionCallback = new ConnectionCallback();
                        nativeController.Call("setConnectionCallback",connectionCallback);
                    }
                #endif
                mSplitSensorConnectionCallback = callback;
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
         public void startSensorsCapturing(){
            #if UNITY_ANDROID
                    if(nativeController!=null){
                        nativeController.Call("startSensorsCapturing");
                    }
                #endif    
        }

        public void stopSensorsCapturing(){
            #if UNITY_ANDROID
                    if(nativeController!=null){
                        nativeController.Call("stopSensorsCapturing");
                    }
                #endif    
        }

         public bool isDeviceConnected(){
            #if UNITY_ANDROID
                    if(nativeController!=null){
                        return nativeController.Call<bool>("isDeviceConnected");
                    }
                #endif    
            return false;
        }
}
