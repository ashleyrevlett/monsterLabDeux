using UnityEngine;
using System.Collections;

public class GameStateStore : MonoBehaviour {

	[System.Serializable]
	public class LabItem
	{
		public string Title;
		public float Price;
		public GameObject TilePrefab;
		public bool Available;
	}

	public LabItem[] labItems;
	private LabItem activeBuildItem;

	private GameManager gm;

	void Start() {
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}


	public void setActiveBuildItem(LabItem item) {
		activeBuildItem = item;
		Debug.Log ("Build item store updated!!!");

		// trigger gm to create tile piece and move it when mouse moves
		gm.HoldPiece (item.TilePrefab);

	}

	public LabItem getActiveBuildItem() {
		return (activeBuildItem);
	}

	public LabItem getLabItem(string title) {

		for (int i = 0; i < this.labItems.Length; i++) {
			if (this.labItems[i].Title == title)
				return (this.labItems[i]);
		}

		return null;

	}

}
