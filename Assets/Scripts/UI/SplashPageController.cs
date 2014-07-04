using UnityEngine;
using System.Collections;

public class SplashPageController : MonoBehaviour {

	public GUIStyle style;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		style.fontSize = Screen.height / 14;

		float left = Screen.width/2-Screen.width/8;
		float top = Screen.height/2-Screen.height/40;
		float width = Screen.width/4;
		float height = Screen.height/4;

		if (GUI.Button (new Rect (left, top, width, height), "start", style)) {
			if (PlayerPrefs.HasKey ("avatar")) {

				string avatar = PlayerPrefs.GetString ("avatar");

				if (string.Equals (avatar, "ethan")) Application.LoadLevel ("ethan");
				if (string.Equals (avatar, "shizuku")) Application.LoadLevel ("shizuku");

			} else {
				Application.LoadLevel ("ethan");
			}
		}

		if (GUI.Button (new Rect (left, top + Screen.height/4 + 10, width, height), "settings", style)) {
			Application.LoadLevel ("settings");
		}
	}
    
}
