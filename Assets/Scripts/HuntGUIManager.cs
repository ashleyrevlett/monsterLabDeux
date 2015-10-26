using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HuntGUIManager : MonoBehaviour {

	public Text dartsText;
	private GameStateStore gss;

	// Use this for initialization
	void Start () {
		gss = GameObject.Find ("GameManager").GetComponent<GameStateStore> ();	
	}
	
	// Update is called once per frame
	
	void Update () {
		dartsText.text = gss.remainingAmmo.ToString() + " AMMO";
	}

}
