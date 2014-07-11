using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text ;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using live2d;


public class LAppModel : BaseModelUnity
{
	private LAppView			view;
	
	
	public  TextAsset 			modelJson;// model file (.model.json)
	private ModelSetting 		modelSetting = null ;	
	
	protected L2DMatrix44		live2DMatrix ;
	
	private Material			mtHitArea;				
	
	
	private AudioSource 		asVoice;
	
	System.Random 				rand = new System.Random();
	
	private Bounds				bounds	;

    private AudioSource source;

	// parameters needed for this game specifically
	private bool isTrackingMouse = true;
	private float headTapTime = 0.0f;
    private float mouthTapTime = 0.0f;
	
	public override void initModel()
	{
		base.initModel ();

		if(isInitialized()) return;
		asVoice = this.gameObject.AddComponent<AudioSource>();
		asVoice.playOnAwake = false;
		
		bounds = this.GetComponent<MeshFilter>().mesh.bounds;
		
		view = new LAppView( this, this.transform);
		view.startAccel();
		
		if(modelJson != null) load(null);
		
		if(LAppDefine.DEBUG_LOG)mainMotionManager.setMotionDebugMode(true);

		if(LAppDefine.DEBUG_DRAW_HIT_AREA )
		{
	        createHitAreaMaterial();
		}
	}
	
    void Start() {
        source = Camera.main.GetComponent<AudioSource>();
    }
	
	void OnRenderObject()
	{
		if(live2DModel==null)return;
		if(live2DModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW)
		{
			draw();
		}
	}
	
	
	void Update()
	{
		if(isInitialized() && !isUpdating())
		{
			
			foreach(Touch t in Input.touches)
		    {
				Vector3 point = new Vector3(t.position.x, t.position.y, 0) ;
				if(t.phase == TouchPhase.Began)
				{
					view.OnMouseDown_exe( point, Input.touches.Length ) ;
				}
				else if(t.phase == TouchPhase.Moved)
				{
					view.OnMouseDrag_exe( point, Input.touches.Length ) ;
				}
				else if(t.phase == TouchPhase.Ended)
				{
					view.OnMouseUp_exe( point ) ;
				}
		    }

		    if (isTrackingMouse) {
		    	try {
		    		view.OnMouseDrag_exe(Input.mousePosition, Input.touches.Length) ;
		    	} catch (IOException e) {
		    		Debug.Log(e.StackTrace);
		    	}
		    }

			updateModel();
			if(live2DModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH)
			{
				draw();
			}
		}
	}
	
	
	public void load(string modelSettingPath)
	{
		updating = true;
		initialized = false;
		
		if(modelJson == null && modelSettingPath != null)
		{
			if(LAppDefine.DEBUG_LOG) Debug.Log("Load json : "+modelSettingPath);
			try{
				TextAsset ta = ( Resources.Load( modelSettingPath , typeof(TextAsset) ) as TextAsset ) ;
				modelSetting = new ModelSettingJson( ta ) ;
			}
			catch (IOException e)
			{
				Debug.Log(e.StackTrace);
				
				
				throw new Exception();
			}
		}
		else
		{
			if(LAppDefine.DEBUG_LOG) Debug.Log("Load json");
			modelSetting = new ModelSettingJson( modelJson ) ;
		}
		
		if( LAppDefine.DEBUG_LOG ) Debug.Log( "Start to load model" ) ;
		
		
		
		loadModelData ( modelSetting.getModelFile() , modelSetting.getTextureFiles() ) ;
		
		
		loadExpressions( modelSetting.getExpressionNames() , modelSetting.getExpressionFiles() );
		
		
		loadPhysics( modelSetting.getPhysicsFile() ) ;
		
		
		loadPose( modelSetting.getPoseFile() );
		
		
		Dictionary<string, float> layout = new Dictionary<string, float>();
		if (modelSetting.getLayout(layout) )
		{
			if (layout.ContainsKey("width"))	modelMatrix.setWidth(layout["width"]);
			if (layout.ContainsKey("height"))	modelMatrix.setHeight(layout["height"]);
			if (layout.ContainsKey("x"))		modelMatrix.setX(layout["x"]);
			if (layout.ContainsKey("y"))		modelMatrix.setY(layout["y"]);
			if (layout.ContainsKey("center_x"))	modelMatrix.centerX(layout["center_x"]);
			if (layout.ContainsKey("center_y"))	modelMatrix.centerY(layout["center_y"]);
			if (layout.ContainsKey("top"))		modelMatrix.top(layout["top"]);
			if (layout.ContainsKey("bottom"))	modelMatrix.bottom(layout["bottom"]);
			if (layout.ContainsKey("left"))		modelMatrix.left(layout["left"]);
			if (layout.ContainsKey("right"))	modelMatrix.right(layout["right"]);      
		}

		
		
		for(int i=0; i<modelSetting.getInitParamNum() ;i++)
		{
			string id = modelSetting.getInitParamID(i);
			float value = modelSetting.getInitParamValue(i);
			live2DModel.setParamFloat(id, value);
		}
		
		for(int i=0; i<modelSetting.getInitPartsVisibleNum() ;i++)
		{
			string id = modelSetting.getInitPartsVisibleID(i);
			float value = modelSetting.getInitPartsVisibleValue(i);
			live2DModel.setPartsOpacity(id, value);
		}

		
		eyeBlink=new L2DEyeBlink();
		
		
		view.setupView(
			live2DModel.getCanvasWidth(),
			live2DModel.getCanvasHeight());
		

		updating=false;
		initialized=true;
	}


    public void loadModelData(string modelFile, string[] texFiles)
    {
        if (modelFile == null || texFiles == null) return;

        try
        {
            if (LAppDefine.DEBUG_LOG) Debug.Log("Load model : " + modelFile);

            
            TextAsset modelMoc = FileManager.loadTextAsset(modelFile);
            live2DModel = Live2DModelUnity.loadModel(modelMoc.bytes);

            
            for (int i = 0; i < texFiles.Length; i++)
            {
                var texPath = texFiles[i];
                if (LAppDefine.DEBUG_LOG) Debug.Log("Load texture " + texPath);
                texPath = Regex.Replace(texPath, ".png$", "");//不要な拡張子を削除

                Texture2D texture = (Texture2D)Resources.Load(texPath, typeof(Texture2D));

                live2DModel.setTexture(i, texture);
            }
        }
        catch (IOException e)
        {
            Debug.Log(e.StackTrace);

            throw new Exception();
        }

        modelMatrix = new L2DModelMatrix(live2DModel.getCanvasWidth(), live2DModel.getCanvasHeight());
        modelMatrix.setWidth(2);
		modelMatrix.multScale(1,1,-1);
        modelMatrix.setCenterPosition(0, 0);
    }
	
	
	public void loadExpressions(string[] names, string[] fileNames)
	{
		if(fileNames==null || fileNames.Length==0)return;
		expressions = new Dictionary<string, AMotion>();
		try
		{
			for (int i = 0; i < fileNames.Length; i++)
			{
				if(LAppDefine.DEBUG_LOG)Debug.Log("Load expression : "+fileNames[i]);
				
				TextAsset ta = (TextAsset)( FileManager.open( fileNames[i] ) as TextAsset );
				expressions.Add(names[i], L2DExpressionMotion.loadJson(ta.bytes));
			}
		}
		catch (IOException e)
		{
			Debug.Log( e.StackTrace );
		}
	}
	
	
	public void loadPose( string fileName )
	{
		if(fileName==null)return;
		if(LAppDefine.DEBUG_LOG) Debug.Log( "Load json : " + fileName ) ;
		try
		{
			TextAsset ta = (TextAsset)( FileManager.open( fileName ) as TextAsset );
			byte[] bytes = ta.bytes ;
			char[] buf = System.Text.Encoding.GetEncoding( "UTF-8" ).GetString( bytes ).ToCharArray();
			pose = L2DPose.load( buf ) ;
		}
		catch (IOException e)
		{
			Debug.Log( e.StackTrace );
		}
		
	}
	
	
	public void loadPhysics( string fileName )
	{
		if(fileName==null)return;
		if(LAppDefine.DEBUG_LOG) Debug.Log( "Load json : " + fileName ) ;
		try
		{
			TextAsset ta = (TextAsset)( FileManager.open( fileName ) as TextAsset );
			byte[] bytes = ta.bytes ;
			char[] buf = System.Text.Encoding.GetEncoding( "UTF-8" ).GetString( bytes ).ToCharArray();
			physics = L2DPhysics.load( buf );
		}
		catch (IOException e)
		{
			Debug.Log( e.StackTrace );
		}
	}
	
	
	
    void updateModel()
	{
		view.update(Input.acceleration);
		if(live2DModel == null)
		{
			if(LAppDefine.DEBUG_LOG)Debug.Log("Can not update there is no model data");
			return;
		}

		long timeMSec = UtSystem.getUserTimeMSec() - startTimeMSec ;
		double timeSec = timeMSec / 1000.0 ;
		double t = timeSec * 2 * Math.PI  ;
		
		
		if(mainMotionManager.isFinished())
		{
			
			//startRandomMotion(LAppDefine.MOTION_GROUP_IDLE, LAppDefine.PRIORITY_IDLE);
			turnOnMouseTracking();
		}
		//-----------------------------------------------------------------
		live2DModel.loadParam();

		bool update = mainMotionManager.updateParam(live2DModel);

		if( ! update)
		{
			
			eyeBlink.updateParam(live2DModel);
		}

		live2DModel.saveParam();
		//-----------------------------------------------------------------

		if(expressionManager!=null)expressionManager.updateParam(live2DModel);
		

		
		
		live2DModel.addToParamFloat( PARAM_ANGLE_X, dragX *  30 , 1 );
		live2DModel.addToParamFloat( PARAM_ANGLE_Y, dragY *  30 , 1 );
		live2DModel.addToParamFloat( PARAM_ANGLE_Z, (dragX*dragY) * -30 , 1 );

		
		live2DModel.addToParamFloat( PARAM_BODY_X    , dragX  , 10 );

		
		live2DModel.addToParamFloat( PARAM_EYE_BALL_X, dragX  , 1 );
		live2DModel.addToParamFloat( PARAM_EYE_BALL_Y, dragY  , 1 );
		
		
		live2DModel.addToParamFloat(PARAM_ANGLE_X,	(float) (15 * Math.Sin( t/ 6.5345 )) , 0.5f);
		live2DModel.addToParamFloat(PARAM_ANGLE_Y,	(float) ( 8 * Math.Sin( t/ 3.5345 )) , 0.5f);
		live2DModel.addToParamFloat(PARAM_ANGLE_Z,	(float) (10 * Math.Sin( t/ 5.5345 )) , 0.5f);
		live2DModel.addToParamFloat(PARAM_BODY_X,	(float) ( 4 * Math.Sin( t/15.5345 )) , 0.5f);
		live2DModel.setParamFloat(PARAM_BREATH,		(float) (0.5f + 0.5f * Math.Sin( t/3.2345 )),1);
		
		
		
		live2DModel.addToParamFloat(PARAM_ANGLE_X, 90 * accelX ,0.5f);
		live2DModel.addToParamFloat(PARAM_ANGLE_Z, 10 * accelX ,0.5f);
		
		
		if(physics!=null)physics.updateParam(live2DModel);

		
		if(lipSync)
		{
			live2DModel.setParamFloat(PARAM_MOUTH_OPEN_Y, lipSyncValue ,0.8f);
		}

		
		if(pose!=null)pose.updateParam(live2DModel);

        if(source.isPlaying) live2DModel.setParamFloat(PARAM_MOUTH_OPEN_Y, source.volume);
		
		live2DModel.update();
	}
	
	
	
	public void draw()
	{
        Matrix4x4 planeLocalToWorld = this.transform.localToWorldMatrix;

		
        Matrix4x4 rotateModelOnToPlane = Matrix4x4.identity;
        rotateModelOnToPlane.SetTRS(Vector3.zero, Quaternion.Euler(90, 0, 0), Vector3.one);
		    
		Matrix4x4 scale2x2ToPlane = Matrix4x4.identity;
		
		Vector3 scale = new Vector3( bounds.size.x / 2.0f , 1 , bounds.size.z / 2.0f ) ;
		scale2x2ToPlane.SetTRS( Vector3.zero , Quaternion.identity , scale ) ;
		
		
        Matrix4x4 modelMatrix4x4 = Matrix4x4.identity;
		float[] matrix = modelMatrix.getArray();
        for (int i = 0; i < 16; i++)
        {
            modelMatrix4x4[i] = matrix[i];
        }

        Matrix4x4 modelCanvasToWorld = planeLocalToWorld * scale2x2ToPlane * rotateModelOnToPlane * modelMatrix4x4;

        live2DModel.setMatrix(modelCanvasToWorld);
		
		live2DModel.draw();

		if(LAppDefine.DEBUG_DRAW_HIT_AREA )
		{
			
			drawHitArea(modelCanvasToWorld);
		}
	}
	
	
	
	public void drawHitArea(Matrix4x4 m3)
	{
        mtHitArea.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(m3);
        
        GL.Begin(GL.LINES);
        GL.Color(new Color(1,0,0,0.5f));
		
		int len = modelSetting.getHitAreasNum();
		for (int i=0 ; i<len ; i++ )
		{
			string	drawID		= modelSetting.getHitAreaID(i) ;
			int		drawIndex	= live2DModel.getDrawDataIndex(drawID) ;
			if( drawIndex<0 ) continue ;
			float[]	points		= live2DModel.getTransformedPoints(drawIndex) ;
			float	left		= live2DModel.getCanvasWidth() ;
			float	right		= 0 ;
			float	top			= live2DModel.getCanvasHeight() ;
			float	bottom		= 0 ;

			for (int j = 0; j < points.Length; j=j+2)
			{
				float x = points[j] ;
				float y = points[j+1] ;
				if(	x < left	) left		= x ; 
				if(	x > right	) right		= x ; 
				if(	y < top		) top		= y ; 
				if(	y > bottom	) bottom	= y ; 
			}
			
			GL.Vertex(new Vector3(left, top, 0));
			GL.Vertex(new Vector3(right, top, 0));
			
			GL.Vertex(new Vector3(right, top, 0));
			GL.Vertex(new Vector3(right, bottom, 0));
			
			GL.Vertex(new Vector3(right, bottom, 0));
			GL.Vertex(new Vector3(left, bottom, 0));
			
			GL.Vertex(new Vector3(left, bottom, 0));
			GL.Vertex(new Vector3(left, top, 0));
		}
		
        GL.End();
        GL.PopMatrix();
	}

	// picks a random idle motion to play
	public void startIdleMotion() {
		int max = modelSetting.getMotionNum(LAppDefine.MOTION_GROUP_IDLE);
		int no = (int)(rand.NextDouble() * max);
		startMotion(LAppDefine.MOTION_GROUP_IDLE, no, LAppDefine.PRIORITY_IDLE);
	}
	
	
	public void startRandomMotion(string name ,int priority)
	{
		int max = modelSetting.getMotionNum( name );
		int no = (int)(rand.NextDouble() * max ) ;
		startMotion( name, no, priority ) ;
	}
	
	
	
	public void startMotion(string name, int no,int priority)
	{
        Debug.Log("starting motion name: " + name);

		string motionName = modelSetting.getMotionFile( name, no ) ;

		if( motionName==null || motionName.Equals( "" ) )
		{
			if(LAppDefine.DEBUG_LOG) Debug.Log("Motion name is invalid");
			return;//
		}
		
		//overwrites the force priority
		/*if (priority == LAppDefine.PRIORITY_FORCE) {
	    	mainMotionManager.setReservePriority(priority);
	   	}
		else*/ if( ! mainMotionManager.reserveMotion( priority ) )
		{
			Debug.Log("Do not play because book already playing, or playing a motion already");

			if(LAppDefine.DEBUG_LOG){ Debug.Log("Do not play because book already playing, or playing a motion already");}
			return ;


		}

		AMotion motion = FileManager.loadAssetsMotion( motionName ) ;

		if( motion == null )
		{
			Debug.Log( "Failed to read the motion." ) ;
			mainMotionManager.setReservePriority( 0 ) ;
			return;
		}
		
		motion.setFadeIn(modelSetting.getMotionFadeIn(name, no));
		motion.setFadeOut(modelSetting.getMotionFadeOut(name, no));

		
		if( ( modelSetting.getMotionSound(name, no)) == null)
		{
			
			if(LAppDefine.DEBUG_LOG) Debug.Log("Start motion : "+motionName);
			mainMotionManager.startMotionPrio(motion,priority);
		}
		else
		{
			
			string soundPath=modelSetting.getMotionSound( name, no ) ;
            soundPath = Regex.Replace(soundPath,".mp3$","");//不要な拡張子を削除
            
			AudioClip acVoice=FileManager.loadAssetsSound( soundPath ) ;
			if(LAppDefine.DEBUG_LOG) Debug.Log("Start motion : "+motionName+"  voice : "+soundPath);
			startVoiceMotion( motion,acVoice,priority);
		}
	}
	
	
	
	public void startVoiceMotion(AMotion motion, AudioClip acVoice, int priority)
	{
		mainMotionManager.startMotionPrio(motion,priority);
		asVoice.clip     = acVoice ;
		asVoice.loop     = false ;
		asVoice.panLevel = 0 ;
		asVoice.Play();
	}

	
	/**
	 * 表情を設定する
	 * @param motion
	 */
	public void setExpression(string name)
	{
		if( ! expressions.ContainsKey( name ) )return;
		if( LAppDefine.DEBUG_LOG ) Debug.Log( "Setting expression : " + name ) ;
		AMotion motion=expressions[ name ] ;
		expressionManager.startMotion( motion , false ) ;
	}
	
	
	
	public void setRandomExpression()
	{
		int no=(int)(rand.NextDouble() * expressions.Count);

		string[] keys = new string[expressions.Count];
		expressions.Keys.CopyTo(keys,0);

		setExpression(keys[no]);
	}
	
	
	
	public bool hitTest(string id,float testX,float testY)
	{
		if(modelSetting==null)return false;
		int len=modelSetting.getHitAreasNum();
		for (int i = 0; i < len; i++)
		{
			if( id.Equals(modelSetting.getHitAreaName(i)) )
			{
				string drawID=modelSetting.getHitAreaID(i);
				int drawIndex=live2DModel.getDrawDataIndex(drawID);
				if(drawIndex<0)return false;
				float[] points=live2DModel.getTransformedPoints(drawIndex);

				float left=live2DModel.getCanvasWidth();
				float right=0;
				float top=live2DModel.getCanvasHeight();
				float bottom=0;

				for (int j = 0; j < points.Length; j=j+2)
				{
					float x = points[j];
					float y = points[j+1];
					if(x<left)left=x;	// minimum x
					if(x>right)right=x;	// maximum x
					if(y<top)top=y;		// minimum y
					if(y>bottom)bottom=y;// maximum y
				}

                float tx = testX;
                float ty = testY;
				
				return ( left <= tx && tx <= right && top <= ty && ty <= bottom ) ;
			}
		}
		return false;
	}
	
	
	
	public void flickEvent(float x,float y)
	{
		if(LAppDefine.DEBUG_LOG) Debug.Log("flick x:"+x+" y:"+y);

		if(hitTest(LAppDefine.HIT_AREA_HEAD, x, y))
		{
            Debug.Log("face flicked");
			if(LAppDefine.DEBUG_LOG) Debug.Log("Flick face");
			//startRandomMotion(LAppDefine.MOTION_GROUP_FLICK_HEAD, LAppDefine.PRIORITY_NORMAL );
		}
	}

	
	void createHitAreaMaterial()
	{
		 mtHitArea = new Material( 
			"Shader \"Lines/HitArea\" {" +
            "SubShader {" +
            "    Pass { " +
            "       Blend SrcAlpha OneMinusSrcAlpha" +
            "       Cull Off" +
            "       ZWrite Off" + 
            "       ZTest Less" +
            "       Fog { Mode Off }" +
            "       BindChannels {" +
            "           Bind \"Vertex\", vertex" + 
            "           Bind \"Color\", color" +
            "       }" +
            "} } }" );
        mtHitArea.hideFlags = HideFlags.HideAndDontSave;
        mtHitArea.shader.hideFlags = HideFlags.HideAndDontSave;
	}
	
	
	//========= TOUCH event =========
	void OnMouseDown(){


		
		if(isInitialized() && !isUpdating())
		{
			view.OnMouseDown_exe(Input.mousePosition, Input.touches.Length) ;

		}
		
	}
	
	void OnMouseUp(){
		if(isInitialized() && !isUpdating())
		{
			view.OnMouseUp_exe(Input.mousePosition) ;
		}
	}

	void OnMouseDrag() {
		if(isInitialized() && !isUpdating())
		{
			view.OnMouseDrag_exe(Input.mousePosition, Input.touches.Length) ;
		}
	}
	

	
	public bool tapEvent(float x,float y)
	{
		if(LAppDefine.DEBUG_LOG) Debug.Log("tapEvent view x:"+x+" y:"+y);

			if(hitTest( LAppDefine.HIT_AREA_HEAD,x, y ))
			{
				Debug.Log ("tapped head");
				if(LAppDefine.DEBUG_LOG) Debug.Log("Tapped face");
				startMotion("head pat", 0, LAppDefine.PRIORITY_NORMAL);
				if (headTapTime != 0.0f) {
					if (Time.time - headTapTime <= 1.0f) {
						Camera.main.GetComponent<GUIAnswers>().eliminateAnswer ();
					}
				}
				headTapTime = Time.time;

			}
			else if(hitTest( LAppDefine.HIT_AREA_BODY,x, y))
			{
				if(LAppDefine.DEBUG_LOG) Debug.Log("Tapped body");
				//startRandomMotion(LAppDefine.MOTION_GROUP_TAP_BODY, LAppDefine.PRIORITY_FORCE );
			}
			else if(hitTest( LAppDefine.HIT_AREA_MOUTH, x, y)) {
				//HTTP REQUEST
                
                if (mouthTapTime != 0.0f) {
                    if(Time.time - mouthTapTime <= 1.0f) {

        				string var = Camera.main.GetComponent<GameController>().getCurrentPronunciation();
                        if (PlayerPrefs.HasKey("isFlipCard")) {
                            if (! bool.Parse (PlayerPrefs.GetString("isFlipCard"))) getAudio(var);
                        } else {
                            getAudio(var);
                        }
                    }
                }
                mouthTapTime = Time.time;          

			}
		return true;
	}
    
    void getAudio(string src) {
        
        string url = "http://api.voicerss.org/?";
        string key = "3b0ecbdfa24345269d712e94f34a9615";
        string hl = "ja-jp";
        string r = "-3";
        string c = "WAV";
        string f = "48khz_16bit_stereo";

        if (SystemInfo.deviceType == DeviceType.Desktop) c = "OGG";
        
        url = url + "key=" + key + "&src=" + src + "&hl=" + hl + "&r=" + r + "&c=" + c + "&f=" + f;

		Debug.Log (url);
        
        WWW www = new WWW(url);
        StartCoroutine(waitForRequest(www));
    }
    
    IEnumerator waitForRequest(WWW www) {
        
        yield return www;
        
        if (www.error == null) {

            AudioClip audio = new AudioClip();

            if (SystemInfo.deviceType == DeviceType.Handheld) audio = www.GetAudioClip(true, false, AudioType.WAV);
            if (SystemInfo.deviceType == DeviceType.Desktop) audio = www.GetAudioClip(true, false, AudioType.OGGVORBIS);

            StartCoroutine(playClip(audio));

        } else {

            Debug.Log(www.error);

        }
    }
	
	IEnumerator playClip(AudioClip audio) {
        
        source.clip = audio;
        source.Play();
        lipSync = true;
        yield return new WaitForSeconds(source.clip.length-1);
        lipSync = false;
        source.Stop();
    
    }
	
	public void shakeEvent()
	{
		if(LAppDefine.DEBUG_LOG)Debug.Log("Shake Event");
		
		//startRandomMotion(LAppDefine.MOTION_GROUP_SHAKE,LAppDefine.PRIORITY_FORCE );
	}
	
	
	public Bounds getBounds()
	{
		return bounds ;
	}

	public void turnOnMouseTracking() {
		isTrackingMouse = true;
	}

	public void turnOffMouseTracking() {
		isTrackingMouse = false;
	}

	public void resetMotionDirection() {
	}

}