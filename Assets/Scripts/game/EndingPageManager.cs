using UnityEngine;
using System.Collections;

public class EndingPageManager : MonoBehaviour {

	public GUIText thisText;
	public GUIStyle style;
    public GameObject[] gradeObjects;

    private float finalPercent;

    private float left, top, width, height;
    Rect retry, quit;

	// Use this for initialization
	void Start () {
		
    finalPercent = float.Parse(PlayerPrefs.GetString("percentage"));
		
        if (finalPercent == 100) {
            //GameObject.Destroy(grade);
            setActive("hanamaru");
            setActive("Aplus");
		} else if (finalPercent >= 90) {
            //GameObject.Destroy(hanamaru);
            setActive("A");
		} else if (finalPercent >= 80) {
            setActive("B");
		} else if (finalPercent >= 70) {
            setActive("C");
        } else if (finalPercent >= 60) {
            setActive("D");
        } else if (finalPercent >= 50) {
            setActive("E");
        } else {
            setActive("F");
        }

        thisText.text = "Results:" + "" +
            "\n\n" + PlayerPrefs.GetString("points") + " out of " + PlayerPrefs.GetString("total")  + " correct"
          + "\n" + finalPercent + " % accuracy " + "\n";

        left = Screen.width*0.25f;
        top = Screen.height*0.75f;
        width = Screen.width*0.25f;
        height = Screen.height*0.25f;

        retry = new Rect(left, top, width, height);
        quit = new Rect(Screen.width*.5f, Screen.height*0.75f, width, height);
	}

    void setActive(string name) {

        foreach (GameObject gObject in gradeObjects) {
          if (string.Equals(gObject.name, name)) {
            gObject.SetActive(true);
            break;
          }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {

		style.fontSize = Screen.height / 14;

        

		if (GUI.Button(retry, "Retry", style)) {
			Application.LoadLevel("splash");
		}

		if (GUI.Button(quit, "Quit", style)) {
			Application.Quit();
		}

	}
}
