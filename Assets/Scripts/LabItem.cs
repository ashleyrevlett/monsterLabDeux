using UnityEngine;
using System.Collections;

public class LabItem : MonoBehaviour {

	public string title;
	public string description;
	public float price;
	public bool occupiable;
	public MonsterController occupant { set; get; }
	private bool isPlaced = false; // for tracking when the tile is set down vs in cursor
	private BoxCollider2D clickCollider;
	private GameManager gm;

	public bool experimentLocation;
	public float damagePerTick;
	public float experimentTime; // seconds exp lasts


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

		if (gm.heldPiece != null)
			gm.DropPiece (transform.position);

		if (gm.selectingExperiment == true)
			gm.DoExperiment (damagePerTick, experimentTime, gameObject.transform.position);

		Debug.Log ("Mouse down on lab item");
	}

}
