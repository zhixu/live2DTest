using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class GameController : MonoBehaviour {

	public TextAsset vocabDeck;

	private GUIQuestions guiQuestions;
	private GUIAnswers guiAnswers;
	
	private string[] kanji;
	private string[] hiragana;
	private string[] english;

	private int[] randomQuestionPicker;
	private int questionNumber;

	private int currentScore = 0;
	private int totalScore;

	// Use this for initialization
	void Start () {

		guiQuestions = this.GetComponent<GUIQuestions>();
		guiAnswers = this.GetComponent<GUIAnswers>();
		
		//split array by comma but ignore the ones within quotation marks
		string[] stringArray = Regex.Split(vocabDeck.text, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
		int totalWords = stringArray.Length / 3;

		Debug.Log("totalwords: " + totalWords);

		kanji = new string[totalWords];
		hiragana = new string[totalWords];
		english = new string[totalWords];

		int wordCount = 0;

		for (int i = 0; i < stringArray.Length-1; i+=3) {
			//Debug.Log("wordCount: " + wordCount + " i " + i + " totalWords: " + totalWords + " arrayLength: " + stringArray.Length);
			kanji[wordCount] 	= stringArray[i];
			hiragana[wordCount] = stringArray[i+1];
			english[wordCount] 	= stringArray[i+2];

			wordCount++;
		}

		/*
		for (int i = 0; i < wordCount; i++) {
			Debug.Log("kanji: " + kanji[i] + " hiragana: " + hiragana[i] + " english: " + english[i]);
		}*/

		totalScore = kanji.Length;
		updateQuestion();
	}

	public void updateQuestion() {

		Debug.Log("update question function");

		Debug.Log("question: " + kanji[questionNumber]);
		guiQuestions.setQuestion(kanji[questionNumber]);

		string[] incorrect = new string[guiAnswers.getNumberAnswers()];
		for (int i = 0; i < incorrect.Length; i++) {
			int r = Random.Range(0, english.Length);
			Debug.Log("INCORRECT: " + english[r]);

			incorrect[i] = english[r];
		}

		guiAnswers.setAnswers(english[questionNumber], incorrect);

		questionNumber++;
	}

	public void addPoint() {
		currentScore++;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}