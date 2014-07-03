using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
/// <summary>
/// An Analysis object analyzes a song based on it's spectral magnitude at different moments in time.
/// An Analysis object can be configured to analyze certain frequency ranges.
/// The results are stored in in array of frames.
/// </summary>
public class Analysis
{	
	/// <summary>
	/// Name for this analysis. Optional, but useful if you're using multiple analyses.
	/// E.g. bass, mid, high, or frequency range.
	/// </summary>
	public string name;
	/// <summary>
	/// The threshold multiplier for finetuning the peak-picking threshold.
	/// </summary>
	private float thresholdMultiplier = 1.5f;	
	/// <summary>
	/// The size of the threshold window. How much of the neighboring data is taken into account
	/// when calculating the peak-picking threshold.
	/// </summary>
	private int thresholdWindowSize = 10;	
	/// <summary>
	/// The frames that contain all results.
	/// </summary>
	private Frame[] frames;
	public Frame[] Frames
	{
		get{return frames;}
	}
	
	public int start;
	public int end;
	
	/// <summary>
	/// AnimationCurve representing how much each frequency is multiplied when advanced analysis is enabled.
	/// </summary>
	public AnimationCurve weightCurve;

	/// <summary>
	/// The total number of frames in the song.
	/// </summary>	
	private int totalFrames;
	
	/// <summary>
	/// If set to true, additional options become available in the inspector.
	/// </summary>
	public bool advancedAnalysis = false;
		
	/// <summary>
	/// Initialize the analysis.
	/// </summary>
	/// <param name='totalFrames'>
	/// Total frames the analysis should have.
	/// </param>
	public void Init (int totalFrames, bool advancedAnalysis)
	{
		this.totalFrames = totalFrames;
		this.advancedAnalysis=advancedAnalysis;		
		
		frames = new Frame[totalFrames];
		
		int spectrumSize = (RhythmTool.fftWindowSize-2)/2;
		if (end < start || start < 0 || end < 0 || start >= spectrumSize || end > spectrumSize)
				Debug.LogError("Invalid range for analysis " + name +". Range must be within Spectrum (fftWindowSize/2 - 1) and start cannot come after end.");
	}
	
	/// <summary>
	/// Analyze the specified spectrum based on frequency ranges and previous spectra.
	/// </summary>
	/// <param name='spectrum'>
	/// A spectrum.
	/// </param>
	/// <param name='index'>
	/// The index of this spectrum.
	/// </param>
	public void Analyze (float[] spectrum, int index)
	{
		//Required offset for each different step in the analysis.
		int offset = 0;
		frames[index].magnitude = Sum (spectrum, start, end);		
		frames[index].magnitudeSmooth = frames[index].magnitude;
		
		offset = Mathf.Max (index - 10, 0);
		frames[offset].flux = frames[offset].magnitude - frames[Mathf.Max (offset - 1, 0)].magnitude;
		Smooth(offset,10);
		
		offset = Mathf.Max (index - 20, 0);
		frames[offset].threshold = Threshold (frames, offset, thresholdMultiplier, thresholdWindowSize);
		Smooth(offset,10);
		
		offset = Mathf.Max (index - 30, 0);		
		if ( frames[offset].flux>  frames[offset].threshold) {
			if ( frames[offset].flux >  frames[Mathf.Min (offset + 1, frames.Length - 1)].flux && frames [offset].flux > frames [Mathf.Max (offset - 1, 0)].flux) {
				frames [offset].onset = frames [offset].flux;
			}
		}
		
		offset = Mathf.Max (index - 100, 0);
		Rank(offset,50);
	}
	
	/// <summary>
	/// Draws the different results in a graph.
	/// </summary>
	/// <param name='index'>
	/// The index from where to start.
	/// </param>
	/// <param name='h'>
	/// The 
	/// </param>
	public void DrawDebugLines (int index, int h)
	{
		for (int i = 0; i<299; i++) {
			if(i+1+index>totalFrames-1)
				break;
			Vector3 s = new Vector3 (i, frames[i + index].magnitude + h * 100, 0);
			Vector3 e = new Vector3 (i + 1, frames[i + 1 + index].magnitude + h * 100, 0);		
			Debug.DrawLine (s, e, Color.red);
			
			s = new Vector3 (i, frames[i + index].magnitudeSmooth + h * 100, 0);
			e = new Vector3 (i + 1, frames[i + 1 + index].magnitudeSmooth + h * 100, 0);		
			Debug.DrawLine (s, e, Color.red);
			
			s = new Vector3 (i, frames [i + index].flux + h * 100, 0);
			e = new Vector3 (i + 1, frames [i + 1 + index].flux + h * 100, 0);		
			Debug.DrawLine (s, e, Color.blue);	
			
			s = new Vector3 (i, frames [i + index].threshold + h * 100, 0);
			e = new Vector3 (i + 1, frames [i + 1 + index].threshold + h * 100, 0);		
			Debug.DrawLine (s, e, Color.blue);	
			
			s = new Vector3 (i, frames [i + index].onset + h * 100, 0);
			e = new Vector3 (i + 1, frames [i + 1 + index].onset + h * 100, 0);		
			Debug.DrawLine (s, e, Color.yellow);
			
			s = new Vector3 (i, -frames [i + index].onsetRank + h * 100, 0);
			e = new Vector3 (i + 1, -frames [i + 1 + index].onsetRank + h * 100, 0);		
			Debug.DrawLine (s, e, Color.white);
		}
	}
	
	
	/// <summary>
	/// Calculates the onset threshold for an index in an array.
	/// It looks at the surrounding indices in the array.
	/// </summary>
	/// <param name='input'>
	/// Input array.
	/// </param>
	/// <param name='index'>
	/// Index.
	/// </param>
	/// <param name='multiplier'>
	/// Multiplier that multiplies the result. Useful for finetuning.
	/// </param>
	/// <param name='windowSize'>
	/// Size of window around the index that is taken into account.
	/// </param>
	private float Threshold (Frame[] input, int index, float multiplier, int windowSize)
	{
		int start = Mathf.Max (0, index - windowSize);
		int end = Mathf.Min (input.Length - 1, index + windowSize);
		float mean = 0;
		for (int j = start; j <= end; j++)
			mean += Mathf.Abs (input[j].flux);
		mean /= (end - start);
		return (mean * multiplier);	
	}
	
	/// <summary>
	/// Sums a section of an array
	/// </summary>
	/// <returns>
	/// The sum of a section of an array.
	/// </returns>
	/// <param name='input'>
	/// Input array.
	/// </param>
	/// <param name='start'>
	/// Index of the start of the section.
	/// </param>
	/// <param name='end'>
	/// Index of the end of the section.
	/// </param>
	/// </param>
	private float Sum (float[] input, int start, int end)
	{
		float output = 0;
		
		for (int i = start; i< end; i++) {
			
			if(advancedAnalysis)
				output += input [i]*weightCurve.Evaluate(i);
			else
				output += input [i];
		}
		
		return output;
	}
	
	/// <summary>
	/// Returns the average of a section in an array.
	/// </summary>
	/// <returns>
	/// The average.
	/// </returns>
	/// <param name='input'>
	/// Input array.
	/// </param>
	/// <param name='start'>
	/// Index of the start of the section.
	/// </param>
	/// <param name='end'>
	/// Index of the end of the section.
	/// </param>
	private float Average (float[] input, int start, int end)
	{
		float output = 0;
		
		for (int i = start; i< end; i++) {
			output += input [i];
		}
		output /= (end - start);
		
		return output;
	}
	
	private void Smooth(int index, int windowSize)
	{
		float average=0;
		for(int i = index-(windowSize/2); i<index+(windowSize/2); i++)
		{
			if(i>0 && i < totalFrames)
				average+=frames[i].magnitudeSmooth;
		}
		
		frames[index].magnitudeSmooth=average/windowSize;
	}	
	
	private void Rank(int index, int windowSize)
	{
		if(frames[index].onset==0)
			return;
		
		List<Frame> ignore = new List<Frame>();
		
		for(int i = 0; i<5; i++)
		{
			int p = 0;
			int n = 0;
			
			for(int ii = index-(windowSize/2); ii<index-1; ii++)
			{
				if(ii>0 && ii<totalFrames)
				{
					if(frames[ii].onset>0 && !ignore.Contains(frames[ii]))
					{
						p=ii;
					}
				}
			}
			
			for(int ii = index+1; ii<index+(windowSize/2); ii++)
			{
				if(ii>0 && ii<totalFrames)
				{
					if(frames[ii].onset>0 && !ignore.Contains(frames[ii]))
					{
						n=ii;
						break;
					}
				}
			}
			
			if(frames[index].onset>frames[p].onset && frames[index].onset>frames[n].onset)
			{
				frames[index].onsetRank=5-i;
				return;
			}
			else
			{
				if(frames[index].onset<frames[p].onset)
					ignore.Add(frames[p]);
				
				if(frames[index].onset<frames[n].onset)
					ignore.Add(frames[n]);
			}
			
			if(p==0 && n==0)
			{
				frames[index].onsetRank=5;
			}
			
		}
		
	}
}

public class AnalysisListAttribute : PropertyAttribute {
}
