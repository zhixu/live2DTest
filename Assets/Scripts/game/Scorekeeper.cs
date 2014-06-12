using UnityEngine;
using System.Collections;

public class Scorekeeper : MonoBehaviour {

	public GUIText score;
	public GameObject live2DCanvas;

	private LAppModel appModel;

	private int points = 0;
	private int totalQuestionsSoFar = 0;
	private float percentage;

	System.Random rand = new System.Random();

	// Use this for initialization
	void Start () {
		appModel = live2DCanvas.GetComponent<LAppModel>();
	}
	
	// Update is called once per frame
	void Update () {
		score.text = "score: " + points + "\n" + "percent correct: " + percentage.ToString("F2") + "%";
	}

	public void addPoint() {
		points++;
		totalQuestionsSoFar++;
		appModel.setExpression("f04");

		updatePercentage();

		if (rand.NextDouble() > 0.5) {
			if (percentage >= 75) {
				appModel.startMotion(LAppDefine.MOTION_GROUP_TAP_BODY, 1, LAppDefine.PRIORITY_NORMAL); // raises 2 hands happily
			} else if (percentage >= 50) {
				appModel.startMotion(LAppDefine.MOTION_GROUP_TAP_BODY, 0, LAppDefine.PRIORITY_NORMAL); // nod
			} else {
				appModel.startMotion(LAppDefine.MOTION_GROUP_TAP_BODY, 2, LAppDefine.PRIORITY_NORMAL); // slight nod
			}
		}

		StartCoroutine(displayReaction(3.5f));
	}

	public void subtractPoint() {
		totalQuestionsSoFar++;

		appModel.setExpression("f03");

		if (rand.NextDouble() > 0.5) {
			appModel.startMotion(LAppDefine.MOTION_GROUP_PINCH_OUT, 1, LAppDefine.PRIORITY_NORMAL);
		}

		updatePercentage();
		StartCoroutine(displayReaction(6));
	}

	void updatePercentage() {
		if(totalQuestionsSoFar != 0) {
			percentage = ((float) points)/totalQuestionsSoFar * 100;
		}
	}

	public void savePlayerSettings() {
		PlayerPrefs.SetString("percentage", percentage.ToString("F2"));
		PlayerPrefs.SetString("points", points.ToString());
		PlayerPrefs.SetString("total", totalQuestionsSoFar.ToString());
	}

	IEnumerator displayReaction(float n) {

		Debug.Log("questions so far: " + totalQuestionsSoFar);
		yield return new WaitForSeconds(n);
		print("after yield: " + Time.time);
		
		// set face after 1 second
		if (percentage >= 75) {
			appModel.setExpression("f04"); // starry eyed
		} else if (percentage >= 50) {
			appModel.setExpression("f01"); // meh happy
		} else {
			appModel.setExpression("f03"); // angry
		}

		//set body
		appModel.startIdleMotion();
		
	}
}
