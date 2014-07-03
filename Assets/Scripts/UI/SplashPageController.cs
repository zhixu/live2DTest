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
		style.fontSize = Screen.height / 20;

		float left = Screen.width/2-100;
		float top = Screen.height/5 * 3;
		float width = 200;
		float height = 60;

		if (GUI.Button (new Rect (left, top, width, height), "start", style)) {
			if (PlayerPrefs.HasKey ("avatar")) {

				string avatar = PlayerPrefs.GetString ("avatar");

				if (string.Equals (avatar, "ethan")) Application.LoadLevel ("ethan");
				if (string.Equals (avatar, "shizuku")) Application.LoadLevel ("shizuku");
			} else {
				Application.LoadLevel ("ethanscene");
			}
		}

		if (GUI.Button (new Rect (left, top + 80.0f, width, height), "settings", style)) {
			Application.LoadLevel ("settings");
		}
	}


}
