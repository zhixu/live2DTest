using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	private string vocabDeck;
	public GUIText index;

    private int numIncorrect = 3;

	private GUIQuestions guiQuestions;
	private GUIAnswers guiAnswers;
	private Scorekeeper scorekeeper;
	
	private List<Card> cards;
	private int questionNumber = 0;
	private int totalScore;

	// Use this for initialization
	void Start () {

		guiQuestions = this.GetComponent<GUIQuestions>();
		guiAnswers = this.GetComponent<GUIAnswers>();
		scorekeeper = this.GetComponent<Scorekeeper>();
 
        if (PlayerPrefs.HasKey("deckLocation")) {
          string path = PlayerPrefs.GetString("deckLocation");
          WWW www = new WWW("file://" + path);
          StartCoroutine(waitForRequest(www));
        }
	}

      IEnumerator waitForRequest (WWW www) {
        yield return www;
        byte[] bytes = www.bytes;
        vocabDeck = System.Text.Encoding.UTF8.GetString(bytes);
        
        if(PlayerPrefs.HasKey("isFlipCard")) {
          
          if (bool.Parse (PlayerPrefs.GetString("isFlipCard"))) loadCards(2, 1, 0);
          if (! bool.Parse (PlayerPrefs.GetString("isFlipCard"))) {
            loadCards(0, 1, 2);
          }
        } else {
          loadCards(0, 1, 2);
            }
        }

    void loadCards(int a, int b, int c) {
        
          //split array by comma but ignore the ones within quotation marks; ignore carriage returns as well
          //string myVocabDeck = vocabDeck.text.Replace(System.Environment.NewLine, "");
          string[] deckArray = Regex.Split(vocabDeck, System.Environment.NewLine);
          
          //string[] lastCard = Regex.Split(deckArray[deckArray.Length-1]);

          //cards = new Card[deckArray.Length];
          cards = new List<Card>();      

          int wordCount = 0;
          foreach (string cardString in deckArray) {

                  string[] stringArray = Regex.Split(cardString, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                  if (stringArray.Length >= 3) {
                      Card card = new Card(stringArray[a], stringArray[b], stringArray[c]);
                      cards.Add(card);
                      
                      wordCount++;
                  }
            }

        if (PlayerPrefs.HasKey("isShuffle")) {
          if (bool.Parse (PlayerPrefs.GetString ("isShuffle"))) shuffleDeck ();
        }
        totalScore = cards.Count;
        if (totalScore == 0) {
          Debug.Log("There are no vocabulary words in this deck.");
        }
        updateQuestion();
    
    }
    
	public void updateQuestion() {
        
		if (questionNumber >= totalScore) {
			scorekeeper.savePlayerSettings();
			Application.LoadLevel("ending");
		}

		guiQuestions.setQuestion(cards[questionNumber].getFront ());

		string[] incorrect = new string[numIncorrect];
		List<int> random = new List<int> ();
        random.Add(questionNumber);

        if (totalScore-1 > numIncorrect) {
    		for (int i = 0; i < incorrect.Length; i++) {
                int r;
    			while(true) {
    			    r = Random.Range(0, totalScore);
    			    if (!random.Contains(r)) break;
    			}
    			
    			random.Add (r);
                    
    			incorrect[i] = cards[r].getBack ();
    		}	
        } else {
            int incNumber = 0;
            for (int i = 0; i < cards.Count; i++) {
                if (i == questionNumber ) continue;
                incorrect[incNumber] = cards[i].getBack();
                incNumber++;
            }
            for (; incNumber < incorrect.Length; incNumber++) {
                incorrect[incNumber] = null;
            }
        }
        guiAnswers.setAnswers(cards[questionNumber].getBack (), incorrect);
        questionNumber++;
	}
	
	// Update is called once per frame
	void Update () {
		index.text = "index: " + questionNumber + " of " + totalScore;

	}
            
    void shuffleDeck() {
        for (int i = 0; i < totalScore; i++) {
            Card temp = cards[i];
            int r = Random.Range(0, totalScore);
            cards[i] = cards[r];
            cards[r] = temp;
        }
    }
        
    public string getCurrentPronunciation() {
        return cards [questionNumber-1].getPronunciation();
    }

}