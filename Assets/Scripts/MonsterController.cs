using UnityEngine;
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
	private GameStateStore gss;

	// Use this for initialization
	void Awake () {

		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		gss = GameObject.Find ("GameManager").GetComponent<GameStateStore> ();

		// turn off activeBg until monster is selected
		selectedBackground.SetActive (false);

		// turn off collider until monster is set down
		bc = gameObject.GetComponent<BoxCollider2D> ();
		bc.enabled = false;

		// prepare death sprite
		isAlive = true;
		sprite = gameObject.GetComponent<SpriteRenderer> ();
		sprite.enabled = true;
		spriteAlive = sprite.sprite;

		// reset stats
		health = maxHealth;
		hunger = 0f;
		thirst = 0f;

		// create initial monster 
		monsterName = "Lorem ipsum";
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

		// begin monster's life cycle
		InvokeRepeating ("Tick", 5f, 5f);

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



		// calc damage if too hungry or thirsty
		hunger = Mathf.Min (maxHunger, hunger + appetite);
		thirst = Mathf.Min (maxThirst, thirst + (appetite * 1.75f)); // thirst is 175% stronger than hunger
		if (hunger >= maxHunger || thirst >= maxThirst) 
			TakeDamage ();
		else 
			sprite.sprite = spriteAlive; // show the default sprite
					
		Debug.Log (string.Format ("Thirst: {0}\tHunger: {1}\tHealth: {2}\t", thirst, hunger, health));


	}

	
	void Die() {
		isAlive = false;
		Debug.Log ("Died!");
		sprite.sprite = spriteDead;
	}
	

	public void TakeDamage() {
		health = Mathf.Max (0, health - damageRate);	
		sprite.sprite = spriteDamage;
	}

	public void setIsPlaced(bool val) {
		isPlaced = val;
		if (isPlaced == true)
			bc.enabled = true;
	}

	void OnMouseDown() {
		Debug.Log ("Mouse down on monster!");
		gm.hideInfo ();
		if (gm.heldPiece == null) {
			gm.showInfo (this);
			selectedBackground.SetActive (true);
		}
	}

	public void Deselect() {
		selectedBackground.SetActive (false);
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


}
