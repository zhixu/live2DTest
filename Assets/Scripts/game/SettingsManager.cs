using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour {

	public GUIText stats;
	public GUIStyle style;

	private bool isShuffle = false;
	private bool isEthan = false;
	private bool isShizuku = false;

	private string shuffleStatus;

	// Use this for initialization
	void Start () {

        stats.text = "Shuffle Status:";

		if (PlayerPrefs.HasKey("isShuffle")) isShuffle = bool.Parse (PlayerPrefs.GetString ("isShuffle"));
		if (PlayerPrefs.HasKey ("avatar")) {

			string avatar = PlayerPrefs.GetString ("avatar");
			Debug.Log (avatar);
			Debug.Log (string.Equals (avatar, "ethan"));

			if (string.Equals (avatar, "ethan")) isEthan = true;
			if (string.Equals (avatar, "shizuku"))isShizuku = true;
		} else {
			isEthan = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		float width = Screen.width / 6;
		float height = Screen.height / 6;
		style.fontSize = Screen.height / 20;

		Rect shuffle = new Rect(Screen.width/2 - width/2, Screen.height/3, width, height);
		Rect ethan = new Rect(3 * Screen.width / 10 + Screen.width/20, Screen.height / 2, width, height);
		Rect shizuku = new Rect(Screen.width / 2 + Screen.width/20,Screen.height / 2, width, height); 
		Rect back = new Rect(Screen.width / 2 - width/2, 3 * Screen.height / 4,width,height);

		shuffleStatus = isShuffle ? "On":"Off";
	
		if (GUI.Button (shuffle, shuffleStatus, style)) {
			isShuffle = !isShuffle;
			PlayerPrefs.SetString ("isShuffle", isShuffle ? "true":"false");
		}

		/*if (GUI.Button (ethan, isEthan ? "using Ethan" : "not using Ethan", style)) {
			isEthan = !isEthan;
			if (isEthan) isShizuku = false;
		}

		if (GUI.Button (shizuku, isShizuku ? "using Shizuku" : "not using Shizuku", style)) {
			isShizuku = !isShizuku;
			if (isShizuku)
			isEthan = false;
		}*/

		if (GUI.Button (back, "back", style)) {
			if (isEthan) PlayerPrefs.SetString ("avatar", "ethan");
			if (isShizuku) PlayerPrefs.SetString ("avatar", "shizuku");
			if (!isEthan && !isShizuku) {
				Debug.Log ("there is a problem");
			}

			Application.LoadLevel ("splash");
		}
	}
}