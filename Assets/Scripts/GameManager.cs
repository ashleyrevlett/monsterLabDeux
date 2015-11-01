using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int currentLevel = 1;
	public GameObject labBoardPrefab; // level 1
	public GameObject huntBoardPrefab; // level 2
	private GameObject labScene;
	private GameObject huntScene;

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


		if (level == 1) {

			// destroy hunt scene if active
			GameObject huntScene = GameObject.Find("HuntScene");
			if (huntScene != null)
				Destroy(huntScene);

			// crate board if doesn't exist
			if (labScene == null) {
				labScene = Instantiate (labBoardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			}

			labScene.SetActive(true);
			labScene.name = "LabScene";
			currentLevel = 1;
			labScene.transform.SetParent (gameObject.transform);

		} else if (level == 2) {

			// pause lab scene
			labScene.SetActive(false);

			// create hunt board scene
			huntScene = Instantiate (huntBoardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			huntScene.name = "HuntScene";
			currentLevel = 2;
			huntScene.transform.SetParent (gameObject.transform);
		}	


	
	}
	


}
