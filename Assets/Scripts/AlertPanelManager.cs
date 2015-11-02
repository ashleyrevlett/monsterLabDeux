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
//
//	public void AddButton(string text) {
//		okBtn.GetComponentInChildren<Text>().text = text;
//		okBtn.onClick.RemoveAllListeners ();
//		// have to add listener from calling object //okBtn.onClick.AddListener (callback);
//	}

	public void CreateButton(string text, MyDelegate theDelegate) {
		float buttonWidth = 30f;
		float buttonPadding = 5f;
		GameObject instance = Instantiate (btn, Vector2.zero, Quaternion.identity) as GameObject;
		instance.name = text + " Button";
		instance.transform.SetParent (alertPanel.transform);
		instance.GetComponentInChildren<Text>().text = text;
		RectTransform rect = instance.GetComponentInChildren<RectTransform> ();
		rect.anchoredPosition = new Vector2(0.5f, 0);
		rect.pivot = new Vector2 (0.5f, 0.5f);
		rect.position = new Vector3 (rect.position.x + (buttons.Count * 100f), rect.position.y + 30f, rect.position.z);
		Button thisBtn = instance.GetComponent<Button> ();
		thisBtn.onClick.RemoveAllListeners ();
		thisBtn.onClick.AddListener(delegate { theDelegate(); });

		buttons.Add(instance);

	}

}
