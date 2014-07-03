using UnityEngine;
using System.Collections;
using System.Windows.Forms;

public static class Util {

	/// <summary>
	/// Shows an openfiledialog.
	/// this method (workaround) works in the standalone player, but gives some warning messages in the editor
	///	and can become hidden in full screen mode.
	/// </summary>
	/// <returns>
	/// File path of the selected file.
	/// </returns>
	public static string OpenFileDialog()
	{
		string path;

		OpenFileDialog openFileDialog = new OpenFileDialog ();
		openFileDialog.Filter = "mp3 files (*.mp3)|*.mp3";		
		Form topForm = new Form(){TopMost=true, TopLevel=true};
		openFileDialog.ShowDialog (topForm);
		topForm.Close();		
		path = openFileDialog.FileName;
		openFileDialog.Dispose ();
		
		return path;
	}
	
		
	/// <summary>
	/// Calculates spectral magnitude
	/// </summary>
	/// <returns>
	/// Array of spectral magnitude.
	/// </returns>
	/// <param name='spectrum'>
	/// Complex valued spectrum with complex data stored as alternating real                                       
	/// and imaginary parts
	/// </param>
	public static float[] SpectrumMagnitude (float[] spectrum)
	{
		float[] output = new float[(spectrum.Length - 2) / 2];
		for (int i = 0; i<output.Length; i++) {
			int ii = (i * 2) + 2;			
			output [i] = Mathf.Sqrt ((spectrum [ii] * spectrum [ii]) + (spectrum [ii + 1] * spectrum [ii + 1]));
		}
		return output;
	}
	
	/// <summary>
	/// Calculates spectral magnitude
	/// </summary>
	/// <returns>
	/// Array of spectral magnitude.
	/// </returns>
	/// <param name='spectrum'>
	/// Complex valued spectrum with complex data stored as alternating real                                       
	/// and imaginary parts
	/// </param>
	public static float[] SpectrumMagnitude (double[] spectrum)
	{
		float[] output = new float[(spectrum.Length - 2) / 2];
		for (int i = 0; i<output.Length; i++) {
			int ii = (i * 2) + 2;
			float x = (float)spectrum [ii];
			float y = (float)spectrum [ii + 1];
			output [i] = Mathf.Sqrt ((x * x) + (y * y));
		}
		return output;
	}
	
	/// <summary>
	/// Converts array of floats to array of doubles.
	/// </summary>
	/// <returns>
	/// Array of doubles.
	/// </returns>
	/// <param name='input'>
	/// Input.
	/// </param>
	public static double[] FloatsToDoubles (float[] input)
	{
		double[] output = new double[input.Length];
		for (int i = 0; i < input.Length; i++) {
			output [i] = input [i];
		}
		return output;
	}	
}
