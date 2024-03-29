﻿using UnityEngine;
using System.Collections;
using System; // for enums

public class MonsterController : MonoBehaviour {

	public enum Sex {Male, Female};
	public string monsterName { get; set; }
	public int age { get; set; } // days
	public Sex monsterSex { get; set; } 
	public float value { get; set; } // dollars
	public int strength { get; set; } 
	public int agility { get; set; } 
	public int intelligence { get; set; } 
	public int lifespan { get; set; } // days	
	public int gestation { get; set; } // days
	public int incubation { get; set; } // days
	public int ageOfAdulthood { get; set; } // days
	public int matingFrequency { get; set; } // times per year
	public bool isAlive { get; set; }
	private bool isPlaced = false;
	private bool isExperimenting = false;
	private float experimentDamage; 
	private float experimentTicksRemaining;
	public float healRate = 1f;  // how fast creature regenerates health, hp per tick
	public float appetite = 1f; // how fast hunger and thirst grow
	private float damageRate = 2f;
	public float maxHealth = 10f; 
	public float health { get; set; }	
	public float maxThirst = 10f; 
	public float thirst { get; set; }
	public float maxHunger = 10f; 
	public float hunger { get; set; }
	public Sprite spriteDamage;
	public Sprite spriteDead;
	public Sprite spriteAlive;
	private SpriteRenderer sprite;
	public GameObject selectedBackground;
	private BoxCollider2D bc; // activate collider when placed to start tracking mouse clicks
	private GameManager gm; 
	private BoardManager board;
	private GameStateStore gss;
	private HuntBoardManager huntboard;
	private Vector2 faceDirection = Vector2.right;
	public float walkSpeed = 1f;

	/*
	 * MonsterController object is created in hunt scene first, then copied to LabScene.
	 * Monster "dies" in hunt but is revived at lab.
	 * MonsterObject is disabled by monsterButton upon its creation, then enabled once button is pressed.
	 */ 
	
	void Awake () {

		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		gss = GameObject.Find ("GameManager").GetComponent<GameStateStore> ();
		sprite = gameObject.GetComponent<SpriteRenderer> ();
		bc = gameObject.GetComponent<BoxCollider2D> ();

		// create initial monster 
		monsterName = gss.getRandomName();
		monsterSex = (Sex)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Sex)).Length));
		lifespan = (int)UnityEngine.Random.Range (2f, gss.daysPerYear * 100f);
		age = (int)UnityEngine.Random.Range (1f, lifespan - 1f);
		gestation = (int)UnityEngine.Random.Range (1f, Mathf.Max(1f,(lifespan * .01f)));
		incubation = (int)UnityEngine.Random.Range (1f, Mathf.Max(1f,(lifespan * .01f)));
		ageOfAdulthood = (int)UnityEngine.Random.Range (lifespan * .01f, lifespan * .25f);
		matingFrequency = (int)UnityEngine.Random.Range(gestation + incubation, lifespan); // baby per days, min once per life
		strength = (int)UnityEngine.Random.Range (1f, 100f);
		agility = (int)UnityEngine.Random.Range (1f, 100f);
		intelligence = (int)UnityEngine.Random.Range (1f, 100f);

		ResetMonster ();

	}


	void OnEnable() {
		if (gm.currentLevel == 1 && isPlaced) {
			// already placed in lab, need to start tick when we enable the monster
			// since we won't be placing it manually
			InvokeRepeating ("Tick", 5f, 5f);
		} else if (gm.currentLevel == 2) {
			huntboard = GameObject.Find ("HuntScene").GetComponent<HuntBoardManager> ();
			StartCoroutine (Roam ());
		}

	}

	void OnDisable() {
		// disabled when in lab going to huntscene (may be on board),
		// and in huntscene when goign to labscene
		CancelInvoke("Tick");
	}



	public void EnterLab() {
		Debug.Log ("Reborn!!");
		board = GameObject.Find ("LabScene").GetComponent<BoardManager> ();
		StopCoroutine (Roam ());
		ResetMonster ();
		selectedBackground.SetActive (false);
		BoxCollider2D bc2 = gameObject.GetComponent<BoxCollider2D>();
		bc2.enabled = false;
	}


	void ResetMonster() {
		// reset stats		
		isAlive = true;
		sprite.enabled = true;
		sprite.sprite = spriteAlive;
		health = maxHealth;
		hunger = 0f;
		thirst = 0f;
	}

	void Update () {	
		if (isAlive && (health <= 0f || age > lifespan)) {
			Die ();
		}

	}


	void Tick() {	

		if (!isAlive) {
			CancelInvoke ();
			return;
		}

		if (isExperimenting) {
			experimentTicksRemaining -= 1f;
			TakeDamage(experimentDamage);
		}

		if (experimentTicksRemaining <= 0)
			isExperimenting = false;

		// calc damage if too hungry or thirsty
		hunger = Mathf.Min (maxHunger, hunger + appetite);
		thirst = Mathf.Min (maxThirst, thirst + (appetite * 1.75f)); // thirst is 175% stronger than hunger
		if (hunger >= maxHunger || thirst >= maxThirst) 
			TakeDamage (damageRate);
		else 
			sprite.sprite = spriteAlive; // show the default sprite

		//  Debug.Log (string.Format ("Thirst: {0}\tHunger: {1}\tHealth: {2}\t", thirst, hunger, health));


	}


	IEnumerator Roam() {
		bool canWalk = true; 
		while (canWalk) {		
			// check for obstacles		
			RaycastHit2D[] hits = Physics2D.RaycastAll(gameObject.transform.position, faceDirection, 1f);
			foreach (RaycastHit2D hit in hits) {
				if (hit.collider != null) {
					float distance = Mathf.Abs (hit.point.y - gameObject.transform.position.y);
//					Debug.Log ("Monster Hit " + hit.collider.gameObject.tag + ", distance to hit: " + distance);
					if (hit.collider.gameObject.tag == "Obstacle") {
//						Debug.Log ("Not moving, object in front");
						// turn around
						faceDirection = new Vector3(-faceDirection.x, faceDirection.y);
					}
				}
			}
			// walk in facing direction
			Vector3 newDir = faceDirection * (walkSpeed * Time.deltaTime);
			gameObject.transform.position += newDir; 
			yield return null;		
		}
	}

	
	public void Die() {
		StopAllCoroutines();
		isAlive = false;
		Debug.Log ("Died!");
		sprite.sprite = spriteDead;
	}

	public void TakeDamage(float amount) {
		health = Mathf.Max (0, health - amount);	
		sprite.sprite = spriteDamage;
	}

	public void setIsPlaced(bool val) {
		isPlaced = val;
		if (isPlaced == true) {		
			BoxCollider2D bc2 = gameObject.GetComponent<BoxCollider2D>();
			bc2.enabled = true;
			InvokeRepeating ("Tick", 5f, 5f);
		}
	}

	void OnMouseDown() {
		Debug.Log ("Mouse down on monster!");
		if (gm != null && huntboard == null) {
			board.hideInfo ();
			if (board.heldPiece == null) {
				board.showInfo (this);
				selectedBackground.SetActive (true);
			}
		}
	}

	public void Deselect() {
	}

	
	public void Water() {
		thirst = 0f;
		Debug.Log ("Watered " + monsterName + ", thirst: " + thirst);
	}
	
	public void Feed() {
		hunger = 0f;
		Debug.Log ("Fed " + monsterName + ", hunger: " + hunger);
	}

	public void Heal() {
		health = maxHealth;
		Debug.Log ("Healed " + monsterName + ", health: " + health);
	}
	
	public void Experiment(float damagePerTick, float ticks) {
		Debug.Log ("Conducting Experiment in Monster!");
		isExperimenting = true;
		experimentDamage = damagePerTick;
		experimentTicksRemaining = ticks;
	}


}
