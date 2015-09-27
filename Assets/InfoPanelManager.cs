using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoPanelManager : MonoBehaviour {

	public GameObject nameText;
	public GameObject ageText;
	public GameObject valueText;
	public GameObject thirstText;
	public GameObject hungerText;
	public GameObject healthText;
	public GameObject statsText;
	public GameObject discoveriesText;

	private Text nameTextT;
	private Text ageTextT;
	private Text valueTextT;
	private Text thirstTextT;
	private Text hungerTextT;
	private Text healthTextT;
	private Text statsTextT;
	private Text discoveriesTextT;

	void Start () {	
		nameTextT = nameText.GetComponent<Text> ();
		ageTextT = ageText.GetComponent<Text> ();
		valueTextT = valueText.GetComponent<Text> ();
		thirstTextT = thirstText.GetComponent<Text> ();
		hungerTextT = hungerText.GetComponent<Text> ();
		healthTextT = healthText.GetComponent<Text> ();
		statsTextT = statsText.GetComponent<Text> ();
		discoveriesTextT = discoveriesText.GetComponent<Text> ();

	}


	public void displayMonster(MonsterController monster) {
	
		thirstTextT.text = "THIRST!!!";


		Debug.Log ("Monster!");
	}

}
