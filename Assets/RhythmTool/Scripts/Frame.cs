using UnityEngine;
using System.Collections;

/// <summary>
/// A frame of data about the song.
/// </summary>
public struct Frame{
	public float magnitude;
	public float magnitudeSmooth;
	public float flux;
	public float threshold;
	public float onset;
	public int onsetRank;
}


//public class FrameAttribute : PropertyAttribute {
//}