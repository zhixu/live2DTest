using UnityEngine ;
using System ;
using System.Collections ;
using System.Collections.Generic ;
using live2d ;

public class ModelSettingJson : ModelSetting 
{
	private Value json ;
	private const string NAME				= "name";
	private const string ID					= "id";
	private const string MODEL				= "model";
	private const string TEXTURES			= "textures";
	private const string HIT_AREAS			= "hit_areas";
	private const string PHYSICS			= "physics";
	private const string POSE				= "pose";
	private const string EXPRESSIONS		= "expressions";
	private const string MOTION_GROUPS		= "motions";
	private const string SOUND				= "sound";
	private const string FADE_IN			= "fade_in";
	private const string FADE_OUT			= "fade_out";
	
	private const string VALUE				= "val";
	private const string FILE				= "file";
	private const string INIT_PARTS_VISIBLE	= "init_parts_visible";
	private const string INIT_PARAM			= "init_param";
	private const string LAYOUT				= "layout";
	
	
	
	public ModelSettingJson( TextAsset ta )
	{
		char[] buf = ta.text.ToCharArray() ;
		json = Json.parseFromBytes( buf ) ;
	}
	
	public bool existMotion(string name) {return json.get(MOTION_GROUPS).getMap(null).ContainsKey(name);}//json.motion_group[name]
	
	public bool existMotionSound(string name,int n)	{return json.get(MOTION_GROUPS).get(name).get(n).getMap(null).ContainsKey(SOUND);}
	
	public bool existMotionFadeIn(string name,int n){return json.get(MOTION_GROUPS).get(name).get(n).getMap(null).ContainsKey(FADE_IN);}
	public bool existMotionFadeOut(string name,int n){return json.get(MOTION_GROUPS).get(name).get(n).getMap(null).ContainsKey(FADE_OUT);}
	
	
	public string getModelName()
	{
		if( !json.getMap(null).ContainsKey(NAME) )return null;
		return json.get(NAME).toString();
	}
	
	
	public string getModelFile()
	{
		if( !json.getMap(null).ContainsKey(MODEL) )return null;
		return json.get(MODEL).toString();
	}
	
	
	public int getTextureNum()
	{
		if( !json.getMap(null).ContainsKey(TEXTURES) )return 0;
		return json.get(TEXTURES).getVector(null).Count ; // json.textures.length
	}
	
	
	public string getTextureFile(int n)
	{
		return json.get(TEXTURES).get(n).toString();//json.textures[n]
	}


	public int getHitAreasNum()
	{
		if( !json.getMap(null).ContainsKey(HIT_AREAS) )return 0;
		return json.get(HIT_AREAS).getVector(null).Count ; // json.hit_area.length
	}


	public string getHitAreaID(int n)
	{
		return json.get(HIT_AREAS).get(n).get(ID).toString();//json.hit_area[n].id
	}


	public string getHitAreaName(int n)
	{
		return json.get(HIT_AREAS).get(n).get(NAME).toString();//json.hit_area[n].name
	}


	public string getPhysicsFile()
	{
		if( !json.getMap(null).ContainsKey(PHYSICS) )return null;
		return json.get(PHYSICS).toString();
	}


	public string getPoseFile()
	{
		if( !json.getMap(null).ContainsKey(POSE) )return null;
		return json.get(POSE).toString();
	}


	public int getMotionNum(string name)
	{
		if( ! existMotion(name))return 0;
		return json.get(MOTION_GROUPS).get(name).getVector(null).Count;//json.motion_group[name].length
	}


	public string getMotionFile(string name,int n)
	{
		if( ! existMotion(name))return null;
		return json.get(MOTION_GROUPS).get(name).get(n).get(FILE).toString();//json.motion_group[name][n].file
	}


	public string getMotionSound(string name,int n)
	{
		if( ! existMotionSound(name,n))return null;	
		return json.get(MOTION_GROUPS).get(name).get(n).get(SOUND).toString();//json.motion_group[name][n].sound
	}


	public int getMotionFadeIn(string name,int n)
	{
		return (! existMotionFadeIn(name,n))? 1000 :  json.get(MOTION_GROUPS).get(name).get(n).get(FADE_IN).toInt();//json.motion_group[name][n].fade_in
	}


	public int getMotionFadeOut(string name,int n)
	{
		return (! existMotionFadeOut(name,n))? 1000 :json.get(MOTION_GROUPS).get(name).get(n).get(FADE_OUT).toInt();//json.motion_group[name][n].fade_out
	}

	
	public string[] getMotionGroupNames()
	{
		if( !json.getMap(null).ContainsKey(MOTION_GROUPS) )return null;
		System.Object[] keys = json.get(MOTION_GROUPS).keySet().ToArray();

		if( keys.Length == 0 )return null;

		string[] names = new string[keys.Length];

		for (int i = 0; i < names.Length; i++)
		{
			names[i] = (string)keys[i];
		}
		return  names;
	}
	
	
	
	public bool getLayout(Dictionary<string, float> layout)
	{
		if(!json.getMap(null).ContainsKey(LAYOUT))return false;

		Dictionary<string,Value> map = json.get(LAYOUT).getMap(null);
		string[] keys = new string[map.Count] ;
		map.Keys.CopyTo(keys,0);

		for(int i=0;i<keys.Length;i++)
		{
			layout.Add(keys[i], json.get(LAYOUT).get(keys[i]).toFloat() );
		}
		return true;
	}
	
	
	
	public int getInitParamNum()
	{
		if(!json.getMap(null).ContainsKey(INIT_PARAM))return 0;
        return json.get(INIT_PARAM).getVector(null).Count;
	}


	public float getInitParamValue(int n)
	{
		return json.get(INIT_PARAM).get(n).get(VALUE).toFloat();
	}


	public string getInitParamID(int n)
	{
		return json.get(INIT_PARAM).get(n).get(ID).toString();
	}

	
	
	public int getInitPartsVisibleNum()
	{
		if(!json.getMap(null).ContainsKey(INIT_PARTS_VISIBLE) )return 0;
        return json.get(INIT_PARTS_VISIBLE).getVector(null).Count;
	}


	public float getInitPartsVisibleValue(int n)
	{
		return json.get(INIT_PARTS_VISIBLE).get(n).get(VALUE).toFloat();
	}


	public string getInitPartsVisibleID(int n)
	{
		return json.get(INIT_PARTS_VISIBLE).get(n).get(ID).toString();
	}


	public int getExpressionNum()
	{
		if(!json.getMap(null).ContainsKey(EXPRESSIONS))return 0;
		return json.get(EXPRESSIONS).getVector(null).Count;
	}


	public string getExpressionFile(int n)
	{
		return json.get(EXPRESSIONS).get(n).get(FILE).toString();
	}


	public string getExpressionName(int n)
	{
		return json.get(EXPRESSIONS).get(n).get(NAME).toString();
	}


	public string[] getTextureFiles() 
	{
		string[] ret=new string[getTextureNum()];
		for (int i = 0; i < ret.Length; i++)
		{
			ret[i] = getTextureFile(i);
		}
		return ret;
	}


	public string[] getExpressionFiles() 
	{
		string[] ret=new string[getExpressionNum()];
		for (int i = 0; i < ret.Length; i++)
		{
			ret[i] = getExpressionFile(i);
		}
		return ret;
	}


	public string[] getExpressionNames() 
	{
		string[] ret=new string[getExpressionNum()];
		for (int i = 0; i < ret.Length; i++)
		{
			ret[i] = getExpressionName(i);
		}
		return ret;
	}
	
}