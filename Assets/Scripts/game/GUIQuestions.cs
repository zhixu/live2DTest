using UnityEngine;
using System.Collections;

public class GUIQuestions : MonoBehaviour {

	public TextMesh question;
	private string questionWord;
	private bool isAskingQuestion;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		question.text = questionWord;
		Color c = question.color;
		if (isAskingQuestion) {
			c.a = 1;
		} else {
			c.a = 0;
		}

		question.color = c;	
	}

	public void setQuestion(string kanji) {
		questionWord = kanji;
		isAskingQuestion = true;
	}

	public void turnOffQuestion() {
		isAskingQuestion = false;
	}
}
