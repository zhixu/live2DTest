using UnityEngine;
using System.Collections;

public class Scorekeeper : MonoBehaviour {

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
      private string avatar;

	// Use this for initialization
	void Start () {
        appModel = live2DCanvas.GetComponent<LAppModel>();
        
        Color color = new Color (1, 1, 1);
        color.a = 0;
        percent.material.color = color;
        
        percent.fontSize = Screen.height / 8;

        avatar = "shizuku";//PlayerPrefs.GetString("avatar");
	}
	
	// Update is called once per frame
	void Update () {
        score.text = "score: " + points;
        
        if (percentage == 100) {
          percent.text = percentage.ToString ("F0") + "%";
        } else {
          percent.text = percentage.ToString ("F2") + "%";
        }
        
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
        maru.GetComponent<SpriteRenderer>().color = color;
            
        points++;
        totalQuestionsSoFar++;
        updatePercentage();
            
        appModel.turnOffMouseTracking();

        if (string.Equals(avatar, "shizuku")){
            appModel.setExpression("default");
            if (percentage == 100)
              {
                appModel.startMotion("100 percent", 0, LAppDefine.PRIORITY_NORMAL);
                StartCoroutine(displayReaction(4.0f));
              } else if (percentage >= 70)
                {
                  appModel.startMotion("happy", 0, LAppDefine.PRIORITY_NORMAL);
                  StartCoroutine(displayReaction(3f));
                } else
                  {
                    appModel.startMotion("okay", 0, LAppDefine.PRIORITY_NORMAL);
                    StartCoroutine(displayReaction(5f));
                  }
        } else if (string.Equals(avatar, "ethan")) {
              appModel.setExpression("idle");
              
              if (percentage >= 70) {
                appModel.startMotion ("thumbs up", 0, LAppDefine.PRIORITY_NORMAL);
                StartCoroutine(displayReaction(3f));
              } else {
                appModel.startMotion("happy", 0, LAppDefine.PRIORITY_NORMAL);
                StartCoroutine(displayReaction(3.5f));
              }
        }
    }

	public void subtractPoint() {
        Color color = batsu.GetComponent<SpriteRenderer>().color;
        color.a = 1;
        batsu.GetComponent<SpriteRenderer> ().color = color;
        
        totalQuestionsSoFar++;
        updatePercentage();
        
        appModel.turnOffMouseTracking();

        if (string.Equals(avatar, "shizuku")){
            appModel.setExpression("default");
            if (percentage == 0) {
                appModel.startMotion("overwhelmed", 0, LAppDefine.PRIORITY_NORMAL);
                StartCoroutine(displayReaction(5f));
            } else if (percentage >= 25) {
              appModel.startMotion("sad", 0, LAppDefine.PRIORITY_NORMAL);
              StartCoroutine(displayReaction(4f));
            } else { 
              appModel.startMotion("angry", 0, LAppDefine.PRIORITY_NORMAL);
              StartCoroutine(displayReaction(6f));
            }
        } else if (string.Equals(avatar, "ethan")) {
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
            appModel.startMotion("hands on hips", 0, LAppDefine.PRIORITY_NORMAL);
            StartCoroutine(displayReaction(3));
          }
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

	IEnumerator displayReaction(float n) {

        yield return new WaitForSeconds(n);
        appModel.turnOnMouseTracking ();
        
        if (string.Equals(avatar, "shizuku")){
            if (percentage >= 80) {
              appModel.setExpression("default");
            } else if (percentage >= 60) {
              appModel.setExpression("judging");
            } else if (percentage >= 40) {
              appModel.setExpression("sad");
            } else {
              appModel.setExpression("angry");
            }

        } else if (string.Equals(avatar, "ethan")) {
            if (percentage >= 80) {
              appModel.setExpression("open eyes happy");
            } else if (percentage >= 60) {
              appModel.setExpression("judging");
            } else if (percentage >= 40) {
              appModel.setExpression("sad");
            } else {
              appModel.setExpression("angry");
            }
        }
            
        //set body
        gc.updateQuestion ();
        appModel.startIdleMotion();
		
	}
}
