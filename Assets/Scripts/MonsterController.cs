using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

	public bool isAlive = true;
	private bool isPlaced = false;

	public float healRate = 1f;  // how fast creature regenerates health, hp per tick
	public float appetite = 1f; // how fast hunger and thirst grow
	public float damageRate = 2f;

	public float maxHealth = 10f; 
	public float health { get; set; }
	
	public float maxThirst = 10f; 
	public float thirst { get; set; }

	public float maxHunger = 10f; 
	public float hunger { get; set; }

	public Sprite spriteDead;
	private SpriteRenderer sprite;

	private BoxCollider2D bc; // activate collider when placed to start tracking mouse clicks
	private GameManager gm; 

	// Use this for initialization
	void Awake () {

		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		bc = gameObject.GetComponent<BoxCollider2D> ();
		bc.enabled = false;

		isAlive = true;
		sprite = gameObject.GetComponent<SpriteRenderer> ();
		sprite.enabled = true;

		health = maxHealth;
		hunger = 0f;
		thirst = 0f;

		InvokeRepeating ("Tick", 5f, 5f);

	}
	

	void Update () {	
		if (health <= 0f && isAlive) {
			Die ();
		}
	}


	void Tick() {	

		Debug.Log ("Ticking!");

		if (!isAlive) {
			CancelInvoke ();
			return;
		}

		// heal self
		health = Mathf.Min(maxHealth, health + healRate);

		// calc damage if too hungry or thirsty
		hunger = Mathf.Min (maxHunger, hunger + appetite);
		thirst = Mathf.Min (maxThirst, thirst + (appetite * 1.75f)); // thirst is 175% stronger than hunger
		if (hunger >= maxHunger || thirst >= maxThirst) 
			health = Mathf.Max (0, health - damageRate);			

		Debug.Log (string.Format ("Thirst: {0}\tHunger: {1}\tHealth: {2}\t", thirst, hunger, health));


	}

	
	void Die() {
		isAlive = false;
		Debug.Log ("Died!");
		sprite.enabled = false;
	}
	

	public void TakeDamage() {
		health -= 1f; 
	}

	public void setIsPlaced(bool val) {
		isPlaced = val;
		if (isPlaced == true)
			bc.enabled = true;
	}

	void OnMouseDown() {
		Debug.Log ("Mouse down on monster!");
		gm.showInfo (this);
	}


}
