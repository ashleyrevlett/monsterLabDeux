using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class HuntBoardManager : MonoBehaviour {

	public int columns = 5;                                         //Number of columns in our game board.
	public int rows = 5;                                            //Number of rows in our game board.

	public Transform boardHolder { get; private set; }              //A variable to store a reference to the transform of our Board object.
	public Transform pieceHolder  { get; private set; }             // gameobject parent for gamepieces
	private Transform tileHolder; 									// gameobject parent for bg tiles
	
	public GameObject[] grassTiles;                                         //Prefab to spawn for tile bg
	public GameObject wallTile;                                         //Prefab to spawn for tile bg

	public GameObject[] obstacleTiles;
	public int minObstacles;
	public int maxObstacles;
	private int obstaclesCount;
	
	private List <GameObject> pieces;							    // all pieces placed on board
	private List <GameObject> tiles;
	private List <GameObject> walls;
	private List <GameObject> obstacles;

	void Start () {
		
		// game object heirarchy:
		// - board
		// -- pieces (game pieces in)
		// -- tiles (all bg tiles and box colliders)

		boardHolder = new GameObject ("Board").transform;
		tileHolder = new GameObject ("Tiles").transform;
		tileHolder.transform.SetParent (boardHolder);
		pieceHolder = new GameObject ("Pieces").transform;
		pieceHolder.transform.SetParent (boardHolder);
		
		pieces = new List <GameObject> (); 
		tiles = new List<GameObject> ();
		walls = new List<GameObject> ();	

		// create bg tiles
		int offset = 6;
		for(int x = -offset; x <= columns+offset; x++) { 
			for(int y = -offset; y <= rows+offset; y++) {
				if (x <= 0 || x >= columns + offset || y <= 0 || y >= rows + offset) {
					GameObject instance = Instantiate (wallTile, new Vector3 ( x, y, 0f), Quaternion.identity) as GameObject;
					instance.name = "Wall (" + x.ToString() + ", " + y.ToString() + ")";
					instance.transform.SetParent (tileHolder);
					walls.Add(instance);
				} else {
					GameObject randomGrass = grassTiles[(int)Random.Range(0, grassTiles.Length-1)];
					GameObject instance = Instantiate (randomGrass, new Vector3 ( x, y, 0f), Quaternion.identity) as GameObject;
					instance.name = "Tile (" + x.ToString() + ", " + y.ToString() + ")";
					instance.transform.SetParent (tileHolder);
					tiles.Add (instance);
				}
			}
		}

		// add random obstacles to board
		obstaclesCount = (int)Random.Range (minObstacles, maxObstacles);
		obstacles = new List<GameObject> ();
		for (int i = 0; i < obstaclesCount; i++) {
			int rndIdx = Random.Range(0, obstacleTiles.Length-1);
			int rndCol = Random.Range(1, columns);
			int rndRow = Random.Range(1, rows);
			GameObject obs = obstacleTiles[(int)rndIdx];
			GameObject instance = Instantiate (obs, new Vector3 ( rndCol, rndRow, 0f), Quaternion.identity) as GameObject;
			instance.transform.SetParent (pieceHolder);
			obstacles.Add(instance);
		}

		// move camera to center of board
		float boardCenterX = columns / 2f;
		float boardCenterY = rows / 2f;
		Camera.main.transform.position = new Vector3 (boardCenterX, boardCenterY, Camera.main.transform.position.z);

	}


}
