using UnityEngine;
using System.Collections;

public class LAppDefine : MonoBehaviour 
{
	
	public static bool DEBUG_LOG=false;
	public static bool DEBUG_DRAW_HIT_AREA=false;
	
	
	
	public const float VIEW_MAX_SCALE = 2f;
	public const float VIEW_MIN_SCALE = 0.8f;
	
	public const float VIEW_LOGICAL_LEFT = -1;
	public const float VIEW_LOGICAL_RIGHT = 1;
	
	public const float VIEW_LOGICAL_MAX_LEFT = -2;
	public const float VIEW_LOGICAL_MAX_RIGHT = 2;
	public const float VIEW_LOGICAL_MAX_BOTTOM = -2;
	public const float VIEW_LOGICAL_MAX_TOP = 2;
	
	public const float SCREEN_WIDTH = 20.0f;
	public const float SCREEN_HEIGHT = 20.0f;
	
	
	public const string BACK_IMAGE_NAME		= "image/back_class_normal.png" ;
	
	
	public const string MODEL_HARU			= "Live2D/haru/haru.model.json";
	public const string MODEL_HARU_A		= "Live2D/haru/haru_01.model.json";
	public const string MODEL_HARU_B		= "Live2D/haru/haru_02.model.json";
	public const string MODEL_SHIZUKU		= "Live2D/shizuku/shizuku.model.json";
	public const string MODEL_WANKO	 		= "Live2D/wanko/wanko.model.json";
	
	
	public const string MOTION_GROUP_IDLE			="idle";		
	public const string MOTION_GROUP_TAP_BODY		="tap_body";	
	public const string MOTION_GROUP_FLICK_HEAD		="flick_head";	
	public const string MOTION_GROUP_PINCH_IN		="pinch_in";	
	public const string MOTION_GROUP_PINCH_OUT		="pinch_out";	
	public const string MOTION_GROUP_SHAKE			="shake";		
	
	
	public const string HIT_AREA_HEAD				="head";
	public const string HIT_AREA_BODY				="body";
	public const string HIT_AREA_MOUTH 				= "mouth";

	
	public const int PRIORITY_NONE			= 0;
	public const int PRIORITY_IDLE			= 1;
	public const int PRIORITY_NORMAL		= 2;
	public const int PRIORITY_FORCE			= 3;
}