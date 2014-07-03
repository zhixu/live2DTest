using UnityEngine;
using System.Collections;

//This is a basic controller. It imports a song, passes it on to the analyzer and starts playing it.
//See ExampleController and the example scene for an example on how to configure the analyzer and get the data in a game.
public class BasicController : MonoBehaviour
{	
	/// <summary>
	/// RhythmTool. Configure it in the inspector.
	/// </summary>
	public RhythmTool rhythmTool;
			
	// Use this for initialization
	void Start ()
	{		
		//Initialize RythmTool.
		rhythmTool.Init(this,false);
		
		//Load a song.
		string songPath=Util.OpenFileDialog();
		rhythmTool.NewSong (songPath);
		
		//Start playing the song
		rhythmTool.Play ();		
	}
	

	
	// Update is called once per frame
	void Update ()
	{						
		//Update RhythmTool. 
		rhythmTool.Update ();
		rhythmTool.DrawDebugLines();
	}
				

}
