using UnityEngine;
using System.Collections;

public class GUIAnswers : MonoBehaviour {
	
	public GUIStyle answerStyle;
    public Font font;
    public Font altFont;

	private EthanScorekeeper scorekeeper;
	private GameController gc;

	private string[] answers;
	private string correctAnswer = null;

	private int numberAnswers = 4;

	private int[] randArray;

	private float heightIncrement;

	private float left;
	private float top;
	private float width;
	private float height;
	private int answerCount;

	private bool isFirstElimination = true;
	private bool isAskingQuestion = true;

	// Use this for initialization
	void Start () {
		scorekeeper = this.GetComponent<EthanScorekeeper>();
		gc = this.GetComponent<GameController>();

		heightIncrement = 3.0f * Screen.height / 8.0f;

		answers = new string[numberAnswers];
		for (int i = 0; i < numberAnswers; i++) {
			answers[i] = "answer";
		}

        if (PlayerPrefs.HasKey("isFlipCard")) {
            if (bool.Parse (PlayerPrefs.GetString("isFlipCard"))) answerStyle.font = altFont;
            if (! bool.Parse (PlayerPrefs.GetString("isFlipCard"))) answerStyle.font = font;
        }
                
        // create a random array of integers and shuffle contents for random answer buttons
		randArray = new int[numberAnswers];
		for (int i = 0; i < randArray.Length; i++)
			randArray[i] = i;
		shuffleRandArray();

        answerStyle.fontSize = Screen.height / 14;
	}
    
	public int getNumberAnswers() {
		return numberAnswers;
	}

	public void setAnswers(string correct, string[] incorrect) {

		correctAnswer = correct;
		isFirstElimination = true;
		isAskingQuestion = true;

		if (incorrect.Length < numberAnswers) {
			for (int i = 0; i < incorrect.Length; i++)
				answers[randArray[i]] = incorrect[i];
			answers[randArray[numberAnswers-1]] = correct;
			shuffleRandArray();
		} else {
			Debug.Log("Number of answers are not matching.");
		}

	}

	void shuffleRandArray() {
		
		for (int i = 0; i < randArray.Length; i++) {

			int temp = randArray[i];
			int r = Random.Range(0, randArray.Length);
			randArray[i] = randArray[r];
			randArray[r] = temp;
		}

	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI() {

        left = Screen.width / 16.0f;
        top = Screen.height / 6.0f;
        width = Screen.width / 4.0f;
        height = Screen.height / 4.0f;
            
		answerCount = 0;

		if (isAskingQuestion) {

			foreach (string answer in answers) {

				if ( answer != null) {

					if (GUI.Button(new Rect(left, top, width, height), answer, answerStyle)) {
						
						if (string.Equals(answer, correctAnswer)) {
							scorekeeper.addPoint();
						} else {
							scorekeeper.subtractPoint();
						}

						isAskingQuestion = false;
						GUIQuestions q = this.GetComponent <GUIQuestions>();
						q.turnOffQuestion();
					}
				}

				top += heightIncrement;
				answerCount++;

				if (answerCount == answers.Length/2) {
					left = Screen.width - 5.0f * Screen.width / 16.0f;
					top = Screen.height / 6.0f;
				}
			}
		}

        if (GUI.Button(new Rect(Screen.width-width/4-10, Screen.height-width/4-10, width/4, height/4), "back")) {
            Application.LoadLevel("splash");
        }
	}

	// eliminates the answer if a double tap event has occurred
	public void eliminateAnswer() {
        
		if (isFirstElimination) {
			Debug.Log ("eliminate answer has been called.");

			int numEliminated = 0;
			for (int i = 0; i < randArray.Length; i++) {
				if (numEliminated == answerCount/2) break;

				int temp = randArray[i];

				if (string.Equals (answers[temp], correctAnswer) ) continue;

				answers[temp] = null;

				numEliminated++;
			}

			isFirstElimination = false;
		}

	}
}
