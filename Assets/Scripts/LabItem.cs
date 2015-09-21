using UnityEngine;
using System.Collections;

public class LabItem : MonoBehaviour {

	public string title;
	public string description;
	public float price;
	public bool occupiable;
	private MonsterController occupant = null;
	private bool isPlaced = false; // for tracking when the tile is set down


	public void setIsPlaced(bool val) {
		isPlaced = val;
	}

	public bool getIsPlaced() {
		return (isPlaced);
	}

}
