using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeaderMenuManager : MonoBehaviour {
	
	public GameObject dayText;
	public GameObject levelText;
	public GameObject monsterText;
	public GameObject resourcesText;
	public GameObject moneyText;

	private Text dayTextT;
	private Text levelTextT;
	private Text monsterTextT;
	private Text resourcesTextT;
	private Text moneyTextT;

	void Start () {	
		dayTextT = dayText.GetComponent<Text> ();
		levelTextT = levelText.GetComponent<Text> ();
		monsterTextT = monsterText.GetComponent<Text> ();
		resourcesTextT = resourcesText.GetComponent<Text> ();
		moneyTextT = moneyText.GetComponent<Text> ();
	}

	public void UpdateDisplay (int daysElapsed, int currentLevel, int monsters, float water, float food, float medicine, float money) {
		// updateDisplay is invoked from gamestatestore when data is updated
		dayTextT.text = "DAY " + daysElapsed.ToString ();
		levelTextT.text = "LEVEL " + currentLevel.ToString ();
		monsterTextT.text = monsters.ToString () + " MONSTERS";
		resourcesTextT.text = water.ToString () + " WATER    " + food.ToString () + " FOOD    " + medicine.ToString () + " MEDS";
		moneyTextT.text = "$" + money.ToString ();
	}

}
