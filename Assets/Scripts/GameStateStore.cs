using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameStateStore : MonoBehaviour {

	public GameObject[] monsterPrefabs;
	public List<MonsterController> monsters { get; private set; }

	public GameObject[] labItemPrefabs;
	public List<LabItem> labItems { get; private set; } // labItem script references

	public float startingMoney;
	public float remainingMoney { get; private set; }

	public float startingWater;
	public float remainingWater { get; private set; }
	
	public float startingFood;
	public float remainingFood { get; private set; }	
	
	public float startingMedicine;
	public float remainingMedicine { get; private set; }	
	
	public float startingAmmo;
	public float remainingAmmo { get; private set; }	

	public string nameStrings = "Ancalagon, Anfauglir, Annatar, Artano, Aulendil, Azog, Balchoth, Bauglir, Belcha, Belegor, Belegûr, Belegurth, Bert, Bill, Bolg, Candle, Carcharoth, Cave-troll, Cold-drake, Corsair, Curumo, Curunír, Draugluin, Durin´s Bane, Dwimmerlaik, Fire-drake, Flame of Udûn, Flie, Fluithuin, Fuinur, Gaurhoth, Golfimbul, Glamhoth, Glaurung, Glorund, Gorbag, Gorgûn, Gorthaur, Gothmog, Grishnák, Half-orc, Half-troll, Herumor, Hill-troll, Hobgoblin, Jaws of Thirst, Khamûl, Kraken, Lagduf, Lieut. o, Lieut, Long-worm, Mauhûr, Melegor, Melkor, Mountain-troll, Mouth of Sauron, Mûmak, Mûmakil, Muzgash, Necromancer, Oathbreakers, Oikeroi, Ol, Orch, Paths o, Radbug, Red Maw, Scatha, Shadow Host, Shagrat, Sharkey, Slinker, Sméagol, Snaga, Sneak, Snow-troll, Southron, Stinker, Stone-troll, Swarthy Men, Swertings, Thuringwethil (se, Tiberth, Tifil, Tom, Torog, Trahald, Two-headed troll, Ufthak, Uglûk, Úlairi, Ulbandi, Uldor, Ulfang, Ulfast, Ulworth, Umuiyan, Urulóki, Valaraukar, War, Watcher i, Were-worm, William, Willow (se, Witch-kin,  Wolf, Worm, Wormtongue, Yrch"; 
	public List<string> names { get; private set; }


	public int daysPerYear = 10;
	public int secsPerDay = 300;
	public int daysElapsed { get; private set; }
	public float secsElapsed { get; private set; }
	
	private GameManager gm;


	void Start() {

		names = new List<string>(nameStrings.Split (','));

		daysElapsed = 0;
		secsElapsed = 0f;
		remainingMoney = startingMoney;
		remainingFood = startingFood;
		remainingMedicine = startingMedicine;
		remainingWater = startingWater;
		remainingAmmo = startingAmmo;

		labItems = new List<LabItem> ();
		foreach (GameObject item in labItemPrefabs) {
			LabItem labitem = item.GetComponent<LabItem>();
			labItems.Add(labitem);
		}

		monsters = new List<MonsterController> ();		
		foreach (GameObject item in monsterPrefabs) {
			MonsterController monster = item.GetComponent<MonsterController>();
			monsters.Add(monster);
		}

	}

	void Update() {	

		if (secsElapsed >= secsPerDay) {
			daysElapsed += 1;
			secsElapsed = 0;
		}

		secsElapsed += Time.deltaTime;
	
	}

	public void addFood(float amount) {
		remainingFood += amount;
	}
	
	public void addWater(float amount) {
		remainingWater += amount;
	}
	
	public void addMedicine(float amount) {
		remainingMedicine += amount;

	}

	public void addMoney(float amount) {
		remainingMoney += amount;
	}

	public bool deductFood(float amount) {
		// return true if success, false if failure

		if (remainingFood <= 0)
			return false;

		remainingFood = Mathf.Max (0, remainingFood - amount);

		return true;
	}

	public bool deductWater(float amount) {
		if (remainingWater <= 0)
			return false;
		remainingWater = Mathf.Max (0, remainingWater - amount);

		return true;
	}
	
	public bool deductMedicine(float amount) {
		if (remainingMedicine <= 0)
			return false;
		remainingMedicine = Mathf.Max (0, remainingMedicine - amount);


		return true;
	}
	
	public bool deductMoney(float amount) {
		if (remainingMoney <= 0)
			return false;
		remainingMoney = Mathf.Max (0, remainingMoney - amount);

		return true;
	}
		
	public LabItem getLabItem(string title) {
		foreach (LabItem labItem in labItems) {
			if (labItem.title == title)
				return (labItem);
		}
		return null;
	}

	public GameObject getRandomMonster() {
		return monsterPrefabs [0];
	}

	public string getRandomName() {	
		int rndIdx = Random.Range (0, (names.Count - 1));
		return names [rndIdx];
	}


}
