using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildMenuManager : MonoBehaviour {

	/* 
	 * Get list of labItems from gss.
	 * Create button for each labItem in buildButton UI panel. 
	 * On buttonClick, set the gm's heldPiece to the button's labItem.
	 * */
	
	public GameObject buildButtonsPanel;
	public GameObject buildButton; // button prefab
	public float buttonSize = 64f;
	private List<BuildButtonManager> buttonManagers;
	private GameStateStore gss;
	
	void Awake () {
		gss = GameObject.Find ("GameManager").GetComponent<GameStateStore> ();
		buttonManagers = new List<BuildButtonManager> ();
	}

	void OnEnable() {

		// clear all previously created buttons
		foreach (BuildButtonManager btn in buttonManagers) {
			Destroy (btn.gameObject);
		}
		buttonManagers.Clear ();

		foreach (GameObject item in gss.labItemPrefabs) {
			
			GameObject btn = Instantiate (buildButton, new Vector3 ( 0f, 0f, 0f), buildButton.transform.localRotation) as GameObject;
			btn.transform.SetParent(buildButtonsPanel.transform);				
			RectTransform btnPos = btn.GetComponent<RectTransform>();
			btnPos.anchoredPosition = new Vector3(0f, -(buttonSize*.5f) + (-buttonSize*buttonManagers.Count), 0f);
			
			BuildButtonManager mgr = btn.GetComponent<BuildButtonManager>();
			LabItem labItem = item.GetComponent<LabItem>();
			mgr.title = labItem.title;
			mgr.price = labItem.price;
			buttonManagers.Add (mgr);
			
		}
		
		float panelHeight = buttonManagers.Count * buttonSize;
		buildButtonsPanel.GetComponent<RectTransform> ().sizeDelta = new Vector2(buildButtonsPanel.GetComponent<RectTransform> ().sizeDelta.x, panelHeight);

	}

	public void RemoveButton(string monsterName) {
		// TODO find btn by monster name, destroy it
	}


}
