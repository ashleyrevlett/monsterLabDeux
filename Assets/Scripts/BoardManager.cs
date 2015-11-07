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
	private List <LabItem> cages;
	private List <TileManager> tiles;
	private List <GameObject> walls;
	private List <MonsterController> monsters; 	// monsters that have been placed on board
	
	public int startingMonsterCount = 1;

	public float dragSpeed = 15f;
	public bool selectingExperiment { get; set; }

	private GameStateStore gss;

	private GameObject infoPanelObject; // for showing monster details
	private InfoPanelManager infoPanel;

	public GameObject heldPiece { get; private set; } // gameobject prefab of piece
	private MonsterController activeMonster; // monster whose info is being shown

	private GameManager gm;
	private AlertPanelManager alert;

	void Start () {		

		// game object heirarchy:
		// - board
		// -- pieces (game pieces in play)
		// -- tiles (all bg tiles and box colliders)


		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		gss = GameObject.Find ("GameManager").GetComponent<GameStateStore> ();
		infoPanelObject = GameObject.Find ("InfoPanel");
		infoPanel = GameObject.Find ("LabGUIPrefab").GetComponent<InfoPanelManager> ();
		alert = GameObject.Find ("LabGUIPrefab").GetComponent<AlertPanelManager> ();

		pieces = new List <GameObject> (); 
		cages = new List <LabItem> (); 
		tiles = new List<TileManager> ();
		walls = new List<GameObject> ();
		monsters = new List<MonsterController> ();

		CreateBoard ();

		alert.ShowAlert ("Welcome to your lab, doctor!\nIt's a bit empty - you should build a cage, then find something to fill it.");
		alert.CreateButton ("Continue", alert.HideAlert);

	}

	void OnEnable() {

		// move camera to center of board
		float boardCenterX = columns / 2f;
		float boardCenterY = rows / 2f;
		Camera.main.transform.position = new Vector3 (boardCenterX, boardCenterY, Camera.main.transform.position.z);

	}

	void OnDisable() {
	
		// TODO: pause execution of monster coroutines so they won't die while we're hunting
		alert.HideAlert ();

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

		// create starting monsters and cages
		LabItem cage = gss.getLabItem ("Cage");
		for (int i = 0; i < startingMonsterCount; i++) {

			GameObject cagePiece = Instantiate (cage.gameObject, Vector3.zero, Quaternion.identity) as GameObject;		 
			cagePiece.transform.SetParent (pieceHolder);
			placeLabPiece(i+1, 2,cagePiece);

			GameObject monster = createNewMonster();
			MonsterController monstercontroller = monster.GetComponent<MonsterController>();
			placeMonsterPiece(i+1, 2, monstercontroller);

			// turn off bc on cage, turn on for monster
			BoxCollider2D bcCage = cagePiece.GetComponent<BoxCollider2D>();
			bcCage.enabled = false;
			BoxCollider2D bcMonster = monster.GetComponent<BoxCollider2D>();
			bcMonster.enabled = true;

		}


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

	
	public bool isMonsterLocationValid(int col, int row) {
		// valid monster location is in unoccupied cage labitem
		foreach (GameObject piece in pieces) {
			LabItem labItem = piece.GetComponent<LabItem>();
			if (labItem != null) {
				if (labItem.occupiable && labItem.occupant == null) {
					if (labItem.gameObject.transform.position.x == col && labItem.gameObject.transform.position.y == row) {
						return true;
					}
				}
			}
		}
		return false;
	}

	
	// Put labitem down on board tile
	public void placeLabPiece(int row, int column, GameObject piece) {	
		
		// snap position to tile pos
		pieces.Add (piece);
		Vector3 newPos = new Vector3 (row, column, 0f);
		piece.transform.position = newPos;
		
		// let piece know it's been set down
		LabItem labItem = piece.GetComponent<LabItem> ();
		labItem.setIsPlaced (true);

		if (labItem.title == "Cage") {
			cages.Add (labItem);
		}
	
	}

	public void placeMonsterPiece(int row, int column, MonsterController monster) {	
		// must be in an empty cage
		LabItem cage = getCage (row, column);
		if (cage != null) {
			if (cage.occupant == null) {
				// snap position to tile pos
				monsters.Add (monster);
				Vector3 newPos = new Vector3 (row, column, 0f);
				monster.gameObject.transform.position = newPos;
				monster.gameObject.transform.parent = GameObject.Find ("LabScene/Board/Pieces").transform;
				cage.placeMonsterInside(monster);
				monster.setIsPlaced (true);
			}
		}	
	}

	public LabItem getCage(int row, int column) {
		foreach (LabItem cage in cages) {
			if (cage.gameObject.transform.position.x == column && cage.gameObject.transform.position.y == column) 
				return cage;
		}
		return null;
	}

	public TileManager getTile(int row, int column) {	
		foreach (TileManager tile in tiles) {
			if (tile.gameObject.transform.position.x == column && tile.gameObject.transform.position.y == row)
				return tile;					
		}
		return null;
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
				placeLabPiece ((int)position.x, (int)position.y, heldPiece);
				Debug.Log ("Money deducted");
			} else {
				// destroy if we can't place it
				Destroy (heldPiece); // don't need this gameobject anymore
			}			
			// no longer holding it either way		
			heldPiece = null;
		} else if (heldPiece.tag == "Monster") {
			Debug.Log("monster wants to drop");
			if (isMonsterLocationValid ((int)position.x, (int)position.y)) {
				Debug.Log("monster location valud");
				MonsterController monster = heldPiece.GetComponent<MonsterController>();
				placeMonsterPiece ((int)position.x, (int)position.y, monster);
				Debug.Log ("Monster placed");
				heldPiece = null;
			} else {
				Debug.Log("monster position not valid");
			}
			// do not destroy monster; hold it until we place it in the right spot
		}
		
	}

	public Vector3 getValidMonsterPosition() {

		foreach (LabItem cage in cages) {
			if (cage.occupant == null) {
				return cage.gameObject.transform.position;
			}
		}

		return Vector3.zero;

	}

	
	public GameObject createNewMonster(GameObject monsterPrefab) {
		hideInfo (); // in case we're viewing info panel, close it
		GameObject instance = Instantiate (monsterPrefab, new Vector3 ( 1f, 1f, 0f), Quaternion.identity) as GameObject;
		instance.transform.SetParent (pieceHolder);
		monsters.Add (instance.GetComponent<MonsterController> ());
		return instance;
	}

	public GameObject createNewMonster() {
		// TODO: hook up hunting scene
		hideInfo (); // in case we're viewing info panel, close it
		GameObject instance = Instantiate (gss.monsterPrefabs[0], new Vector3 ( 1f, 1f, 0f), Quaternion.identity) as GameObject;
		instance.transform.SetParent (pieceHolder);
		monsters.Add (instance.GetComponent<MonsterController> ());
		return instance;
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

	public int GetEmptyCageCount() {
		int num = 0;
		foreach (GameObject obj in pieces) {
			LabItem labItem = obj.GetComponent<LabItem>();
			if (labItem != null)
				if (labItem.title == "Cage" && labItem.occupant == null)
					num++;
		}
		Debug.Log ("Empty cages: " + num.ToString ());
		return num;
	}

	public void ShowHuntAlert() {
		if (GetEmptyCageCount () <= 0) {
			alert.ShowAlert ("You need to build a cage before hunting!\nWhere else do you plan on keeping the monsters you trap?");
			alert.CreateButton ("OK", alert.HideAlert);
		} else {
			alert.ShowAlert ("Are you sure you want to go hunting?");
			alert.CreateButton ("Go Hunt", LoadHuntScene);
			alert.CreateButton ("Cancel", alert.HideAlert);
		}
	}

	public void LoadHuntScene() {
		gm.LoadHuntScene ();
	}

}
