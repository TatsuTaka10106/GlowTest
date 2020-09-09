using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DemoCamera : BaseMADGazeMonoBehaviour , ISplitCameraCallback{

		// [SerializeField] Text txtLabel1;

		// [SerializeField] Text txtPreviewLabel;

		Texture2D mTexture2D;
		public Image bgImage;

		private bool isReady = true;

		private string cameraMessage = "";
	
		public void Start () {
 			isReady = true;
			mTexture2D = new Texture2D(640, 480, TextureFormat.ARGB32, false);
			
			//init split camera
			// SplitCamera.Instance.init();
			//set split camera connection callback
			SplitCamera.Instance.setCameraCallback(this);
		}



	public void onConnected(){
		//Connected Camera 
		Debug.Log("DemoCamera:onSplitCameraConnected");
		cameraMessage = "Camera: connected - PreviewSize ["+SplitCamera.Instance.getPreviewWidth()+","+SplitCamera.Instance.getPreviewHeight()+"]";
		updatePreviewLabelTxt(false);
	}
	
	public void onDisconnected(){
		//onDisconnected Camera 
		Debug.Log("DemoCamera:onSplitCameraDisconnected");
		cameraMessage = "Camera: disconnected";
		updatePreviewLabelTxt(true);
	}
	
	public void onError(int errorCode){
		//onError Camera errorCode 
		int error = errorCode;
		if (error == -1){
        		Debug.Log("DemoCamera: There is no connecting MAD Gaze Cameras.");
                cameraMessage = "Please connect your Camera device...";
            } else{
                Debug.Log("DemoCamera: MAD Gaze Camera Init Failure (Error=" + error + ")");
                cameraMessage = "MAD Gaze Camera Init Failure (Error=" + error + ")";
            }
	}



	private void Update(){
		   
			// txtLabel1.text = cameraMessage;	
			
			//Split Camera connected
			if(SplitCamera.Instance.isDeviceConnected()){
			if(isReady){
				isReady = false;
				//Get Camera Result
	    		byte[] data = SplitCamera.Instance.getPreviewResult();
				if(data!=null){
					// Update Preview Splite Image
					mTexture2D.LoadImage(data);
					Sprite sprite = Sprite.Create (mTexture2D, new Rect(0,0,mTexture2D.width,mTexture2D.height), new Vector2(.5f,.5f));
					bgImage.sprite = sprite;
					bgImage.color = new Color32(255,255,225,255);
					// txtLabel1.text = cameraMessage+" data: "+data.Length;
        		}else{
        			// txtLabel1.text = cameraMessage+" data = null";
        		}
        		isReady = true;
			}
			}else{
				bgImage.color = new Color32(0,0,0,255);
				bgImage.sprite = null;
			}
 	}


	public void toggleCameraOnOffBtn(){
		//Toggle camera on/ off	
			if(SplitCamera.Instance.isDeviceConnected()){
				SplitCamera.Instance.stopPreview();
			}else{
				SplitCamera.Instance.startPreview();
			}
		
	}

	private void updatePreviewLabelTxt(bool isStart){
		//update preview label
			if(isStart){
				// txtPreviewLabel.text = "StartPreview";
			}else{
				// txtPreviewLabel.text = "StopPreview";
			}
	}





}
