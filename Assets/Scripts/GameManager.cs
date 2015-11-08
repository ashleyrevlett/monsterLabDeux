using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public int currentLevel = 1;
	public GameObject labBoardPrefab; // level 1
	public GameObject huntBoardPrefab; // level 2
	private GameObject labScene;
	private GameObject huntScene;
	public List<GameObject> capturedMonsters;

	void Start () {

		capturedMonsters = new List<GameObject> ();
		LoadLabScene ();

	}

	public void LoadHuntScene() {

		capturedMonsters.Clear ();

		// pause lab scene
		labScene.SetActive(false);
		currentLevel = 2;

		// create hunt board scene
		huntScene = Instantiate (huntBoardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		huntScene.name = "HuntScene";
		huntScene.transform.SetParent (gameObject.transform);


	}

	public void LoadLabScene() {

		capturedMonsters.Clear ();

		// called on first game load and after hunt scene
		currentLevel = 1;

		// destroy hunt scene if active
		GameObject huntScene = GameObject.Find("HuntScene");
		if (huntScene != null) {
			// we are exiting the huntscene, so save any captured monsters, then clean up
			HuntBoardManager huntBoard = huntScene.GetComponent<HuntBoardManager>();
			capturedMonsters = new List<GameObject>(huntBoard.GetDeadMonsters());
			Destroy(huntScene);
		}
		
		// create board if doesn't exist
		if (labScene == null) {
			labScene = Instantiate (labBoardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			labScene.name = "LabScene";
			labScene.transform.SetParent (gameObject.transform);
		}
		
		labScene.SetActive(true);

	}

	


}
