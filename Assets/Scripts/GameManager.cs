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

		List<GameObject> capturedMonsters = new List<GameObject> ();

		if (level == 1) {

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
			currentLevel = 1;


		} else if (level == 2) {

			// pause lab scene
			labScene.SetActive(false);

			// create hunt board scene
			huntScene = Instantiate (huntBoardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			huntScene.name = "HuntScene";
			currentLevel = 2;
			huntScene.transform.SetParent (gameObject.transform);

		}	

		BoardManager board = labScene.GetComponent<BoardManager>();
		foreach (GameObject m in capturedMonsters) {
			// find first available cage on board
			Vector2 pos = board.getValidMonsterPosition(); 
			// create new copy of captured monster
			GameObject monster = board.createNewMonster(m);
			MonsterController monstercontroller = monster.GetComponent<MonsterController>();
			board.placeMonsterPiece((int)pos.x, (int)pos.y, monstercontroller);					
			// turn bc on for monster
			BoxCollider2D bcMonster = monster.GetComponent<BoxCollider2D>();
			bcMonster.enabled = true;			
		}

	
	}
	


}
