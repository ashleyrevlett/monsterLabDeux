using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildMenuManager : MonoBehaviour {

	private LabItem activeBuildItem;

	public GameObject buildButtonsPanel;
	public GameObject buildButton; // button prefab
	private List<BuildButtonManager> buttonManagers;

	private BoardManager board;
	private GameStateStore gss;

	// Use this for initialization
	void Start () {

		board = GameObject.Find ("LabScene").GetComponent<BoardManager> ();
		gss = GameObject.Find ("GameManager").GetComponent<GameStateStore> ();

		buttonManagers = new List<BuildButtonManager> ();

		// setup build menu
		foreach (GameObject item in gss.labItemPrefabs) {

			LabItem labItem = item.GetComponent<LabItem>();

			GameObject btn = Instantiate (buildButton, new Vector3 ( 0f, 0f, 0f), buildButton.transform.localRotation) as GameObject;
			btn.transform.SetParent(buildButtonsPanel.transform);				
			RectTransform btnPos = btn.GetComponent<RectTransform>();
			btnPos.anchoredPosition = new Vector3(0f, -32f + (-64f*buttonManagers.Count), 0f);

			BuildButtonManager mgr = btn.GetComponent<BuildButtonManager>();
			mgr.title = labItem.title;
			mgr.price = labItem.price;
			buttonManagers.Add (mgr);

		}

		float panelHeight = buttonManagers.Count * 64f;
		buildButtonsPanel.GetComponent<RectTransform> ().sizeDelta = new Vector2(buildButtonsPanel.GetComponent<RectTransform> ().sizeDelta.x, panelHeight);

	}
	
	
	public void setActiveBuildItem(LabItem item) {
		activeBuildItem = item;
		
		// board creates tile piece and moves it when mouse moves
		board.HoldPiece (item.gameObject);
		
	}
	
	public LabItem getActiveBuildItem() {
		return (activeBuildItem);
	}

}
