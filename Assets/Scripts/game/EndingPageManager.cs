using UnityEngine;
using System.Collections;

public class EndingPageManager : MonoBehaviour {

	public GUIText thisText;
	public GUIStyle style;
    public GameObject hanamaru;

    private float finalPercent;

	// Use this for initialization
	void Start () {
		thisText.text = PlayerPrefs.GetString("points") + " out of " + PlayerPrefs.GetString("total")  + " correct"
					+ "\n" + PlayerPrefs.GetString("percentage") + " % accuracy " + "\n";

		finalPercent = float.Parse(PlayerPrefs.GetString("percentage"));
		if (finalPercent >= 80) {
			thisText.text = thisText.text + "\n" + "Congrats, you did it!";
		} else if (finalPercent >= 60) {
			thisText.text = thisText.text + "\n" + "You can do better. Study up!";
		} else {
			thisText.text = thisText.text + "\n" + "You need more practice. Study up!";
		}

        hanamaru.animation.
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {

		style.fontSize = Screen.height / 14;

		float left = Screen.width/2-Screen.width/10;
		float top = Screen.height/2;
        float width = Screen.width/5;
        float height = Screen.height/5;

		if (GUI.Button(new Rect(left, top, width, height), "Retry", style)) {
			Application.LoadLevel("splash");
		}

		if (GUI.Button(new Rect(left, top + height + 10, width, height), "Quit", style)) {
			Application.Quit();
		}

	}
}
