using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

namespace Controller {

public class GameController : MonoBehaviour {

	public TextAsset vocabDeck;
	public GUIText index;

	private GUIQuestions guiQuestions;
	private GUIAnswers guiAnswers;
	private EthanScorekeeper scorekeeper;
	
	private string[] kanji;
	private string[] hiragana;
	private string[] english;

	private int questionNumber;
	private int totalScore;

	// Use this for initialization
	void Start () {

		guiQuestions = this.GetComponent<GUIQuestions>();
		guiAnswers = this.GetComponent<GUIAnswers>();
		scorekeeper = this.GetComponent<EthanScorekeeper>();
		
		/*
		if ( this.GetComponent<EthanScorekeeper>() != null) {
			Debug.Log("adding ethan scorekeeper");
			scorekeeper = (EthanScorekeeper) this.GetComponent<EthanScorekeeper>();
		} else {
			Debug.Log("adding regular scorekeeper");
			scorekeeper = this.GetComponent<Scorekeeper>();
		}*/
		
		//split array by comma but ignore the ones within quotation marks; ignore carriage returns as well
		string myVocabDeck = vocabDeck.text.Replace(System.Environment.NewLine, "");
		string[] stringArray = Regex.Split(myVocabDeck, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
		int totalWords = stringArray.Length / 3;

		kanji = new string[totalWords];
		hiragana = new string[totalWords];
		english = new string[totalWords];

		int wordCount = 0;

		for (int i = 0; i < stringArray.Length-1; i+=3) {

			kanji[wordCount] 	= stringArray[i];
			hiragana[wordCount] = stringArray[i+1];
			english[wordCount] 	= stringArray[i+2];

			wordCount++;

		}

		//Enumerable.Range(0, 

		totalScore = kanji.Length;
		if (totalScore == 0) {
			Debug.Log("There are no vocabulary words chosen.");
		}
		updateQuestion();
	}

	public void updateQuestion() {

		if (questionNumber >= totalScore) {
			scorekeeper.savePlayerSettings();
			Application.LoadLevel("ending");
		}

		guiQuestions.setQuestion(kanji[questionNumber]);

		string[] incorrect = new string[guiAnswers.getNumberAnswers()-1];
		for (int i = 0; i < incorrect.Length; i++) {
			int r = Random.Range(0, english.Length);
			incorrect[i] = english[r];
		}

		guiAnswers.setAnswers(english[questionNumber], incorrect);

		questionNumber++;
	}
	
	// Update is called once per frame
	void Update () {
		index.text = "index: " + questionNumber + " of " + totalScore;

	}
}

}