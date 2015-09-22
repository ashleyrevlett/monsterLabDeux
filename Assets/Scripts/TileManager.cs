using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {
	
	private GameManager gm;
	private LabItem occupant;
	private bool isDropping = false;


	
	// Use this for initialization
	void Start () {
		
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		if (!isDropping) {
			isDropping = true;
			Debug.Log ("Mouse down on tile");
			Debug.Log ("Pos: " + transform.position);
			gm.DropPiece (transform.position);
		}
	}

	public void setLabItem(LabItem item) {
		occupant = item;
	}

}
