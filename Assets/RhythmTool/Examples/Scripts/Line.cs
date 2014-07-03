using UnityEngine;
using System.Collections;

/// <summary>
/// Simple class containing a GameObject and an index.
/// Used to synchronize a GameObject to a frame from RhythmTool.
/// </summary>
public class Line {
	
	private GameObject lineObject;
	public GameObject LineObject
	{
		get{return lineObject;}
		set{lineObject=value;}
	}
	private int index;
	public int Index
	{
		get{return index;}
		set{index=value;}
	}
	
	[SerializeField]
	public GameObject testPrefab;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Line"/> class.
	/// </summary>
	/// <param name='lineObject'>
	/// GameObject representing a line.
	/// </param>
	/// <param name='index'>
	/// Index of the frame to which the GameObject should belong.
	/// </param>
	public Line(GameObject lineObject, int index)
	{
		this.lineObject=lineObject;
		this.index=index;
	}	
}
