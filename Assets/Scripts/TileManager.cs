using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

	public Sprite hoverImage;
	private Sprite originalImage;
	private SpriteRenderer spriteRdr;
	private bool isDropping = false;
	private BoardManager board;


	// Use this for initialization
	void Start () {		
		board = GameObject.Find ("LabScene").GetComponent<BoardManager> ();
		spriteRdr = gameObject.GetComponent<SpriteRenderer> ();
		originalImage = spriteRdr.sprite;
	}

	void OnMouseDown() {
		board.hideInfo (); // in case we're viewing info panel, close it
		if (!isDropping) {
			isDropping = true;
			Debug.Log ("Mouse down on tile");
			Debug.Log ("Pos: " + transform.position);
			board.DropPiece (transform.position);
		}
	}

	void OnMouseEnter() {
		if (board.heldPiece != null)
			spriteRdr.sprite = hoverImage;
	}

	void OnMouseExit() {
		spriteRdr.sprite = originalImage;
	}

}
