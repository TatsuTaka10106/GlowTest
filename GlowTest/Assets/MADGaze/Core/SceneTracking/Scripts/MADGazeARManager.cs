using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace MADGazeSDK {
    
    public class MADGazeARManager 
    {
   private static MADGazeARManager instance = null;  
      public static MADGazeARManager Instance
      {
         get
         { 
            if (instance == null) 
                instance = new MADGazeARManager();
            return instance; 
         }
      }


    AndroidJavaObject nativeController;
    AndroidJavaObject mUnityObjModel;
    // AndroidJavaObject mUnityObjectRenderer;

    
      public MADGazeARManager()
   {
		init();
   }
	
	protected void init(){

        isShowModel = false;

        #if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		if(jc!= null){
	   		nativeController = jc.GetStatic<AndroidJavaObject>("currentActivity"); 
            mUnityObjModel = nativeController.Call<AndroidJavaObject>("getUnityObjModel");
            
            var madModelListener = new MadModelListener();
            nativeController.Call("setMadModelListener",madModelListener);

            
        }
        #endif

       
    }


    class MadModelListener : AndroidJavaProxy
    {
        public MadModelListener() : base("com.madgaze.slam.listener.MadModelListener") {}

        void onModelTracked(bool tracked){
			MADGazeARManager.Instance.isShowModel = tracked;
    	}

        async void onModelPlace(bool isSuccess){
            await MADCallbackManager.Instance.EnqueueAsync(()=>MADGazeARManager.Instance.onModelPlace(isSuccess));
        }
    }
	
    void onModelPlace(bool isSuccess){
        if(mActionOnModelPlaceCallback!=null){
            mActionOnModelPlaceCallback(isSuccess);
        }
    }

    public void requestAppPurchaseStatus(Action<bool,string> onSuccess, Action<string> onFail){
    
    }


    Matrix4x4 viewMatrix = new Matrix4x4();
    Matrix4x4 modelMatrix = new Matrix4x4();
    Matrix4x4 projectionMatrix = new Matrix4x4();
    //Matrix4x4 calculatedModelMatrix = new Matrix4x4();

  
        public  void onUpdateViewMatrix(float[] M){		
            viewMatrix.SetColumn(0, new Vector4(M[0], M[1], M[2], M[3]));
            viewMatrix.SetColumn(1, new Vector4(M[4], M[5], M[6], M[7]));
            viewMatrix.SetColumn(2, new Vector4(M[8], M[9], M[10], M[11]));
            viewMatrix.SetColumn(3, new Vector4(M[12], M[13], M[14], M[15]));
        
            //viewMatrix.SetRow(2, -viewMatrix.GetRow(2));
    	}


      	public 	void onUpdateModelMatrix(float[]M){
			modelMatrix.SetColumn(0, new Vector4(M[0], M[1], M[2], M[3]));
            modelMatrix.SetColumn(1, new Vector4(M[4], M[5], M[6], M[7]));
            modelMatrix.SetColumn(2, new Vector4(M[8], M[9], M[10], M[11]));
            modelMatrix.SetColumn(3, new Vector4(M[12], M[13], M[14], M[15]));
      	}

    	public  void onUpdateProjectionMatrix(float[] M){
            projectionMatrix.SetColumn(0, new Vector4(M[0], M[1], M[2], M[3]));
            projectionMatrix.SetColumn(1, new Vector4(M[4], M[5], M[6], M[7]));
            projectionMatrix.SetColumn(2, new Vector4(M[8], M[9], M[10], M[11]));
            projectionMatrix.SetColumn(3, new Vector4(M[12], M[13], M[14], M[15]));
        }


        public void requestReset(){

		}

    	public void setDraw(bool flag){
            // if(onModelTrackedCallback!=null){
            //     onModelTrackedCallback(flag);
            // }
		}

    public static Vector3 PositionFromMatrix(Matrix4x4 m)
    {
        return m.GetColumn(3);
    }

    //get rotation quaternion from matrix
    public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
    {
        // Trap the case where the matrix passed in has an invalid rotation submatrix.
        if (m.GetColumn(2) == Vector4.zero) {
            return Quaternion.identity;
        }
        return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
    }

        
        public Vector3 camArPosition = Vector3.zero;
        public Quaternion camArRotation = Quaternion.identity;

        public bool IsCameraReady(){
            Vector3 v = PositionFromMatrix(projectionMatrix);
            if(v.x == 0 && v.y == 0 && v.z == 0){
                return false;
            }
            return true;
        }


        public Matrix4x4 getProjectMatrix(){
            return projectionMatrix;
        }
        public void onUpdateObject(Camera cam,Transform model){
            if(mUnityObjModel!=null){
                mUnityObjModel.Call("onUpdate");
            
                float[] PM = mUnityObjModel.Call<float[]>("getPM");
                onUpdateProjectionMatrix(PM);
                
            
                if(IsCameraReady()){
                cam.projectionMatrix = projectionMatrix;
                
                float[] VM = mUnityObjModel.Call<float[]>("getVM");
                onUpdateViewMatrix(VM);

                
                float[] MMM = mUnityObjModel.Call<float[]>("getMMM");
                onUpdateModelMatrix(MMM);
                modelMatrix.SetRow(2, -modelMatrix.GetRow(2)); 
           
                updateModelTransform(modelMatrix);
                updateCamTransform(viewMatrix);


                if(isShowModel != model.gameObject.activeSelf){
                    if(onActionModelTrackedCallback!=null){
                        onActionModelTrackedCallback(isShowModel);
                }

                }
                }
            }   


        }
        
        public Action<Vector3,Quaternion> oncameraUpdateCallback; 
        public Action<Vector3,Quaternion,Vector3> onModelUpdateCallback; 
        public Action<Boolean> onActionModelTrackedCallback; 
        private Action<bool> mActionOnModelPlaceCallback;
        private bool isShowModel;
        public void setCameraPositionCallback(Action<Vector3, Quaternion> callback){
             oncameraUpdateCallback  =  callback;
        }
        
        public void setModelPositionCallback(Action<Vector3, Quaternion, Vector3> callback){
             onModelUpdateCallback  =  callback;
        }

        public void setModelTrackedCallback(Action<Boolean> callback){
             onActionModelTrackedCallback  =  callback;
        }


        // public void setModelPlaceCallback(Action<Boolean> callback){
        //      mActionOnModelPlaceCallback = callback;
        // }
        private void updateModelTransform(Matrix4x4 modelMatrix){
            Vector3 position = PositionFromMatrix(modelMatrix);
            Quaternion rotation = QuaternionFromMatrix(modelMatrix);
            Vector3 localScale = ExtractScale(modelMatrix); 

            if(onModelUpdateCallback!=null){
                onModelUpdateCallback(position, rotation, localScale);
            }
        }

        private void updateCamTransform (Matrix4x4 viewMatrix){
                Matrix4x4 transformationMatrix = LHMatrixFromRHMatrix (viewMatrix);
                Matrix4x4 pose = transformationMatrix.inverse;

                Vector3 arPosition = PositionFromMatrix (pose);
                Quaternion arRotation = QuaternionFromMatrix (pose);

                camArPosition = arPosition;
                camArRotation = arRotation;

                if(oncameraUpdateCallback!=null){
                    oncameraUpdateCallback(camArPosition,camArRotation);
                }
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


    public void showGridObject(bool isShow){
        #if UNITY_ANDROID
		if(nativeController!=null){
            nativeController.Call("showGridObject",isShow);
        }
        #endif  
    }


   
    public void resetTargetPosition(Action<Boolean> callback){
        
        mActionOnModelPlaceCallback = callback;

        #if UNITY_ANDROID
            if(nativeController!=null){
                nativeController.Call("resetModelPosition");
            }
        #endif    
    }
}

  
}