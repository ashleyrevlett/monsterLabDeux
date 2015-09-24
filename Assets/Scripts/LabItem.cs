using UnityEngine;
using System.Collections;

public class LabItem : MonoBehaviour {

	public string title;
	public string description;
	public float price;
	public bool occupiable;
	private MonsterController occupant = null;
	private bool isPlaced = false; // for tracking when the tile is set down vs in cursor
	private GameObject colliderObject;

	void Start() {

		colliderObject = transform.Find ("ObstacleCollider").gameObject;
		Debug.Log ("colliderObject: " + colliderObject);
		colliderObject.SetActive(false);

	}


	public void setIsPlaced(bool val) {
		isPlaced = val;
		Debug.Log (val);
		Debug.Log (colliderObject);
		if (val == true) {
			colliderObject = transform.Find ("ObstacleCollider").gameObject;
			colliderObject.SetActive (true);
		}
	}

	public bool getIsPlaced() {
		return (isPlaced);
	}

}
