using UnityEngine;
using System.Collections;
using System.IO;
using live2d;

public class FileManager  {

	public static bool exists_resource( string path )
	{
		try 
		{
			Resources.Load( path ) ;
			return true ;
		} catch  {
			return false ;
		}
	}


	public static Object open( string path ) 
	{
		return Resources.Load( path ) ;
	}

	public static TextAsset loadTextAsset( string path ){
		return (TextAsset)Resources.Load( path , typeof(TextAsset) ) ;
	}
	

	public static Texture2D loadTexture2D( string path ){
		return (Texture2D)Resources.Load( path , typeof(Texture2D) ) ;
	}
	
	public static Live2DMotion loadAssetsMotion(string path)
	{
		Live2DMotion motion=null;
		if(LAppDefine.DEBUG_LOG) Debug.Log( "load motion : "+path);

		try
		{
			TextAsset ta = (TextAsset)( FileManager.open( path ) as TextAsset ) ;
			byte[] buf = ta.bytes ;
			motion = Live2DMotion.loadMotion( buf ) ;
		}
		catch(IOException e)
		{
			Debug.Log( e.StackTrace );
		}
		return motion;
	}
	
	
	
	static public AudioClip loadAssetsSound(string filename)
	{
		if(LAppDefine.DEBUG_LOG) Debug.Log( "Load voice : "+filename);

		AudioClip player = new AudioClip() ;

		try
		{
			player = (AudioClip)(FileManager.open( filename )) as AudioClip;
			
		}
		catch (IOException e)
		{
			Debug.Log( e.StackTrace );
		}

		return player;
	}
}