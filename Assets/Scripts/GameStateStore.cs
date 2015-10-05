using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameStateStore : MonoBehaviour {

	public int currentLevel = 1;
	public bool isPaused;

	public GameObject[] labItemPrefabs;
	private LabItem activeBuildItem;
	
	public float startingMoney;
	private float remainingMoney;

	public float startingWater;
	public float remainingWater { get; private set; }
	
	public float startingFood;
	public float remainingFood { get; private set; }	
	
	public float startingMedicine;
	public float remainingMedicine { get; private set; }	

	public int daysPerYear = 10;
	public int secsPerDay = 300;
	private int daysElapsed = 0;
	private float secsElapsed = 0f;

	private float storageMax = 10f;
	private float storageRemaining;

	private GameManager gm;
	private HeaderMenuManager headerMenu;


	void Start() {
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		headerMenu = gm.GetComponent<HeaderMenuManager> ();

		remainingMoney = startingMoney;
		remainingFood = startingFood;
		remainingMedicine = startingMedicine;
		remainingWater = startingWater;

	}

	void Update() {
	
		if (isPaused)
			return;

		if (secsElapsed >= secsPerDay || secsElapsed == 0) {
			daysElapsed += 1;
			secsElapsed = 0;
			headerMenu.UpdateDisplay(daysElapsed, currentLevel, 1, remainingWater, remainingFood, remainingMedicine, remainingMoney);
		}

		secsElapsed += Time.deltaTime;
	
	}

	public int getDaysElapsed() {
		return(daysElapsed);
	}

	public float getRemainingMoney() {
		return remainingMoney;
	}


	//TODO implement storage limits and max resources	
	public float getStorageRemaining() {
		return 10f;
	}

	public void addFood(float amount) {
		remainingFood += amount;
		headerMenu.UpdateDisplay(daysElapsed, currentLevel, 1, remainingWater, remainingFood, remainingMedicine, remainingMoney);
	}
	
	public void addWater(float amount) {
		remainingWater += amount;
		headerMenu.UpdateDisplay(daysElapsed, currentLevel, 1, remainingWater, remainingFood, remainingMedicine, remainingMoney);
	}
	
	public void addMedicine(float amount) {
		remainingMedicine += amount;
		headerMenu.UpdateDisplay(daysElapsed, currentLevel, 1, remainingWater, remainingFood, remainingMedicine, remainingMoney);
	}

	public void addMoney(float amount) {
		remainingMoney += amount;
		headerMenu.UpdateDisplay(daysElapsed, currentLevel, 1, remainingWater, remainingFood, remainingMedicine, remainingMoney);
	}

	public bool deductFood(float amount) {
		// return true if success, false if failure

		if (remainingFood <= 0)
			return false;

		remainingFood = Mathf.Max (0, remainingFood - amount);
		headerMenu.UpdateDisplay(daysElapsed, currentLevel, 1, remainingWater, remainingFood, remainingMedicine, remainingMoney);
		return true;
	}

	public bool deductWater(float amount) {
		if (remainingWater <= 0)
			return false;
		remainingWater = Mathf.Max (0, remainingWater - amount);
		headerMenu.UpdateDisplay(daysElapsed, currentLevel, 1, remainingWater, remainingFood, remainingMedicine, remainingMoney);
		return true;
	}
	
	public bool deductMedicine(float amount) {
		if (remainingMedicine <= 0)
			return false;
		remainingMedicine = Mathf.Max (0, remainingMedicine - amount);
		headerMenu.UpdateDisplay(daysElapsed, currentLevel, 1, remainingWater, remainingFood, remainingMedicine, remainingMoney);
		return true;
	}
	
	public bool deductMoney(float amount) {
		if (remainingMoney <= 0)
			return false;
		remainingMoney = Mathf.Max (0, remainingMoney - amount);
		headerMenu.UpdateDisplay(daysElapsed, currentLevel, 1, remainingWater, remainingFood, remainingMedicine, remainingMoney);
		return true;
	}

	public void setActiveBuildItem(LabItem item) {
		activeBuildItem = item;

		// trigger gm to create tile piece and move it when mouse moves
		gm.HoldPiece (item.gameObject);

	}

	public LabItem getActiveBuildItem() {
		return (activeBuildItem);
	}

	public LabItem getLabItem(string title) {
		for (int i = 0; i < this.labItemPrefabs.Length; i++) {
			if (this.labItemPrefabs[i].GetComponent<LabItem>().title == title)
				return (this.labItemPrefabs[i].GetComponent<LabItem>());
		}
		return null;
	}

}
