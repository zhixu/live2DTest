using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using live2d;

public class BaseModelUnity : MonoBehaviour 
{
	
	protected const string PARAM_ANGLE_X="PARAM_ANGLE_X";
	protected const string PARAM_ANGLE_Y="PARAM_ANGLE_Y";
	protected const string PARAM_ANGLE_Z="PARAM_ANGLE_Z";
	protected const string PARAM_BODY_X="PARAM_BODY_ANGLE_X";
	protected const string PARAM_BREATH="PARAM_BREATH";
	protected const string PARAM_EYE_BALL_X="PARAM_EYE_BALL_X";
	protected const string PARAM_EYE_BALL_Y="PARAM_EYE_BALL_Y";
	protected const string PARAM_MOUTH_OPEN_Y="PARAM_MOUTH_OPEN_Y";

	// model-related
	protected Live2DModelUnity 		live2DModel=null;			
	protected L2DModelMatrix		modelMatrix=null;			

	// motioin, state management
	protected Dictionary<string,AMotion> 	expressions ;	
	protected Dictionary<string,AMotion> 	motions ;		

	protected L2DMotionManager 		mainMotionManager;		
	protected L2DMotionManager 		expressionManager;		
	protected L2DEyeBlink 			eyeBlink;				
	protected L2DPhysics 			physics;				
	protected L2DPose 				pose;					

	protected bool 					initialized = false;	
	protected bool 					updating = false;		
	protected bool 					lipSync = false;		
	protected float 				lipSyncValue;			

	
	protected float 				accelX=0;
	protected float 				accelY=0;
	protected float 				accelZ=0;

	
	protected float 				dragX=0;
	protected float 				dragY=0;

	protected long 					startTimeMSec;
	
	
	void Awake()
	{
		initModel ();
	}

	public virtual void initModel ()
	{
		
		mainMotionManager = new L2DMotionManager();
		expressionManager = new L2DMotionManager();
		
		motions = new Dictionary<string, AMotion>();
		expressions = new Dictionary<string, AMotion>();
	}
	
	
	public L2DModelMatrix getModelMatrix()
	{
		return modelMatrix;
	}
	
	
	
	public bool isInitialized() 
	{
		return initialized;
	}


	public void setInitialized(bool v)
	{
		initialized = v ;
	}
	
	
	
	public bool isUpdating() {
		return updating;
	}


	public void setUpdating(bool v)
	{
		updating=v;
	}


	/**
	 * Live2Dモデルクラスを取得する。
	 * @return
	 */
	public Live2DModelUnity getLive2DModel() {
		return live2DModel;
	}


	public void setLipSync(bool v)
	{
		lipSync=v;
	}


	public void setLipSyncValue(float v)
	{
		lipSyncValue=v;
	}


	public void setAccel(float x,float y,float z)
	{
		accelX=x;
		accelY=y;
		accelZ=z;
	}


	public void setDrag(float x,float y)
	{
		dragX=x;
		dragY=y;
	}


	public MotionQueueManager getMainMotionManager()
	{
		return mainMotionManager;
	}


	public MotionQueueManager getExpressionManager()
	{
		return expressionManager;
	}
}