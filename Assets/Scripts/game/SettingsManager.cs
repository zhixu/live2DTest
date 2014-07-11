using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

public class SettingsManager : MonoBehaviour {

	public GUIStyle style;
    public GUIStyle buttonStyle;
    public GUIStyle avatarStyle;

    public Texture shizukuTexture1;
    public Texture shizukuTexture2;
    public Texture ethanTexture1;
    public Texture ethanTexture2;

    public Texture checkbox1;
    public Texture checkbox2;

    private Texture flipbox;
    private Texture shufflebox;
    private Texture shizukuTexture;
    private Texture ethanTexture;
    
	private bool isShuffle = false;
    private bool isFlip = false;
	private bool isEthan = false;
	private bool isShizuku = false;

    private string path = "none";

    private float width, height;

    private Rect settingsLabel, shuffle, shuffleLabel, flip, flipLabel, ethan, shizuku, back, next, load, chooseLabel, loadLabel;

	// Use this for initialization
	void Start () {

		if (PlayerPrefs.HasKey("isShuffle")) isShuffle = bool.Parse (PlayerPrefs.GetString ("isShuffle"));
        if (PlayerPrefs.HasKey("isFlipCard")) isFlip = bool.Parse (PlayerPrefs.GetString("isFlipCard"));
        if (PlayerPrefs.HasKey("deckLocation")) path = PlayerPrefs.GetString("deckLocation");
		if (PlayerPrefs.HasKey ("avatar")) {

			string avatar = PlayerPrefs.GetString ("avatar");
			if (string.Equals (avatar, "ethan")) isEthan = true;
            if (string.Equals (avatar, "shizuku")) isShizuku = true;
 
		} else {
			isEthan = true;
		}

        flipbox = checkbox2;
        shufflebox = checkbox2;

        width = Screen.width / 6;
        height = Screen.height / 6;

        settingsLabel = new Rect(Screen.width/2 - width/2, 0, width, height);
        
        shuffle = new Rect(Screen.width/3, Screen.height/8, width/2, height/2);
        shuffleLabel = new Rect(Screen.width/3 + width/2, Screen.height/8 - height/4, width, height);
        flip = new Rect(Screen.width/3, Screen.height/6+height/2, width/2, height/2);
        flipLabel = new Rect(Screen.width / 3 + width/2, Screen.height/6 + height/4, width, height);

        chooseLabel = new Rect(Screen.width/2 - width*2, Screen.height/3 + height/4, width*4, height);
        loadLabel = new Rect(3 * Screen.width / 10 + Screen.width/20, Screen.height - height * 1.7f, width, height);
                              
        ethan = new Rect(3 * Screen.width / 10 + Screen.width/20, Screen.height / 2, width*1.2f, height*1.2f);
        shizuku = new Rect(Screen.width / 2 + Screen.width/20,Screen.height / 2, width*1.2f, height*1.2f); 
        back = new Rect(Screen.width / 2 - width*0.75f, Screen.height - height/2,width/2,height/2);
        next = new Rect(Screen.width/2 + width/4, Screen.height - height/2, width/2, height/2);
        load = new Rect(Screen.width / 2 + Screen.width/20, Screen.height - height*1.7f, width, height);
	}
	
	// Update is called once per frame
	void Update () {
        if (isShizuku) shizukuTexture = shizukuTexture1;
        if (!isShizuku) shizukuTexture = shizukuTexture2;
        if (isEthan) ethanTexture = ethanTexture1;
        if (!isEthan) ethanTexture = ethanTexture2;
        if (isShuffle) shufflebox = checkbox2;
        if (!isShuffle) shufflebox = checkbox1;
        if (isFlip) flipbox = checkbox2;
        if (!isFlip) flipbox = checkbox1;
	}

	void OnGUI() {
		
        style.fontSize = Screen.height / 25;
        buttonStyle.fontSize = Screen.height / 30;
        string name = Path.GetFileName(path);

        GUI.Label(settingsLabel, "Settings", style);
        GUI.Label(shuffleLabel, "shuffle cards", style);
        GUI.Label(flipLabel, "flip cards", style);
        GUI.Label(chooseLabel, "choose character:", style);
        GUI.Label(loadLabel, name, style);
	
        if (GUI.Button (shuffle, shufflebox, avatarStyle)) isShuffle = !isShuffle;
        if (GUI.Button (flip, flipbox, avatarStyle)) isFlip = !isFlip;
        
		if (GUI.Button (ethan, ethanTexture, avatarStyle)) {
			isEthan = !isEthan;
			if (isEthan) isShizuku = false;
		}

		if (GUI.Button (shizuku, shizukuTexture, avatarStyle)) {
			isShizuku = !isShizuku;
			if (isShizuku)
			isEthan = false;
		}

        if (GUI.Button (load, "load deck", buttonStyle)) {
            Debug.Log("opening file dialogue");
            path = GetFile.OpenFileDialog();
            Debug.Log("file path: " + path);
        }

        if (GUI.Button (back, "back", buttonStyle)) Application.LoadLevel("splash");

		if (GUI.Button (next, "next", buttonStyle)) {
            
            PlayerPrefs.SetString ("isShuffle", isShuffle ? "true":"false");
            PlayerPrefs.SetString ("isFlipCard", isFlip ? "true":"false");
            PlayerPrefs.SetString ("deckLocation", path);

            Debug.Log("isShuffle: " + PlayerPrefs.GetString("isShuffle") + " isFlipCard: " + PlayerPrefs.GetString("isFlipCard"));

			if (isEthan) PlayerPrefs.SetString ("avatar", "ethan");
			if (isShizuku) PlayerPrefs.SetString ("avatar", "shizuku");
			if (!isEthan && !isShizuku) {
				Debug.Log ("there is a problem");
			}

            if (isEthan) Application.LoadLevel ("ethan");
            if (isShizuku) Application.LoadLevel ("shizuku");
		}
	}
}