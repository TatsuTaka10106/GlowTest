using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MADGazeHandGestureCamera : MGCameraManager
{
	public Image ImagePreview;
	private Texture2D mTexture2D;

	private Color32 defaultColor = new Color32(0,0,0,255);
	private Color32 previewColor = new Color32(255,255,225,255);

	void Start()
	{
		initCallback();
		mTexture2D = new Texture2D(640, 480, TextureFormat.ARGB32, false);
	}
	void Update(){
		if(showCameraPreview && SplitCamera.Instance.isDeviceConnected() && mTexture2D != null && ImagePreview != null){
			lock(mTexture2D) {
					byte[] data = MADHandGesture.Instance.getPreviewResult();
					if(data != null) {
						mTexture2D.LoadImage(data);
						Sprite sprite = Sprite.Create (mTexture2D, new Rect(0,0,mTexture2D.width,mTexture2D.height), new Vector2(.5f,.5f));
						ImagePreview.sprite = sprite;
						ImagePreview.color = previewColor;
					} else {
						ImagePreview.color = defaultColor;
						ImagePreview.sprite = null;
					}
			}
		} else{
			if(ImagePreview!=null){
				ImagePreview.color = defaultColor;
			}
		}
	}

	void OnDestroy()
	{
		mTexture2D = null;
		ImagePreview = null;
	}

	
}
