using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

	public Sprite hoverImage;
	private Sprite originalImage;
	private SpriteRenderer spriteRdr;
	private GameManager gm;
	private LabItem occupant;
	private bool isDropping = false;


	
	// Use this for initialization
	void Start () {
		
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		spriteRdr = gameObject.GetComponent<SpriteRenderer> ();
		originalImage = spriteRdr.sprite;
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

	void OnMouseEnter() {
		if (gm.heldPiece != null)
			spriteRdr.sprite = hoverImage;
	}

	void OnMouseExit() {
		spriteRdr.sprite = originalImage;
	}

	public void setLabItem(LabItem item) {
		occupant = item;
	}

}
