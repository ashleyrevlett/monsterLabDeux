using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic; 
using System.Collections;

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

	public GameObject[] monsterObjects;
	private MonsterController[] monsterControllers;

	public float dragSpeed = 15f;
	public bool selectingExperiment { get; set; }

	private GameStateStore gss;

	private GameObject infoPanelObject; // for showing monster details
	private InfoPanelManager infoPanel;

	public GameObject heldPiece { get; private set; } // gameobject prefab of piece
	private MonsterController activeMonster; // monster whose info is being shown

	private GameManager gm;

	void Start () {		

		// game object heirarchy:
		// - board
		// -- pieces (game pieces in play)
		// -- tiles (all bg tiles and box colliders)


		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		gss = GameObject.Find ("GameManager").GetComponent<GameStateStore> ();
		infoPanelObject = GameObject.Find ("InfoPanel");
		infoPanel = GameObject.Find ("LabGUIPrefab").GetComponent<InfoPanelManager> ();

		pieces = new List <GameObject> (); 
		tiles = new List<TileManager> ();
		walls = new List<GameObject> ();
		monsters = new List<GameObject> ();

		CreateBoard ();

		// move camera to center of board
		float boardCenterX = columns / 2f;
		float boardCenterY = rows / 2f;
		Camera.main.transform.position = new Vector3 (boardCenterX, boardCenterY, Camera.main.transform.position.z);

	}

	void Update() {
	
		// update position of held piece
		if (heldPiece != null) {		
			var posVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			posVec.z = 0f;
			heldPiece.transform.position = Vector3.MoveTowards(heldPiece.transform.position, posVec, dragSpeed * Time.deltaTime);			
							
		}

	}


	private void CreateBoard() {

		boardHolder = new GameObject ("Board").transform;
		boardHolder.SetParent (gameObject.transform);
		tileHolder = new GameObject ("Tiles").transform;
		tileHolder.transform.SetParent (boardHolder);
		pieceHolder = new GameObject ("Pieces").transform;
		pieceHolder.transform.SetParent (boardHolder);
		
		// create wall and ground tiles
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

	}


	// Put labitem down on board tile
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
		else
			Debug.Log ("occupied");

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

	// pick up a game piece
	public void HoldPiece(GameObject piece) {
		hideInfo (); // in case we're viewing info panel, close it
		float price = piece.GetComponent<LabItem>().price;
		if (gss.remainingMoney >= price) {
			var posVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			posVec.z = 0f;
			heldPiece = Instantiate (piece, posVec, Quaternion.identity) as GameObject;		 
			heldPiece.transform.SetParent (pieceHolder);
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
			if (isLocationValid ((int)position.x, (int)position.y, heldPiece)) {
				float price = heldPiece.GetComponent<LabItem> ().price;
				gss.deductMoney (price);
				placePiece ((int)position.x, (int)position.y, heldPiece);
				Debug.Log ("Money deducted");
			} else {
				// destroy if we can't place it
				Destroy (heldPiece); // don't need this gameobject anymore
			}			
			// no longer holding it either way		
			heldPiece = null;
		} else if (heldPiece.tag == "Monster") {
			Debug.Log("monster wants to drop");
			if (isMonsterLocationValid ((int)position.x, (int)position.y, heldPiece)) {
				Debug.Log("monster location valud");
				placeMonsterPiece ((int)position.x, (int)position.y, heldPiece);
				Debug.Log ("Monster placed");
				heldPiece = null;
			} else {
				Debug.Log("monster position not valid");
			}
			// do not destroy monster; hold it until we place it in the right spot
		}
		
	}
	
	
	public void createNewMonster() {
		// TODO: hook up hunting scene
		hideInfo (); // in case we're viewing info panel, close it
		GameObject instance = Instantiate (monsterObjects[0], new Vector3 ( 1f, 1f, 0f), Quaternion.identity) as GameObject;
		instance.transform.SetParent (pieceHolder);
		heldPiece = instance;
		
	}
	
	// show the info panel with the monster's live info
	public void showInfo(MonsterController monster) {	
		// if holding a piece, do not activate infopanel
		if (heldPiece == null) {
			infoPanelObject.SetActive (true);
			infoPanel.displayMonster (monster);	
			activeMonster = monster;
		}
	}
	
	public void hideInfo() {	
		infoPanelObject.SetActive (false);
		if (activeMonster != null) {
			activeMonster.Deselect ();
			activeMonster = null;
			selectingExperiment = false;
		}
	}
	
	// apply actions to active monster (called from buttons)
	public void Water() {
		if (gss.remainingWater >= 1) {
			activeMonster.Water ();
			gss.deductWater (1f);
		} else {
			Debug.Log ("No water remaining");
		}
	}
	
	public void Feed() {
		if (gss.remainingFood >= 1) {
			activeMonster.Feed ();
			gss.deductFood(1f);
		} else {
			Debug.Log ("No food remaining");
		}
	}
	
	public void Heal() {
		if (gss.remainingMedicine >= 1) {
			activeMonster.Heal ();
			gss.deductMedicine (1f);
		} else {
			Debug.Log ("No meds remaining");
		}
	}

	public void Sell() {			
		// TODO implement 
		Debug.Log ("Selling");	
	}

	// when the monster's experiment button has pressed and we need to 
	// choose the lab table or other experiment location
	public void SelectExperiment() {
		selectingExperiment = true;
		// TODO show alert
	}
	
	public void DoExperiment(float damagePerTick, float experimentTime, Vector3 position) {
		Debug.Log ("Doing experiment in GM");
		selectingExperiment = false;
		// TODO show alert
		activeMonster.transform.position = position;
		activeMonster.Experiment (damagePerTick, experimentTime);
		
	}
	
	public int getMonsterCount() {
		return monsters.Count;		
	}

	public void LoadHuntScene() {
		gm.LoadHuntScene ();
	}

}
