using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

	public Sprite hoverImage;
	private Sprite originalImage;
	private SpriteRenderer spriteRdr;
	private GameManager gm;
	private LabItem occupant;
	private MonsterController monster;
	private bool isDropping = false;


	// Use this for initialization
	void Start () {		
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		spriteRdr = gameObject.GetComponent<SpriteRenderer> ();
		originalImage = spriteRdr.sprite;
	}

	public LabItem getOccupant() {
		return occupant;
	}

	public void setLabItem(LabItem item) {
		occupant = item;
	}

	public MonsterController getMonster () {
		return monster;
	}

	public void setMonster ( MonsterController m) {
		monster = m;
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

}
