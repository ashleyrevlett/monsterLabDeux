using UnityEngine;
using System.Collections;

public class LabItem : MonoBehaviour {

	public string title;
	public string description;
	public float price;
	public bool occupiable;
	private bool isPlaced = false; // for tracking when the tile is set down vs in cursor
	private BoxCollider2D clickCollider;
	private GameManager gm;


	void Start() {
		clickCollider = gameObject.GetComponent<BoxCollider2D> ();
		clickCollider.enabled = false;
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	public void setIsPlaced(bool val) {
		isPlaced = val;
		Debug.Log (val);
		clickCollider.enabled = true;
	}

	public bool getIsPlaced() {
		return (isPlaced);
	}

	void OnMouseDown() {
		gm.DropPiece (transform.position);
	}

}
