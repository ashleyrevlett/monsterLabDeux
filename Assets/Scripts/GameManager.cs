using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject scientistPrefab;
	private ScientistController scientist;

	public GameObject[] monsterObjects;
	private MonsterController[] monsters;

	public GameObject infoPanelObject;
	private InfoPanelManager infoPanel;

	public float dragSpeed = 15f;
	public GameObject heldPiece; // gameobject prefab of piece
	private BoardManager boardManager;
	private GameStateStore gss;

	void Start () {
		GameObject gm = GameObject.Find ("GameManager");
		boardManager = gm.GetComponent<BoardManager> ();
		gss = gm.GetComponent<GameStateStore> ();
		infoPanel = gm.GetComponent<InfoPanelManager> ();
		infoPanelObject.SetActive (false);

		// place scientist and start him walking
		GameObject instance = Instantiate (scientistPrefab, new Vector3 ( 1f, 1f, 0f), Quaternion.identity) as GameObject;
		instance.transform.SetParent (boardManager.pieceHolder);
		scientist = instance.GetComponent<ScientistController> ();
	}
	

	void Update () {
		// update position of held piece
		if (heldPiece != null) {		
			var posVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			posVec.z = 0f;
			heldPiece.transform.position = Vector3.MoveTowards(heldPiece.transform.position, posVec, dragSpeed * Time.deltaTime);			
			if (Input.GetKeyDown("space")) {
				Debug.Log("Space key pressed");
				heldPiece.transform.Rotate (Vector3.forward * 90);
			}
		}
	}
	
	// pick up a game piece
	public void HoldPiece(GameObject piece) {
		hideInfo (); // in case we're viewing info panel, close it
		float price = piece.GetComponent<LabItem>().price;
		if (gss.getRemainingMoney() >= price) {
			var posVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			posVec.z = 0f;
			heldPiece = Instantiate (piece, posVec, Quaternion.identity) as GameObject;		 
			heldPiece.transform.SetParent (boardManager.pieceHolder);
		} else {
			Debug.Log("Not enough money to pick up");
		}
	}


	// drop a game piece
	public void DropPiece(Vector3 position) {
	
		if (heldPiece == null)
			return;
		
		Debug.Log ("Dropping piece from gm");

		if (heldPiece.tag == "Obstacle") { // obstacle == labitem
			if (boardManager.isLocationValid ((int)position.x, (int)position.y, heldPiece)) {
				float price = heldPiece.GetComponent<LabItem> ().price;
				gss.deductMoney (price);
				boardManager.placePiece ((int)position.x, (int)position.y, heldPiece);
				Debug.Log ("Money deducted");
			} else {
				// destroy if we can't place it
				Destroy (heldPiece); // don't need this gameobject anymore
			}			
			// no longer holding it either way		
			heldPiece = null;
		} else if (heldPiece.tag == "Monster") {
			if (boardManager.isMonsterLocationValid ((int)position.x, (int)position.y, heldPiece)) {
				boardManager.placeMonsterPiece ((int)position.x, (int)position.y, heldPiece);
				Debug.Log ("Monster placed");
				heldPiece = null;
			} 
			// do not destroy monster; hold it until we place it in the right spot
		}

	}

	
	public void createNewMonster() {
		hideInfo (); // in case we're viewing info panel, close it
		GameObject instance = Instantiate (monsterObjects[0], new Vector3 ( 1f, 1f, 0f), Quaternion.identity) as GameObject;
		instance.transform.SetParent (boardManager.pieceHolder);
		heldPiece = instance;

	}

	// show the info panel with the monster's live info
	public void showInfo(MonsterController monster) {	
		// if holding a piece, do not activate infopanel
		if (heldPiece == null) {
			infoPanelObject.SetActive (true);
			infoPanel.displayMonster (monster);	
		}
	}

	public void hideInfo() {	
		infoPanelObject.SetActive (false);
	}


}
