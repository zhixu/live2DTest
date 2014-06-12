﻿using UnityEngine;
using System.Collections;

public class EndingPageManager : MonoBehaviour {

	public GUIText thisText;

	// Use this for initialization
	void Start () {
		thisText.text = PlayerPrefs.GetString("points") + " out of " + PlayerPrefs.GetString("total")  + " correct"
					+ "\n" + PlayerPrefs.GetString("percentage") + " % accuracy " + "\n";

		float finalPercent = float.Parse(PlayerPrefs.GetString("percentage"));
		if (finalPercent >= 80) {
			thisText.text = thisText.text + "\n" + "Congrats, you're good at this!";
		} else if (finalPercent >= 60) {
			thisText.text = thisText.text + "\n" + "You can do better. Study up!";
		} else {
			thisText.text = thisText.text + "\n" + "Go do more practice!";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {

		float left = Screen.width/2-100;
		float top = Screen.height/4 * 3;
		float width = 200;
		float height = 60;

		if (GUI.Button(new Rect(left, top, width, height), "Retry")) {
			Application.LoadLevel("MainScene");
		}

		if (GUI.Button(new Rect(left, top + 80.0f, width, height), "Quit")) {
			Application.Quit();
		}

	}
}
