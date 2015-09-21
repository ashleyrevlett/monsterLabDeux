using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public float dragSpeed = 15f;
	private GameObject heldPiece; // gameobject prefab of piece
	private BoardManager boardManager;

	void Start () {
		boardManager = GameObject.Find ("GameManager").GetComponent<BoardManager> ();
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

		if (heldPiece == null)
			return;
	
		if (boardManager.isLocationValid ((int)position.x, (int)position.y, heldPiece)) {
								
			boardManager.placePiece ((int)position.x, (int)position.y, heldPiece);

			Destroy(heldPiece); // don't need this gameobject anymore

			heldPiece = null;

		}
	}

}
