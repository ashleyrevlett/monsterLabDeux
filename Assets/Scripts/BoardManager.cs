using UnityEngine;
using System;
using System.Collections.Generic; 


public class BoardManager : MonoBehaviour
{
	
	public int columns = 5;                                         //Number of columns in our game board.
	public int rows = 5;                                            //Number of rows in our game board.
	public GameObject tile;                                         //Prefab to spawn for tile bg
	public GameObject wall;                                         //Prefab to spawn for tile bg
	public Transform boardHolder { get; private set; }              //A variable to store a reference to the transform of our Board object.
	public Transform pieceHolder  { get; private set; }             // gameobject parent for gamepieces
	private Transform tileHolder; 									// gameobject parent for bg tiles
	private List <GameObject> pieces;							    // all pieces placed on board
	private List <TileManager> tiles;
	private List <GameObject> walls;
	private List <GameObject> monsters;

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

		pieces = new List <GameObject> (); 
		tiles = new List<TileManager> ();
		walls = new List<GameObject> ();
		monsters = new List<GameObject> ();

		// create bg tiles
		for(int x = 0; x <= columns+1; x++) { 
			for(int y = 0; y <= rows+1; y++) {
				if (x == 0 || x == columns + 1 || y == 0 || y == rows + 1) {
					GameObject instance = Instantiate (wall, new Vector3 ( x, y, 0f), Quaternion.identity) as GameObject;
					instance.name = "Wall (" + x.ToString() + ", " + y.ToString() + ")";
					instance.transform.SetParent (tileHolder);
					walls.Add(instance);
				} else {
					GameObject instance = Instantiate (tile, new Vector3 ( x, y, 0f), Quaternion.identity) as GameObject;
					instance.name = "Tile (" + x.ToString() + ", " + y.ToString() + ")";
					instance.transform.SetParent (tileHolder);
					tiles.Add (instance.GetComponent<TileManager>());
				}
			}
		}
					
		// move camera to center of board
		float boardCenterX = columns / 2f;
		float boardCenterY = rows / 2f;
		Camera.main.transform.position = new Vector3 (boardCenterX, boardCenterY, Camera.main.transform.position.z);

	}

	
	public void placePiece(int row, int column, GameObject piece) {	

		// snap position to tile pos
		pieces.Add (piece);
		Vector3 newPos = new Vector3 (row, column, 0f);
		piece.transform.position = newPos;

		// let piece know it's been set down
		LabItem labItem = piece.GetComponent<LabItem> ();
		labItem.setIsPlaced (true);

		// notify tile of its new occupant
		TileManager tile = getTile (row, column);
		tile.setLabItem (labItem);

	}
	
	public bool isLocationValid(int col, int row, GameObject piece) {

		// space must be on board 
		if (row > rows || col > columns || row < 1 || col < 1) {
			Debug.Log ("Piece off board position");
			return (false);
		}

		// space must be unoccupied
		foreach (GameObject p in pieces) {
			if (p != null) {
				if (p.transform.position.x == col && p.transform.position.y == row) {
					Debug.Log("Piece already at pos");
					return(false);
				}
			}
		}

		// no rule violations found
		Debug.Log("Position for piece valid");
		return(true);
	}

	public void placeMonsterPiece(int row, int column, GameObject piece) {	

		// snap position to tile pos
		monsters.Add (piece);
		Vector3 newPos = new Vector3 (row, column, 0f);
		piece.transform.position = newPos;
		
		// let piece know it's been set down
		MonsterController m = piece.GetComponent<MonsterController> ();
		m.setIsPlaced (true);

		// notify tile of its new occupant
		TileManager tile = getTile (row, column);
		tile.setMonster (m);

	}
	
	public bool isMonsterLocationValid(int col, int row, GameObject piece) {

		TileManager tile = getTile (row, col);
		LabItem item = tile.getOccupant ();

		if (item == null) // empty tile
			return true;

		// cage (occupiable=true)
		if (tile.getOccupant ().occupiable && tile.getMonster() == null)
			return true;
		else 
			return false;

	}

	public TileManager getTile(int row, int column) {

		TileManager theTile = null;

		foreach (TileManager tile in tiles) {
			if (tile.gameObject.transform.position.x == column && tile.gameObject.transform.position.y == row) {
				theTile = tile;		
			}
		}

		return theTile;
	
	}


}
