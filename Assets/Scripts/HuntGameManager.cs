using UnityEngine;
using System.Collections;

public class HuntGameManager : MonoBehaviour {
	
	public GameObject scientistPrefab;
	private ScientistController scientist;
	
	private HuntBoardManager boardManager;
	private GameStateStore gss;


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

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
