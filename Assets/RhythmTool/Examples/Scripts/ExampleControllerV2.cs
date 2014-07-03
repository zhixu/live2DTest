using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//In this "game", the controller lets the player choose a song.
//RythmTool has been configured in the inspector, to analyze three frequency ranges, named "Low", "Mid" and "High".
//Then the data from the analyses is used to create some simple visuals.
public class ExampleControllerV2 : MonoBehaviour
{
	/// <summary>
	/// prefab that represents a line.
	/// </summary>
	public GameObject linePrefab;
	
	/// <summary>
	/// RythmTool. Configure it in the inspector.
	/// </summary>
	public RhythmTool rhythmTool;	
	/// <summary>
	/// Is this controller initialized?
	/// </summary>
	private bool initialized = false;
	
	//Variables for example "game".
	private int lastFrame;
	private List<Line> lines;
	
	private Frame[] low;
	private Frame[] mid;
	private Frame[] high;
			
	// Use this for initialization
	void Start ()
	{		
		Application.runInBackground=true;
		
		//Initialize RhythmTool. We're not going to pre calculate the data.
		rhythmTool.Init(this,false);
		//Open a new song.
		initialized = NewSong();
	}
	
	/// <summary>
	/// Load and play a song,(re)initialize variables and RhythmTool.
	/// </summary>
	/// <returns>
	/// boolean indicating if succeeded. 
	/// </returns>
	private bool NewSong()
	{		
		//stop playing and alanyzing the current song.
		rhythmTool.Stop();
		
		//open an openfiledialog. If nothing was selected, do nothing.
		string songPath = Util.OpenFileDialog();
		if(songPath=="")
			return false;
		//Give the path to RhythmTool. If something went wrong, do nothing.
		if(!rhythmTool.NewSong(songPath))
			return false;
		//Start playing and analyzing the song.
		rhythmTool.Play();
		
		//Game related initializatoin.
		//get the data from the analyzer. 
		//"Low", "Mid" and "High" have been configured in the inspector, but are also the names for the default analyses.				
		low=rhythmTool.GetResults("Low");
		mid=rhythmTool.GetResults("Mid");
		high=rhythmTool.GetResults("High");
		
		lines = new List<Line> ();
		lastFrame = 0;	
	
		return true;
	}
	
	// Update is called once per frame
	void Update ()
	{		
		//Hit space to load another song.
		if (Input.GetKey (KeyCode.Space)) {
			ClearLines();
			initialized = NewSong();
		}		
		//Hit esc to quit.
		if (Input.GetKey (KeyCode.Escape)) {
			UnityEngine.Application.Quit ();
		}		
		//If not initialized for some reason (no song loaded), don't do anything.
		if (!initialized){						
			return;
		}
				
		//Update RhythmTool and draw debug lines. 
		rhythmTool.Update ();
		rhythmTool.DrawDebugLines();
		//Game logic
		//create some visual lines to represent some of the analyzed data.
		UpdateLines ();			
		//If the song has ended, reset.
		if (rhythmTool.CurrentFrame == rhythmTool.TotalFrames - 1)
			initialized = NewSong ();
	}
	
	/// <summary>
	/// Updates the lines. Creates new lines, destroys old ones and sets their positions.
	/// </summary>
	private void UpdateLines ()
	{		
		//Create new lines when necessary.
		//This loop makes sure we're not missing any frames, which would happen when
		//the game is running at frame rates lower or near the frame rate of the analysis (~30).
		for (int i = lastFrame+1; i<rhythmTool.CurrentFrame+100; i++) {
			
			if (i > rhythmTool.TotalFrames - 1)
				break;
			
			//if there is an onset, create a new line representing this onset.
			if(low[i].onset > .3f )
			{
				lines.Add (CreateLine (i, Color.blue, low[i].onset));
			}
			
			if(mid[i].onset > .3f)
			{
				lines.Add (CreateLine (i, Color.green, mid[i].onset*.7f));
			}
			
			if(high[i].onset > .1f)
			{
				lines.Add (CreateLine (i, Color.yellow, high[i].onset));
			}
			
			lastFrame = i;
		}
				
		List<Line> toRemove = new List<Line> ();
		
		//Move the lines according to the current frame.
		//Delete lines when their frame has passed.
		foreach (Line l in lines) {
			
			//We're going to move the lines based on the frame corresponding to the current time in the song.
			//The lines should be moving towards a point, and reach that point when their time in the song has come.
			//To do this, we simply subtract the current frame number, from the index of the line. 
			//subtracting the interpolation variable means that we're essentially interpolating between the position for this frame, and the next.
			Vector3 position = new Vector3 ((l.Index - rhythmTool.CurrentFrame) - rhythmTool.Interpolation, -10, 0);
			l.LineObject.transform.position = position;
			
			//Remove this line if it's time has come.
			if (rhythmTool.CurrentFrame >= l.Index)
				toRemove.Add (l);
		}
		foreach (Line l in toRemove) {
			lines.Remove (l);
			GameObject.Destroy (l.LineObject);
		}
	}
	
	/// <summary>
	/// Clears the remaining lines.
	/// </summary>
	private void ClearLines()
	{
		if(lines==null) return;
		
		List<Line> toRemove = new List<Line> ();
		
		foreach (Line l in lines) {
				toRemove.Add (l);
		}
		foreach (Line l in toRemove) {
			lines.Remove (l);
			GameObject.Destroy (l.LineObject);
		}
	}
	
	/// <summary>
	/// Creates a line.
	/// </summary>
	/// <returns>
	/// Line containing a GameObject and an Index.
	/// </returns>
	/// <param name='index'>
	/// Index of the onset to which this line belongs.
	/// </param>
	/// <param name='c'>
	/// Color.
	/// </param>
	/// <param name='s'>
	/// Scale and brightness.
	/// </param>
	public Line CreateLine (int index, Color c, float s)
	{
		GameObject lineObject = (GameObject)GameObject.Instantiate (linePrefab, new Vector3 (100, 0, 0), Quaternion.identity);
		lineObject.transform.localScale = new Vector3 (.5f, s, .5f);
		MeshRenderer meshRenderer = (MeshRenderer)lineObject.GetComponent ("MeshRenderer");
		c = Color.Lerp (Color.black, c, s * .01f);
		meshRenderer.material.SetColor ("_TintColor", c);
		
		Line line = new Line(lineObject,index);		
	
		return line;
	}	
}
