using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//The data from the analyses is used to create some simple visuals.
public class ExampleController : MonoBehaviour
{
	
	/// <summary>
	/// RythmTool. Configure it in the inspector.
	/// </summary>
	public RhythmTool rhythmTool;	
	
	/// <summary>
	/// Frames from "Low" analysis.
	/// </summary>
	private Frame[] low;
			
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
		
		
		//get the data from the analyzer. 
		//"Low", is the name for one of the default analyses.				
		low=rhythmTool.GetResults("Low");
	}
		
	// Update is called once per frame
	void Update ()
	{						
		//Update RhythmTool and draw debug lines. 
		rhythmTool.Update ();
		rhythmTool.DrawDebugLines();
		
		//Game logic
		//Look at 100 frames and draw a line if there is an onset.
		for(int i = 0; i<100; i++)
		{
			//the index of the frame we are going to check and possibly draw a line for
			int frameIndex = Mathf.Min(i+rhythmTool.CurrentFrame, rhythmTool.TotalFrames);
			
			//the onset value for this frame.
			float onset = low[frameIndex].onset;
			
			if(onset>0)
			{
				//horizontal position of the line.
				float x = i-rhythmTool.Interpolation;
				
				//create two vectors for drawing a line. 
				//The magnitude of the onset is used as the length of the line.
				Vector3 l1 = new Vector3(x,-20,0);
				Vector3 l2 = new Vector3(x,onset-20,0);
				Debug.DrawLine(l1,l2,Color.red);
			}			
		}
		
		//Draw one line at the start, so we can see where it is.
		Debug.DrawLine(new Vector3(-1,-20,0),new Vector3(-1,-10,0),Color.red);
	}
	
	
}
