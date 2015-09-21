using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {

	public GameObject buildButtonsPanel;
	public GameObject buildButton;
	private List<BuildButtonManager> buttonManagers;

	private GameManager gm;
	private GameStateStore gss;

	// Use this for initialization
	void Start () {

		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		gss = GameObject.Find ("GameManager").GetComponent<GameStateStore> ();
		buttonManagers = new List<BuildButtonManager> ();

		// setup build menu
		for (int i = 0; i < gss.labItemPrefabs.Length; i++) {

			LabItem labItem = gss.labItemPrefabs[i].GetComponent<LabItem>();

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
	

	void Update () {

		// if there is an active build button
	
	}
}
