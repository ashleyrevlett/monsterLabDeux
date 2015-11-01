using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameStateStore : MonoBehaviour {

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


	public int daysPerYear = 10;
	public int secsPerDay = 300;
	public int daysElapsed { get; private set; }
	public float secsElapsed { get; private set; }
	
	private GameManager gm;


	void Start() {

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



}
