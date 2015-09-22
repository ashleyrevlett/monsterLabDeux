using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public float dragSpeed = 15f;
	private GameObject heldPiece; // gameobject prefab of piece
	private BoardManager boardManager;
	private GameStateStore gss;

	void Start () {
		GameObject gm = GameObject.Find ("GameManager");
		boardManager = gm.GetComponent<BoardManager> ();
		gss = gm.GetComponent<GameStateStore> ();
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
		var posVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		posVec.z = 0f;
		heldPiece = Instantiate (piece, posVec, Quaternion.identity) as GameObject;		 
		heldPiece.transform.SetParent (boardManager.pieceHolder);
	}


	// drop a game piece
	public void DropPiece(Vector3 position) {

		Debug.Log ("Dropping piece from gm");

		if (heldPiece == null)
			return;
	
		if (boardManager.isLocationValid ((int)position.x, (int)position.y, heldPiece)) {
			float price = heldPiece.GetComponent<LabItem>().price;
			if (gss.getRemainingMoney() >= price) {
				gss.deductMoney(price);
				boardManager.placePiece ((int)position.x, (int)position.y, heldPiece);
				Debug.Log("Money deducted");
			} else {
				Debug.Log("Not enough money to place");
			}

			// drop it either way
			Destroy(heldPiece); // don't need this gameobject anymore
			heldPiece = null;

		}
	}

}
