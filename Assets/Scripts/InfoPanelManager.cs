using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoPanelManager : MonoBehaviour {

	public GameObject buttonPanel;
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

	private MonsterController activeMonster;
	private GameStateStore gss;
	private Button[] buttons;

	void Start () {	
			
		nameTextT = nameText.GetComponent<Text> ();
		ageTextT = ageText.GetComponent<Text> ();
		valueTextT = valueText.GetComponent<Text> ();
		thirstTextT = thirstText.GetComponent<Text> ();
		hungerTextT = hungerText.GetComponent<Text> ();
		healthTextT = healthText.GetComponent<Text> ();
		statsTextT = statsText.GetComponent<Text> ();
		discoveriesTextT = discoveriesText.GetComponent<Text> ();
		activeMonster = null;

		gss = GameObject.Find ("GameManager").GetComponent<GameStateStore> ();

		GameObject obj = GameObject.Find ("InfoPanel");
		if (obj)
			obj.SetActive (false);

		// temp
		discoveriesTextT.text = "";

		// TODO: fix this
		buttons = buttonPanel.GetComponentsInChildren<Button> (true);



	}


	void Update() {
	
		// monster hasn't been created yet
		if (activeMonster == null)
			return;

		if (activeMonster.isAlive == false) {

			// disable buttons
			for (int i = 0; i < buttons.Length; i++) {
				Button b = buttons [i];
				b.interactable = false;
			}

			valueTextT.text = "DEAD";

		} else {

			// enable buttons that have enough resources
			for (int i = 0; i < buttons.Length; i++) {
				Button b = buttons [i];
				if (b.gameObject.name == "FeedButton" && gss.remainingFood <= 0)
					b.interactable = false;
				else if (b.gameObject.name == "WaterButton" && gss.remainingWater <= 0)
					b.interactable = false;
				else if (b.gameObject.name == "HealButton" && gss.remainingMedicine <= 0)
					b.interactable = false;
			}

			nameTextT.text = activeMonster.monsterName;
			ageTextT.text = ((int)(activeMonster.age / gss.daysPerYear)).ToString () + " Years Old / " + activeMonster.monsterSex.ToString ();
			valueTextT.text = "$" + activeMonster.value.ToString ();
			thirstTextT.text = "THIRST: " + activeMonster.thirst.ToString () + "/" + activeMonster.maxThirst.ToString ();
			hungerTextT.text = "HUNGER: " + activeMonster.hunger.ToString () + "/" + activeMonster.maxHunger.ToString ();
			healthTextT.text = "HEALTH: " + activeMonster.health.ToString () + "/" + activeMonster.maxHealth.ToString ();

			string statsOutput = "";
			statsOutput += "Lifespan: " + ((int)(activeMonster.lifespan / gss.daysPerYear)).ToString () + " Years\n";
			statsOutput += "Fertile Age: " + activeMonster.ageOfAdulthood.ToString () + " Days\n";
			statsOutput += "Mating Freq: " + activeMonster.matingFrequency.ToString () + " days\n";
			statsOutput += "Gestation: " + activeMonster.gestation.ToString () + " days \n";
			statsOutput += "Incubation: " + activeMonster.incubation.ToString () + " days \n\n";
			statsOutput += "Strength: " + activeMonster.strength.ToString () + "\n";
			statsOutput += "Agility: " + activeMonster.agility.ToString () + "\n";
			statsOutput += "Intelligence: " + activeMonster.intelligence.ToString () + "\n";
			statsTextT.text = statsOutput;

		}


	}

	public void displayMonster(MonsterController monster) {
		activeMonster = monster;

		if (monster.isAlive) {			
			// re-enable buttons
			for (int i = 0; i < buttons.Length; i++) {
				Button b = buttons [i];
				b.interactable = true;
			}		
		}
	}

}
