using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateStore : MonoBehaviour {

	public GameObject[] labItemPrefabs;
	private LabItem activeBuildItem;

	private GameManager gm;

	void Start() {
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}


	public void setActiveBuildItem(LabItem item) {
		activeBuildItem = item;
		Debug.Log ("Build item store updated!!!");

		// trigger gm to create tile piece and move it when mouse moves
		gm.HoldPiece (item.gameObject);

	}

	public LabItem getActiveBuildItem() {
		return (activeBuildItem);
	}

	public LabItem getLabItem(string title) {

		for (int i = 0; i < this.labItemPrefabs.Length; i++) {
			if (this.labItemPrefabs[i].GetComponent<LabItem>().title == title)
				return (this.labItemPrefabs[i].GetComponent<LabItem>());
		}

		return null;

	}

}
