using UnityEngine;
using System;
using System.Collections;

public class ButtonChangeModel : MonoBehaviour 
{
	private LAppLive2DManager live2DMgr ;
	
	void Awake()
	{
		int size = Screen.height / 14 ;
		Rect rctGUISize = new Rect(0,0,size, size);
		this.guiTexture.pixelInset = rctGUISize ;	
	}
	
	
	void Start() 
	{
		live2DMgr = GameObject.Find("Main Camera").GetComponent<LAppLive2DManager>() ;
	}
	
	void Update()
	{
		// Android、iOSでのモデル切り替え
		if (Application.platform == RuntimePlatform.IPhonePlayer ||
		    Application.platform == RuntimePlatform.Android)
		{
			foreach (Touch t in Input.touches) 
			{
				if (guiTexture.HitTest (t.position, Camera.main) && t.phase == TouchPhase.Began) 
				{
					live2DMgr.changeModel ();
				}
			}
		}
		
		// Androidのバックボタンで終了
		if(Application.platform == RuntimePlatform.Android)
		{
			if( Input.GetKey(KeyCode.Escape) )
			{
				Application.Quit();
				return;
			}
		}
	}
	
	void OnMouseDown() 
	{
		// Android、iOS以外のモデル切り替え
		if (Application.platform != RuntimePlatform.IPhonePlayer &&
		    Application.platform != RuntimePlatform.Android)
		{
			live2DMgr.changeModel ();
		}
	}
}