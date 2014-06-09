using UnityEngine;
using System.Collections;

public class GUIQuestions : MonoBehaviour {

	public TextMesh question;
	private string questionWord;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		question.text = questionWord;
	}

	public void setQuestion(string kanji) {
		questionWord = kanji;
	}
}
