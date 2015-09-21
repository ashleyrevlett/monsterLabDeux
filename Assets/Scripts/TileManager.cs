using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {
	
	private GameManager gm;
	
	// Use this for initialization
	void Start () {
		
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
//		Debug.Log ("Mouse down on tile");
//		Debug.Log ("Pos: " + transform.position);
		gm.DropPiece (transform.position);
	}

}
