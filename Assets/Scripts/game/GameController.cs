using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Controller {

public class GameController : MonoBehaviour {

	public TextAsset vocabDeck;
	public GUIText index;

	private GUIQuestions guiQuestions;
	private GUIAnswers guiAnswers;
	private EthanScorekeeper scorekeeper;
	
	private Card[] cards;

	private int questionNumber;
	private int totalScore;

	// Use this for initialization
	void Start () {

		guiQuestions = this.GetComponent<GUIQuestions>();
		guiAnswers = this.GetComponent<GUIAnswers>();
		scorekeeper = this.GetComponent<EthanScorekeeper>();
		
		//split array by comma but ignore the ones within quotation marks; ignore carriage returns as well
		string myVocabDeck = vocabDeck.text.Replace(System.Environment.NewLine, "");
		string[] stringArray = Regex.Split(myVocabDeck, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
		int totalWords = stringArray.Length / 3;
		
        cards = new Card[totalWords];

		int wordCount = 0;

		for (int i = 0; i < stringArray.Length-1; i+=3) {

            Card card = new Card(stringArray[i], stringArray[i+1], stringArray[i+2]);
            cards[wordCount] = card;

			wordCount++;
		}
            
        if (PlayerPrefs.HasKey("isShuffle")) {
                if (bool.Parse (PlayerPrefs.GetString ("isShuffle"))) {
                    shuffleDeck ();
                }
        }
		totalScore = cards.Length;
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

		guiQuestions.setQuestion(cards[questionNumber].getFront ());

		string[] incorrect = new string[guiAnswers.getNumberAnswers()-1];
		List<int> random = new List<int> ();
        random.Add(questionNumber);

		for (int i = 0; i < incorrect.Length; i++) {
            int r;
			while(true) {
			    r = Random.Range(0, cards.Length);
			    if (!random.Contains(r)) break;
			}
			
			random.Add (r);
                
			incorrect[i] = cards[r].getBack ();
		}

		guiAnswers.setAnswers(cards[questionNumber].getBack (), incorrect);

		questionNumber++;
	}
	
	// Update is called once per frame
	void Update () {
		index.text = "index: " + questionNumber + " of " + totalScore;

	}
            
    void shuffleDeck() {
        for (int i = 0; i < cards.Length; i++) {
            Card temp = cards[i];
            int r = Random.Range(0, cards.Length);
            cards[i] = cards[r];
            cards[r] = temp;
        }
    }
}

}