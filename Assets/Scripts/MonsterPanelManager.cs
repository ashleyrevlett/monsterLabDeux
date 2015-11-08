using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterPanelManager : MonoBehaviour {
	
	private GameObject monsterPanel;
	public GameObject buttonObject;
	private List<GameObject> monsters;
	private GameManager gm;
	
	void Awake () {	
		monsterPanel = GameObject.Find ("MonsterPanel");
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		monsters = new List<GameObject> ();

	}

	void OnEnable() {	
		monsters.Clear ();
		foreach (GameObject m in gm.capturedMonsters) {			
			AddMonster (m);
		};
	}


	public void AddMonster(GameObject monsterObject) {	
		monsters.Add (monsterObject);
		GameObject btn = Instantiate (buttonObject, new Vector3 ( 0f, 0f, 0f), buttonObject.transform.localRotation) as GameObject;
		btn.transform.SetParent(monsterPanel.transform);				

		MonsterButton monsterBtn = btn.GetComponent<MonsterButton> ();
		monsterBtn.setMonster (monsterObject);

	}
	
}
