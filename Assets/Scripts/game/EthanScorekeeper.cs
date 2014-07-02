using UnityEngine;
using Controller;
using System.Collections;

public class EthanScorekeeper : MonoBehaviour {

	
	public GUIText score;
	public GUIText percent;
	public GameObject live2DCanvas;
	public GameController gc;

	public GameObject maru;
	public GameObject batsu;

	private LAppModel appModel;

	private int points = 0;
	private int totalQuestionsSoFar = 0;
	private float percentage;

	// Use this for initialization
	void Start () {
		appModel = live2DCanvas.GetComponent<LAppModel>();

		Color color = new Color (1, 1, 1);
		color.a = 0;
		percent.material.color = color;

		percent.fontSize = Screen.height / 8;
	}
	
	// Update is called once per frame
	void Update () {
		score.text = "score: " + points;
		percent.text = percentage.ToString ("F2") + "%";

		Color color;
		
		if (percent.material.color.a != 0) {
			color = percent.material.color;
			color.a -= 0.005f;
			percent.material.color = color;
		}
		if (maru.GetComponent<SpriteRenderer> ().color.a != 0) { 
			color = maru.GetComponent<SpriteRenderer>().color;
			color.a -= 0.005f;
			maru.GetComponent<SpriteRenderer>().color = color;
		}
		if (batsu.GetComponent<SpriteRenderer> ().color.a != 0) {
			color = batsu.GetComponent<SpriteRenderer>().color;
			color.a -= 0.005f;
			batsu.GetComponent<SpriteRenderer>().color = color;
		}
	}

	public void addPoint() {

		Color color = maru.GetComponent<SpriteRenderer>().color;
		color.a = 1;
		maru.GetComponent<SpriteRenderer> ().color = color;

		points++;
		totalQuestionsSoFar++;
		updatePercentage();

		appModel.turnOffMouseTracking();
		appModel.setExpression("idle");

		if (percentage >= 70) {
			appModel.startMotion ("thumbs up", 0, LAppDefine.PRIORITY_NORMAL);
		} else {
			appModel.startMotion("happy", 0, LAppDefine.PRIORITY_NORMAL);
		}

		StartCoroutine(displayReaction(3.5f));
	}

	public void subtractPoint() {

		Color color = batsu.GetComponent<SpriteRenderer>().color;
		color.a = 1;
		batsu.GetComponent<SpriteRenderer> ().color = color;

		totalQuestionsSoFar++;
		updatePercentage();

		appModel.turnOffMouseTracking();

		if (percentage >= 60) {

			if (percentage >= 90) {
				appModel.setExpression("open eyes happy");
			} else if (percentage >=80) {
				appModel.setExpression("judging");
			} else if (percentage >=70) {
				appModel.setExpression("sad");
			} else {
				appModel.setExpression("angry");
			}
			
			appModel.startMotion("judging", 0, LAppDefine.PRIORITY_NORMAL);
			StartCoroutine(displayReaction(4));

		} else if (percentage >= 25) {
			appModel.setExpression("angry");
			appModel.startMotion("angry", 0, LAppDefine.PRIORITY_NORMAL);
			StartCoroutine(displayReaction(6));
		} else { 
			appModel.setExpression("angry");
			appModel.startMotion("grossed", 0, LAppDefine.PRIORITY_NORMAL);
			StartCoroutine(displayReaction(3));
		}



	}

	void updatePercentage() {
		if(totalQuestionsSoFar != 0) {
			percentage = ((float) points)/totalQuestionsSoFar * 100;
		}
		Color color = percent.material.color;
		color.a = 1.0f;
		percent.material.color = color;
	}

	public void savePlayerSettings() {
		PlayerPrefs.SetString("percentage", percentage.ToString("F2"));
		PlayerPrefs.SetString("points", points.ToString());
		PlayerPrefs.SetString("total", totalQuestionsSoFar.ToString());
	}

	// changes default face to reflect your score
	IEnumerator displayReaction(float n) {

		yield return new WaitForSeconds(n);
		appModel.turnOnMouseTracking ();
		
		if (percentage >= 80) {
			appModel.setExpression("open eyes happy");
		} else if (percentage >= 60) {
			appModel.setExpression("judging");
		} else if (percentage >= 40) {
			appModel.setExpression("sad");
		} else {
			appModel.setExpression("angry");
		}

		//set body
		gc.updateQuestion ();
		appModel.startIdleMotion();
		
	}
}
