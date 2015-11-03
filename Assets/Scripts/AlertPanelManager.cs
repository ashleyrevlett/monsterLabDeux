using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class AlertPanelManager : MonoBehaviour {

	private GameObject alertPanel;
	public Text alertText;
	public GameObject btn;
	public List<GameObject> buttons;

	public delegate void MyDelegate();
	public MyDelegate myDelegate;
	
	void Awake () {	
		alertPanel = GameObject.Find ("AlertPanel");
		buttons = new List<GameObject> ();
	}


	public void ShowAlert(string msg) {
		alertPanel.SetActive (true);
		alertText.text = msg;
	}

	public void HideAlert() {
		Debug.Log ("Hiding alert!");
		foreach (GameObject btn in buttons) {
			Destroy (btn);
		}
		buttons.Clear ();
		alertPanel.SetActive (false);	
	}

	public void CreateButton(string text, MyDelegate theDelegate) {
		Debug.Log ("Creating button!");
		GameObject instance = Instantiate (btn, Vector2.zero, Quaternion.identity) as GameObject;
		instance.name = text + " Button";
		GameObject parent = GameObject.Find ("AlertButtonContainer");
		instance.transform.SetParent (parent.transform);
		instance.GetComponentInChildren<Text>().text = text;
		Button thisBtn = instance.GetComponent<Button> ();
		thisBtn.onClick.RemoveAllListeners ();
		thisBtn.onClick.AddListener(delegate { theDelegate(); });
		buttons.Add(instance);
	}

}
