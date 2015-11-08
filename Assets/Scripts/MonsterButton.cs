using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterButton : MonoBehaviour {

	private GameObject monsterObject;
	private Text nameText;
	private BoardManager board;

	
	// Use this for initialization
	void Awake () {
		nameText = gameObject.GetComponentInChildren<Text> ();	
		board = GameObject.Find ("LabScene").GetComponent<BoardManager> ();
	}

	public void setMonster(GameObject newMonster) {
		monsterObject = Instantiate(newMonster);
		MonsterController monster = monsterObject.GetComponent<MonsterController> ();
		nameText.text = monster.monsterName;

		// hide actual monster until we pick it up
		monsterObject.SetActive (false);

		// setup click handler to change active build button in game state
		Button button = gameObject.GetComponent<Button> ();
		button.onClick.AddListener(delegate { buttonClick(); });
	}
		
	// button press callback
	public void buttonClick(){
		Debug.Log ("Btn pressed");

		//board.createNewMonster(m)
		//
		//			// find first available cage on board
		//			Vector2 pos = board.getValidMonsterPosition(); 
		//			// create new copy of captured monster
		//			GameObject monster = board.createNewMonster(m);
		//			MonsterController monstercontroller = monster.GetComponent<MonsterController>();
		//			board.placeMonsterPiece((int)pos.x, (int)pos.y, monstercontroller);					
		//			// turn bc on for monster
		//			BoxCollider2D bcMonster = monster.GetComponent<BoxCollider2D>();
		//			bcMonster.enabled = true;		

		if (monsterObject != null) {

			//need to instantiate monster, then tell board to hold it
			monsterObject.SetActive (true);
			board.HoldPiece (monsterObject);
			Destroy(gameObject);
		}
	}


}
