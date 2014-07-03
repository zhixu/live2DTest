using UnityEngine;
using System.Collections;

[System.Serializable]
/// <summary>
/// RhythmTool.
/// It contains multiple analyses, which can/should be configured in the inspector.
/// It also keeps the time in order to be able to synchronize the data with the song.
/// </summary>
public class RhythmTool
{	
	private AudioSource audioSource;
	/// <summary>
	/// Number of samples that fft is to be performed on. Must be a power of 2. 4096 seems to be ideal.
	/// </summary>
	public const int fftWindowSize = 4096;	
	/// <summary>
	/// How much the fft window moves for each frame. 1500 seems to be ideal.
	/// Making it too small will lower the difference between two frames, making peak picking difficult.
	/// Making it too big will decrease the number of frames per second, making the timing too coarse.
	/// </summary>
	public const int frameSpacing = 1500;
	
	/// <summary>
	/// The total number of frames.
	/// </summary>
	[SerializeField]
	private int totalFrames;
	public int TotalFrames
	{
		get{return totalFrames;}
	}

	/// <summary>
	/// The frame corresponding with the current time of the song.
	/// Important for synchronizing the data with the song.
	/// </summary>
	[SerializeField]
	private int currentFrame;
	public int CurrentFrame
	{
		get{return currentFrame;}
	}
	
	/// <summary>
	/// Because the framerate of the analysis usually does not line up with the game's framerate,
	/// it is useful to have a variable that can be used to interpolate between two frames.
	/// </summary>
	[SerializeField]
	private float interpolation;
	public float Interpolation
	{
		get{return interpolation;}
	}
	
	/// <summary>
	/// The last frame that has been analyzed.
	/// </summary>
	[SerializeField]
	private int lastFrame = 0;
	public int LastFrame
	{
		get{return lastFrame;}
	}
	
	/// <summary>
	/// How much of a lead (in frames) the analysis has on the song. 
	/// Can be changed at runtime. See "preCalculate" for pre calculating all data.
	/// </summary>
	[SerializeField]
	private int lead = 300;
	public int Lead
	{
		get{return lead;}
		set{lead = Mathf.Max(lead,40);}
	}
	
	/// <summary>
	/// The analysis is finished. Mostly useful if it's set to pre calulate;
	/// </summary>
	[SerializeField]
	private bool isDone = false;
	public bool IsDone
	{
		get{return isDone;}
	}
	
	[SerializeField]
	private bool advancedAnalyses = false;
	
	/// <summary>
	/// List of Analysis objects. It's easiest to configure these in the inspector.
	/// </summary>
	//public List<Analysis> analyses;
	[AnalysisList]
	public Analysis[] analyses;
	/// <summary>
	/// Set to "true" if you want the analyzer to pre calculate everything.
	/// It will do this in multiple frames, so it's progress can be displayed ingame if necessary.
	/// Pre calculation allows for additional analysis, but it takes some time.
	/// </summary>
	private bool preCalculate = false;
	/// <summary>
	/// Is the analyzer initialized?
	/// </summary>
	private bool initialized = false;
	public bool Initialized
	{
		get{return initialized;}
	}
	/// <summary>
	/// The script of which RhythmTool is an instance.
	/// </summary>
	private MonoBehaviour script;
	/// <summary>
	/// The total amount of samples of the music file.
	/// </summary>
	private int totalSamples;
	/// <summary>
	/// The the index of the current sample of the song.
	/// </summary>
	private int sampleIndex;
	
	/// <summary>
	/// Initialize RythmTool.
	/// </summary>
	/// <param name='script'>
	/// The script of which RhythmTool is an instance.
	/// </param>
	/// <param name='preCalculate'>
	/// Set to true to pre calculate the data. Set to false to calculate the data on the go.
	/// </param>
	public AudioSource Init(MonoBehaviour script, bool preCalculate)
	{
		this.script=script;
		this.preCalculate=preCalculate;
				
		audioSource = (AudioSource)script.gameObject.GetComponent (typeof(AudioSource));
		if (audioSource == null)
			audioSource = (AudioSource)script.gameObject.AddComponent ("AudioSource");
		
		if(!advancedAnalyses)
		{
			analyses = new Analysis[3];
			
			Analysis analysis = new Analysis();
			analysis.start=0;
			analysis.end=12;
			analysis.name="Low";
			analyses[0]=analysis;
			
			analysis = new Analysis();
			analysis.start=30;
			analysis.end=200;
			analysis.name="Mid";
			analyses[1]=analysis;
			
			analysis = new Analysis();
			analysis.start=300;
			analysis.end=550;
			analysis.name="High";
			analyses[2]=analysis;			
		}
		
		if(analyses.Length <=0){
			Debug.LogWarning("No analysis configured");
			return null;
		}
		
		initialized=true;
		
		return audioSource;
	}
		
	/// <summary>
	/// Imports a (new) song. (re) initializes analyses and variables.
	/// </summary>
	/// <returns>
	/// Boolean indicating if succeeded
	/// </returns>
	/// <param name='songPath'>
	/// File path of an MP3 file.
	/// </param>
	public bool NewSong (string songPath)
	{
		if(!initialized)
			return false;
		audioSource.Stop ();				
		audioSource.clip=Mp3Importer.Import(songPath);
		
		totalSamples = audioSource.clip.samples;
		totalFrames = totalSamples / frameSpacing;
				
		foreach (Analysis s in analyses) {
			s.Init (totalFrames, advancedAnalyses);
		}
				
		isDone=false;
		lastFrame=0;
		initialized = true;
		
		return true;
	}
	
	/// <summary>
	/// Use an AudioClip as the new song. (re) initializes analyses and variables.
	/// </summary>
	/// <returns>
	/// Boolean indicating if succeeded
	/// </returns>
	/// <param name='musicPath'>
	/// Audioclip of the song.
	/// </param>
	public bool NewSong(AudioClip audioClip)
	{
		if(!initialized)
			return false;
		audioSource.Stop ();				
		audioSource.clip=audioClip;
		
		totalSamples = audioSource.clip.samples;
		totalFrames = totalSamples / frameSpacing;
				
		foreach (Analysis s in analyses) {
			s.Init (totalFrames, advancedAnalyses);
		}
				
		isDone=false;
		lastFrame=0;
		initialized = true;
		
		return true;
	}
		
	public void Play()
	{
		if(audioSource!=null)
			audioSource.Play();
	}

	public void Pause()
	{
		if(audioSource!=null)
			audioSource.Pause();
	}
	
	public void Stop()
	{
		if(audioSource!=null)
			audioSource.Stop();
	}
	
	/// <summary>
	/// Gets called when the analysis is completed. 
	/// </summary>
	private void EndOfAnalysis ()
	{
		if (preCalculate) {
			//Do additional analysis here.
		}
		
		isDone = true;
	}
	
	/// <summary>
	/// Update RhythmTool. Even if the analysis is complete, 
	/// Update() still needs to be called in order to keep the time.
	/// </summary>
	public void Update ()
	{
		if (!initialized) {
			Debug.LogWarning ("RhythmTool not initialized");
			return;	
		}
					
		sampleIndex = audioSource.timeSamples;
		float timeFactor = (float)sampleIndex / (float)totalSamples;
		currentFrame = (int)(timeFactor * totalFrames);		
		
		interpolation = timeFactor * (float)totalFrames;		
		interpolation = interpolation - (float)currentFrame;
		
		if (isDone)
			return;
		
		if (preCalculate)
			lead += 500;
		
		lead = Mathf.Max (40, lead);
				
		for (int i = lastFrame+1; i<currentFrame+lead; i++) {
		
			if (i >= totalFrames) {
				EndOfAnalysis ();
				break;
			}
			
			//get samples from the audioclip and perfrom an FFT on them.
			//First get the samples.
			float[] floatSamples = new float[fftWindowSize];									
			audioSource.clip.GetData (floatSamples, i * frameSpacing);
			
			//Convert samples to doubles for fft.
			double[] doubleSamples = Util.FloatsToDoubles (floatSamples);
			
			//Perform FFT.
			LomontFFT fft = new LomontFFT ();
			fft.RealFFT (doubleSamples, true);
			
			//Calculate spectral magnitudes.
			float[] spectrum = Util.SpectrumMagnitude (doubleSamples);
			
			//The resulting spectrum is passed on to an analysis, 
			//where the specific data is generated and stored.
			foreach (Analysis s in analyses) {
				s.Analyze (spectrum, i);				
			}
			
			lastFrame = i;
		}						
	}
		
	/// <summary>
	/// Gets the results.
	/// </summary>
	/// <returns>
	/// Array of frames containing the results.
	/// </returns>
	/// <param name='name'>
	/// Name of the analysis you want to get the results from.
	/// </param>
	public Frame[] GetResults(string name)
	{
		foreach (Analysis a in analyses) {
			if (a.name == name)
				return a.Frames;
		}
		
		Debug.LogWarning("Analysis " + name + " not found." );
		return null;
	}
		
	/// <summary>
	/// Draws the debug lines.
	/// </summary>
	public void DrawDebugLines ()
	{
		float[] floatSamples = new float[fftWindowSize];			
		audioSource.clip.GetData (floatSamples, currentFrame * frameSpacing);
		double[] doubleSamples = Util.FloatsToDoubles (floatSamples);
		
		LomontFFT fft = new LomontFFT ();	
		fft.RealFFT (doubleSamples, true);					
		
		float[] magnitude = Util.SpectrumMagnitude (doubleSamples);
		
		for (int i = 0; i<magnitude.Length-1; i++) {						
			Vector3 sv = new Vector3 (i * -1, magnitude [i] * (1 + i * .05f), 0);
			Vector3 ev = new Vector3 ((i + 1) * -1, magnitude [i + 1] * (1 + i * .05f), 0);
			Debug.DrawLine (sv, ev, Color.green);
		}

		for(int i = 0; i < analyses.Length; i++)
		{
			analyses[i].DrawDebugLines(currentFrame, i);
		}
	}

}