using UnityEngine;
using System;
using System.Collections.Generic;
using live2d;


public class LAppLive2DManager : MonoBehaviour
{
	
	private List<GameObject> canvases ;
	
	
	private int			modelCount = -1 ;
	
	Vector2 startPos;
	Vector2 endPos;
		
	
	//=========================================================
	void Awake()
	{
		Live2D.init();
		
		canvases = new List<GameObject>
		{
			GameObject.Find("Live2D_Canvas_Haru") ,
			GameObject.Find("Live2D_Canvas_Shizuku") ,
			GameObject.Find("Live2D_Canvas_Wanko") ,
			GameObject.Find("Live2D_Canvas_HaruA") ,
			GameObject.Find("Live2D_Canvas_HaruB") ,
		};
		
		for(int i = 0; i < canvases.Count; i++){canvases[i].SetActive(false);}
		
		
		if(!GameObject.Find("Main Camera").GetComponent<Camera>().isOrthoGraphic)
		{
			Debug.Log("\"Main Camera\" Projection : Perspective");
		}
	}
	
	
	void Start()
	{
		changeModel();
	}
	
	
	
	public LAppModel getLAppModel(int no)
	{
		if(no>=canvases.Count)return null;
		return canvases[no].GetComponent<LAppModel>();
	}


	public int getModelNum()
	{
		int modelNum = 0;
		for(int i = 0; i < canvases.Count;i++)
		{
			if(canvases[i].activeSelf) modelNum++;
		}
		return modelNum;
	}
	
	
	
	public void changeModel()
	{
		
		for( int i = 0; i < canvases.Count; i++){canvases[i].SetActive(false);}
		modelCount++;
		
		int no = modelCount % 4 ;
		
		try
		{
			switch(no)
			{
			case 0: 
				canvases[0].SetActive(true);
				
				if(canvases[0].GetComponent<LAppModel>().modelJson == null)
				{
					canvases[0].GetComponent<LAppModel>().load( LAppDefine.MODEL_HARU);
				}
				break ;
				
			case 1: 
				canvases[1].SetActive(true);
				
				if(canvases[1].GetComponent<LAppModel>().modelJson == null)
				{
					canvases[1].GetComponent<LAppModel>().load( LAppDefine.MODEL_SHIZUKU);
				}
				break;
				
			case 2: 
				canvases[2].SetActive(true);
				
				if(canvases[2].GetComponent<LAppModel>().modelJson == null)
				{
					canvases[2].GetComponent<LAppModel>().load( LAppDefine.MODEL_WANKO);
				}
				break;
				
			case 3: 
				canvases[3].SetActive(true);
				canvases[4].SetActive(true);
				
				if(canvases[3].GetComponent<LAppModel>().modelJson == null)
				{
					canvases[3].GetComponent<LAppModel>().load( LAppDefine.MODEL_HARU_A);
				}
				
				if(canvases[4].GetComponent<LAppModel>().modelJson == null)
				{
					canvases[4].GetComponent<LAppModel>().load( LAppDefine.MODEL_HARU_B);
				}
				break;
			}
		}
		catch (Exception e) 
		{
			Debug.Log(e.StackTrace);
			Debug.Log("Failed to load model. Exit the application.");
			Application.Quit();
		}
	}
}