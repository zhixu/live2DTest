using UnityEngine;
using System.Collections;

public class Card {

	private string front;
	private string pronunciation;
	private string back;

	public Card() {
	}

	public Card(string front, string pronunciation, string back) {
		this.front = front;
		this.pronunciation = pronunciation;
		this.back = back;
	}

	public string getFront() {
		return front;
	}

	public string getPronunciation() {
		return pronunciation;
	}

	public string getBack() {
		return back;
	}

}
