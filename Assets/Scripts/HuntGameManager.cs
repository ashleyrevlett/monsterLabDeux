using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HuntGameManager : MonoBehaviour {
	
	public GameObject scientistPrefab;
	private ScientistController scientist;
	
	private HuntBoardManager boardManager;
	private GameStateStore gss;

	public Text timerText;
	public Text dartsText;
	private float timeRemaining;
	public float sceneTimer; // seconds

	// Use this for initialization
	void Start () {
		GameObject gm = GameObject.Find ("GameManager");
		boardManager = gm.GetComponent<HuntBoardManager> ();
		gss = gm.GetComponent<GameStateStore> ();
				
		// place scientist and start him walking
		GameObject instance = Instantiate (scientistPrefab, new Vector3 ( 1f, 1f, 0f), Quaternion.identity) as GameObject;
		instance.transform.SetParent (boardManager.pieceHolder);
		instance.transform.position = new Vector3 ();
		scientist = instance.GetComponent<ScientistController> ();

//		float boardCenterX = (int)(boardManager.columns / 2f);
//		float boardCenterY = (int)(boardManager.rows / 2f);
		scientist.transform.position = new Vector3 (1f, 1f, 0f);

		// start UI and timer
		timeRemaining = sceneTimer;
		StartCoroutine (Countdown ());


	}

	IEnumerator Countdown() {

		while (timeRemaining >= 0f) {
			timeRemaining -= Time.deltaTime;
			int seconds = Mathf.CeilToInt (timeRemaining);
			timerText.text = string.Format ("0:{0}", seconds.ToString("D2"));
			yield return null;
		}
		Debug.Log ("End Hunt Scene!");

	}

	// Update is called once per frame
	void Update () {
	
	}

}
