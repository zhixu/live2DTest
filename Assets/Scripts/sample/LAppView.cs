using UnityEngine;
using System;
using System.Collections;
using live2d;



public class LAppView 
{
	private LAppModel			model;
	private L2DMatrix44 		deviceToScreen;
	private L2DViewMatrix		viewMatrix;//
	private AccelHelper 		accelHelper;
	private TouchManager 		touchMgr;
	private L2DTargetPoint 		dragMgr;
	
	private Transform			transform;
	
	private bool				isMove = false;
	private float	 			lastX , lastY ;
	
	protected float canvasWidth = -1 ;
	protected float canvasHeight= -1 ;
	
	
	protected Vector2 touchPos_onPlane ;//touch pos on plane , LeftTop(0,0) RightBottom(1,1)
	protected Vector2 touchPos_onModelCanvas01 ;
	protected Vector2 touchPos_onModelCanvas ;
	
	//------------------------------------
	//Unity 'Plane' object layout
	
	private Vector3 localP_LT ;//o
	private Vector3 localP_RT ;//x
	private Vector3 localP_LB ;//y

	
	Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
	
	
	public LAppView( LAppModel model, Transform tr )
	{
		this.model = model ;
		transform    = tr;
		
		
		deviceToScreen=new L2DMatrix44();

		
		viewMatrix=new L2DViewMatrix();

		
		viewMatrix.setMaxScale( LAppDefine.VIEW_MAX_SCALE );
		viewMatrix.setMinScale( LAppDefine.VIEW_MIN_SCALE );

		viewMatrix.setMaxScreenRect(
				LAppDefine.VIEW_LOGICAL_MAX_LEFT,
				LAppDefine.VIEW_LOGICAL_MAX_RIGHT,
				LAppDefine.VIEW_LOGICAL_MAX_BOTTOM,
				LAppDefine.VIEW_LOGICAL_MAX_TOP
				);
		
		
		touchMgr = new TouchManager();
		
		dragMgr = new L2DTargetPoint();
		
		Bounds bounds = model.getBounds();
		localP_LT = new Vector3(-(bounds.size.x/2), 0, (bounds.size.z/2)) ;
		localP_RT = new Vector3( (bounds.size.x/2), 0, (bounds.size.z/2)) ;
		localP_LB = new Vector3(-(bounds.size.x/2), 0,-(bounds.size.z/2)) ;
	}


	public void startAccel()
	{
		
		accelHelper = new AccelHelper() ;
	}
	
	
	
	public void OnMouseDown_exe(Vector3 inputPos, int touchCount)
	{
		if(LAppDefine.DEBUG_LOG) Debug.Log("touchesBegan(Device)"+" x:"+inputPos.x +" y:"+inputPos.y);
		
		if(camera.isOrthoGraphic == false)
		{ 
			
			updateTouchPos_3DCamera(inputPos) ;
		}
		else
		{ 
			
			updateTouchPos_2DCamera(inputPos) ;
		}
		
		touchesBegan( touchPos_onModelCanvas.x , touchPos_onModelCanvas.y ) ;
	}
	
	
	public void OnMouseDrag_exe(Vector3 inputPos, int touchCount ) 
	{
		if(LAppDefine.DEBUG_LOG) Debug.Log("touchesMoved(Device)"+" x:"+inputPos.x +" y:"+inputPos.y);
		
		if(camera.isOrthoGraphic == false)
		{
			
			updateTouchPos_3DCamera(inputPos) ;
		}
		else
		{ 
			
			updateTouchPos_2DCamera(inputPos) ;
		}
		touchesMoved( touchPos_onModelCanvas.x , touchPos_onModelCanvas.y ) ;
	}
	
	
	public void OnMouseUp_exe(Vector3 inputPos)
	{
		if(LAppDefine.DEBUG_LOG) Debug.Log("touchesEnded");
		if(camera.isOrthoGraphic == false)
		{ 
			
			updateTouchPos_3DCamera(inputPos) ;
		}
		else
		{ 
			
			updateTouchPos_2DCamera(inputPos) ;
		}
		touchesEnded( ) ;
	}

	
	public void setupView( float width, float height )
	{
		
		float ratio	 = height/width;
		float left	 = LAppDefine.VIEW_LOGICAL_LEFT;
		float right	 = LAppDefine.VIEW_LOGICAL_RIGHT;
		float bottom = -ratio;
		float top	 = ratio;

		viewMatrix.setScreenRect(left,right,bottom,top);

		float screenW=Math.Abs(left-right);
		deviceToScreen.identity();
		deviceToScreen.multTranslate(-width/2.0f,height/2.0f );
		deviceToScreen.multScale( screenW/width , screenW/width );
		
		canvasWidth  = width;
		canvasHeight = height;
	}
	
	
	public void update(Vector3 acceleration)
	{
		dragMgr.update();
		model.setDrag(dragMgr.getX(), dragMgr.getY());
		
		accelHelper.setCurAccel( acceleration ) ;
		
		accelHelper.update();

		if( accelHelper.getShake() > 1.5f )
		{
			if(LAppDefine.DEBUG_LOG) Debug.Log("shake event");
			
			model.shakeEvent() ;
			accelHelper.resetShake() ;
		}

		model.setAccel(accelHelper.getAccelX(), accelHelper.getAccelY(), accelHelper.getAccelZ());
	}


	private float transformDeviceToViewX(float deviceX)
	{
		float screenX = deviceToScreen.transformX( deviceX );
		return  viewMatrix.invertTransformX(screenX);
	}


	private float transformDeviceToViewY(float deviceY)
	{
		float screenY = deviceToScreen.transformY( deviceY );
		return  viewMatrix.invertTransformY(screenY);
	}
	
	
	
	public void touchesBegan(float p1x,float p1y)
	{
		if(LAppDefine.DEBUG_LOG) Debug.Log("touchesBegan(Local)"+" x:"+p1x+" y:"+p1y);
		touchMgr.touchBegan(p1x,p1y);

		float x=transformDeviceToViewX( touchMgr.getX() );
		float y=transformDeviceToViewY( touchMgr.getY() );

		dragMgr.Set(x, y);
		
		lastX = p1x;
		lastY = p1y;
	}


	
	public void touchesMoved(float p1x,float p1y)
	{
		if(LAppDefine.DEBUG_LOG)Debug.Log("touchesMoved(Local)"+"x:"+p1x+" y:"+p1y);
		touchMgr.touchesMoved(p1x,p1y);
		float x=transformDeviceToViewX( touchMgr.getX() );
		float y=transformDeviceToViewY( touchMgr.getY() );
		
		dragMgr.Set(x, y);

		const int FLICK_DISTANCE=100;
		
		

		if(touchMgr.isSingleTouch() && touchMgr.isFlickAvailable() )
		{
			float flickDist=touchMgr.getFlickDistance();
			if(flickDist>FLICK_DISTANCE)
			{
				model.flickEvent(touchPos_onModelCanvas.x, touchPos_onModelCanvas.y);
				touchMgr.disableFlick();
			}
		}
		
		if(lastX != p1x && lastY != p1y)
		{
			isMove = true;
		}
	}
	

	
	public void touchesEnded()
	{
		if(LAppDefine.DEBUG_LOG)Debug.Log("touchesEnded");
		dragMgr.Set(0,0);
		
		if(!isMove )
		{
			model.tapEvent(touchPos_onModelCanvas.x, touchPos_onModelCanvas.y);
		}
		else
		{
			isMove = false ;
		}
	}
	

	
	protected void updateTouchPos_3DCamera(Vector3 inputPos)
	{
		//--- calc local plane coord ---
		{
			Ray ray = Camera.main.ScreenPointToRay(inputPos);
			
			Vector3 worldP_LT = transform.TransformPoint( localP_LT ) ;
			Vector3 worldP_RT = transform.TransformPoint( localP_RT ) ;
			Vector3 worldP_LB = transform.TransformPoint( localP_LB ) ;
			
			Vector3 PO = worldP_LT ;
			Vector3 VX = worldP_RT - worldP_LT ;
			Vector3 VY = worldP_LB - worldP_LT ;
			
			Vector3 PL = ray.origin ;
			Vector3 VL = ray.direction ;
			
			float Dx = PO.x - PL.x ;
			float Dy = PO.y - PL.y ;
			float Dz = PO.z - PL.z ;
			
			float E = (VX.x*VL.y - VX.y*VL.x) ;
			float F = (VY.x*VL.y - VY.y*VL.x) ;
			float G = (Dx*VL.y - Dy*VL.x) ;
			
			float H = (VX.x*VL.z - VX.z*VL.x) ;
			float I = (VY.x*VL.z - VY.z*VL.x) ;
			float J = (Dx*VL.z - Dz*VL.x) ;
			
			float tmp = ( F*H - E*I ) ;

			if( tmp == 0 )
			{
				//not update value
			}
			else
			{
				touchPos_onPlane.x = ( G*I - F*J ) / tmp ;
				touchPos_onPlane.y = ( E*J - G*H ) / tmp ;
			}
		}
		
		//--- calc touchPos on the model canvas
		{
			float touchPos_plane2x2_X =  touchPos_onPlane.x*2 - 1;
			float touchPos_plane2x2_Y = -touchPos_onPlane.y*2 + 1;
			
			L2DModelMatrix m = model.getModelMatrix() ;
			touchPos_onModelCanvas.x = m.invertTransformX(touchPos_plane2x2_X) ;
			touchPos_onModelCanvas.y = m.invertTransformY(touchPos_plane2x2_Y) ;
		}
	}

	
	
	protected void updateTouchPos_2DCamera(Vector3 inputPos)
	{
		//--- calc local plane coord ---
		{
			Vector3 worldP_LT = transform.TransformPoint( localP_LT ) ;
			Vector3 worldP_RT = transform.TransformPoint( localP_RT ) ;
			Vector3 worldP_LB = transform.TransformPoint( localP_LB ) ;
			
			Vector3 ScreenLT = Camera.main.WorldToScreenPoint( worldP_LT ) ;
			Vector3 ScreenRT = Camera.main.WorldToScreenPoint( worldP_RT ) ;
			Vector3 ScreenLB = Camera.main.WorldToScreenPoint( worldP_LB ) ;
			
			Vector3 sVX = ScreenRT - ScreenLT;
			Vector3 sVY = ScreenLB - ScreenLT;
			
			
			
			float x = inputPos.x;
			float y = inputPos.y;
			
			float vax = sVX.x;
			float vay = sVX.y;
			
			float vbx = sVY.x;
			float vby = sVY.y;
			
			float p0x = ScreenLT.x;
			float p0y = ScreenLT.y;
			
			float f = vbx*vay-vby*vax;
			float g = p0y*vax-p0x*vay-y*vax+x*vay;
			
			if(f == 0 || vax == 0)
			{
				//not update value
			}
			else
			{
				float t = g/f;
				float k = (x-p0x-t*vbx)/vax;
				touchPos_onPlane.x = k;
				touchPos_onPlane.y = t;
			}
		}
		
		//--- calc touchPos on the model canvas
		{
			
			float touchPos_plane2x2_X =  touchPos_onPlane.x*2 - 1;
			float touchPos_plane2x2_Y = -touchPos_onPlane.y*2 + 1;
			
			L2DModelMatrix m = model.getModelMatrix() ;
			touchPos_onModelCanvas.x = m.invertTransformX(touchPos_plane2x2_X) ;
			touchPos_onModelCanvas.y = m.invertTransformY(touchPos_plane2x2_Y) ;
		}
	}
}