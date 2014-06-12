using UnityEngine;
using System.Collections;

public class GUIAnswers : MonoBehaviour {

	private Scorekeeper scorekeeper;
	private GameController gc;

	private string[] answers;
	private string correctAnswer = null;

	private int numberAnswers = 4;

	private int[] randArray;
	// Use this for initialization
	void Start () {
		scorekeeper = this.GetComponent<Scorekeeper>();
		gc = this.GetComponent<GameController>();

		answers = new string[numberAnswers];
		for (int i = 0; i < numberAnswers; i++) {
			answers[i] = "answer";
		}

		// create a random array of integers and shuffle contents for random answer buttons
		randArray = new int[numberAnswers];
		for (int i = 0; i < randArray.Length; i++)
			randArray[i] = i;
		shuffleRandArray();

	}

	public int getNumberAnswers() {
		return numberAnswers;
	}

	public void setAnswers(string correct, string[] incorrect) {

		correctAnswer = correct;

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

		float left = 10.0f;
		float top = 130.0f;
		float width = 175.0f;
		float height = 60.0f;
		int answerCount = 0;

		GUI.skin.button.wordWrap = true;

		foreach (string answer in answers) {
			if (GUI.Button(new Rect(left, top, width, height), answer)) {
				
				if (string.Equals(answer, correctAnswer)) {
					scorekeeper.addPoint();
				} else {
					scorekeeper.subtractPoint();
				}
				gc.updateQuestion();
			}

			top += 100;
			answerCount++;

			if (answerCount == answers.Length/2) {
				left = Screen.width - 185.0f;
				top = 130.0f;
			}
		}
	}
}
