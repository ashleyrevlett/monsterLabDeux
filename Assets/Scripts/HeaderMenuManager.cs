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

	private GameManager gm;
	private GameStateStore gss;
	private BoardManager board;

	void Start () {	
		dayTextT = dayText.GetComponent<Text> ();
		levelTextT = levelText.GetComponent<Text> ();
		monsterTextT = monsterText.GetComponent<Text> ();
		resourcesTextT = resourcesText.GetComponent<Text> ();
		moneyTextT = moneyText.GetComponent<Text> ();

		gss = GameObject.Find ("GameManager").GetComponent<GameStateStore> ();
		board = GameObject.Find ("LabScene").GetComponent<BoardManager> ();
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	void Update() {
	
		dayTextT.text = "DAY " + gss.daysElapsed.ToString ();
		levelTextT.text = "LEVEL " + gm.currentLevel.ToString();
		monsterTextT.text = board.getMonsterCount ().ToString () + " MONSTERS";
		resourcesTextT.text = gss.remainingWater.ToString() + " WATER    " + gss.remainingFood.ToString () + " FOOD    " + gss.remainingMedicine.ToString () + " MEDS";
		moneyTextT.text = "$" + gss.remainingMoney.ToString ();
	}
}
