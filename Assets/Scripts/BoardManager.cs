using UnityEngine;
using System;
using System.Collections.Generic; 


public class BoardManager : MonoBehaviour
{
	
	public int columns = 5;                                         //Number of columns in our game board.
	public int rows = 5;                                            //Number of rows in our game board.
	public GameObject tile;                                         //Prefab to spawn for tile bg
	public Transform boardHolder { get; private set; }              //A variable to store a reference to the transform of our Board object.
	public Transform pieceHolder  { get; private set; }             // gameobject parent for gamepieces
	private Transform tileHolder; 									// gameobject parent for bg tiles
	private List <GameObject> pieces = new List <GameObject> ();    // all pieces placed on board


	void Start () {		

		// game object heirarchy:
		// - board
		// -- pieces (game pieces in play)
		// -- tiles (all bg tiles and box colliders)

		boardHolder = new GameObject ("Board").transform;
		tileHolder = new GameObject ("Tiles").transform;
		tileHolder.transform.SetParent (boardHolder);
		pieceHolder = new GameObject ("Pieces").transform;
		pieceHolder.transform.SetParent (boardHolder);
		
		// create bg tiles
		for(int x = 1; x <= columns; x++) { 
			for(int y = 1; y <= rows; y++) {
				GameObject instance = Instantiate (tile, new Vector3 ( x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (tileHolder);
			}
		}
					
		// move camera to center of board
		float boardCenterX = columns / 2f;
		float boardCenterY = rows / 2f;
		Camera.main.transform.position = new Vector3 (boardCenterX, boardCenterY, Camera.main.transform.position.z);

	}

	
	public void placePiece(int row, int column, GameObject newTile) {	
		GameObject instance = Instantiate (newTile, new Vector3 ( row, column, 0f), newTile.transform.localRotation) as GameObject;
		instance.transform.SetParent (pieceHolder);
		pieces.Add (instance);

		// let piece know it's been set down
		LabItem labItem = instance.GetComponent<LabItem> ();
		labItem.setIsPlaced (true);

	}


	public bool isLocationValid(int col, int row, GameObject piece) {

		// space must be on board 
		if (row > rows || col > columns || row < 1 || col < 1) {
			Debug.Log ("Piece off board position");
			return (false);
		}

		// space must be unoccupied
		foreach (GameObject p in pieces) {
			if (p.transform.position.x == col && p.transform.position.y == row) {
				Debug.Log("Piece already at pos");
				return(false);
			}
		}

		// no rule violations found
		Debug.Log("Position for piece valid");
		return(true);
	}



}
