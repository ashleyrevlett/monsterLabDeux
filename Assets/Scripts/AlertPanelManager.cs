using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlertPanelManager : MonoBehaviour {

	public GameObject alertPanel;
	public Text alertText;
	public Button okBtn;

	
	// Update is called once per frame
	void Update () {	
	}

	public void ShowAlert(string msg) {
		alertPanel.SetActive (true);
		alertText.text = msg;
	}

	public void HideAlert() {
		Debug.Log ("Hiding alert!");
		alertPanel.SetActive (false);	
	}

	public void AddButton(string text) {
		okBtn.GetComponentInChildren<Text>().text = text;
		okBtn.onClick.RemoveAllListeners ();
		// have to add listener from calling object //okBtn.onClick.AddListener (callback);
	}


}
