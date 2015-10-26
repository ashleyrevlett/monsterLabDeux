using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int currentLevel = 1;
	public GameObject labBoardPrefab; // level 1
	public GameObject huntBoardPrefab; // level 2
	private GameObject board; // "LabScene" or "HuntScene"
	

	void Start () {
		LoadLevel(currentLevel);
	}

	public void LoadHuntScene() {
		LoadLevel (2);
	}

	public void LoadLabScene() {
		LoadLevel (1);
	}

	void LoadLevel(int level) {

		ResetLevels ();	// destroy other level if loaded

		if (level == 1) {
			board = Instantiate (labBoardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			board.name = "LabScene";
			currentLevel = 1;
		} else if (level == 2) {
			board = Instantiate (huntBoardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			board.name = "HuntScene";
			currentLevel = 2;
		}	
		board.transform.SetParent (gameObject.transform);
	
	}

	
	void ResetLevels() {
		
		GameObject labScene = GameObject.Find("LabScene");
		if (labScene != null)
			Destroy(labScene);
		
		GameObject huntScene = GameObject.Find("HuntScene");
		if (huntScene != null)
			Destroy(huntScene);
		
	}


}
