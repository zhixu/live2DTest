using UnityEngine;
using System.Collections;

using System;
using System.Runtime.InteropServices;


public class Mp3Importer
{	
	public IntPtr handle_mpg;
	public IntPtr errPtr;
	public IntPtr rate;
	public IntPtr channels;
	public IntPtr encoding;
	public IntPtr id3v1;
	public IntPtr id3v2;
	public IntPtr done;
	
	public int status;
	public int intRate;
	public int intChannels;
	public int intEncoding;
	public int FrameSize;
	public int lengthSamples;
	public AudioClip myClip;	
	
	 #region Consts: Standard values used in almost all conversions.
	private const float const_1_div_128_ = 1.0f / 128.0f;  // 8 bit multiplier
	private const float const_1_div_32768_ = 1.0f / 32768.0f; // 16 bit multiplier
	private const double const_1_div_2147483648_ = 1.0 / 2147483648.0; // 32 bit
     #endregion
	
	/// <summary>
	/// Import the mp3 file at the specified filePath.
	/// </summary>
	/// <returns>
	/// AudioClip with data from the decoded MP3 file.
	/// </returns>
	/// <param name='filePath'>
	/// File path.
	/// </param>
	private AudioClip ImportFile (string filePath)
	{		
		MPGImport.mpg123_init ();
		handle_mpg = MPGImport.mpg123_new (null, errPtr);
		status = MPGImport.mpg123_open (handle_mpg, filePath);		
		MPGImport.mpg123_getformat (handle_mpg, out rate, out channels, out encoding);
		intRate = rate.ToInt32 ();
		intChannels = channels.ToInt32 ();
		intEncoding = encoding.ToInt32 ();
		
		MPGImport.mpg123_id3 (handle_mpg, out id3v1, out id3v2);		
		MPGImport.mpg123_format_none (handle_mpg);
		MPGImport.mpg123_format (handle_mpg, intRate, intChannels, 208);
		
		FrameSize = MPGImport.mpg123_outblock (handle_mpg);		
		byte[] Buffer = new byte[FrameSize];		
		lengthSamples = MPGImport.mpg123_length (handle_mpg);
		
		
		//A decoded audio file with a length of 
		//~one hour is likely to take up more than 1 gB of ram.
		if(lengthSamples/intRate>3000){
			Debug.LogError("Audio file too big");
			return null;
		}
		
		if(lengthSamples/intRate>2000)
			Debug.LogWarning("Large audio file");
		
		myClip = AudioClip.Create ("myClip", lengthSamples, intChannels, intRate, false, false);
				
		int importIndex = 0;
		
		while (0 == MPGImport.mpg123_read(handle_mpg, Buffer, FrameSize, out done)) {
                
			
			float[] fArray;
			fArray = ByteToFloat (Buffer);
								
			myClip.SetData (fArray, (importIndex * fArray.Length) / 2);
			
			importIndex++;                 
		}			
		
		MPGImport.mpg123_close (handle_mpg);
						
		return myClip;
	}
	
	/// <summary>
	/// Import the mp3 file at the specified filePath.
	/// </summary>
	/// <returns>
	/// AudioClip with data from the decoded MP3 file.
	/// </returns>
	/// <param name='filePath'>
	/// File path.
	/// </param>
	public static AudioClip Import (string filePath)
	{
		Mp3Importer mp3Import = new Mp3Importer();
		return mp3Import.ImportFile(filePath);
	}
	
	/// <summary>
	/// Opens an OpenFileDialog and immediately imports the selected file;
	/// </summary>
	public static AudioClip Import ()
	{
		string filePath = Util.OpenFileDialog();
		Mp3Importer mp3Import = new Mp3Importer();
		return mp3Import.ImportFile(filePath);
	}
		
	public float[] IntToFloat (Int16[] from)
	{
		float[] to = new float[from.Length];
            
		for (int i = 0; i < from.Length; i++)
			to [i] = (float)(from [i] * const_1_div_32768_);

		return to;
	}

	public Int16[] ByteToInt16 (byte[] buffer)
	{
		Int16[] result = new Int16[1];
		int size = buffer.Length;
		if ((size % 2) != 0) {
			/* Error here */
			Console.WriteLine ("error");
			return result;
		} else {
			result = new Int16[size / 2];
			IntPtr ptr_src = Marshal.AllocHGlobal (size);
			Marshal.Copy (buffer, 0, ptr_src, size);
			Marshal.Copy (ptr_src, result, 0, result.Length);
			Marshal.FreeHGlobal (ptr_src);
			return result;
		}
	}
	
	public float[] ByteToFloat (byte[] bArray)
	{
		Int16[] iArray;		
        	
		iArray = ByteToInt16 (bArray);
		
		return IntToFloat (iArray);
	}
	
	
}
