using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

	public float health = 100f; // %
	public bool isAlive = true;

	// per second, milliliters or grams // secsPerDay = 86400
	public float oxygenConsumption = 6.37f; 	// 550f liters per 24 hrs
	public float waterConsumption = 0.03f; 	// 2.3 liters per 24 hrs
	public float co2Production = .05f; 		// 450f liters per 24 hrs
	public float foodConsumption = .02f; 	// 1.9 kg per 24 hrs

	private SpriteRenderer sprite;

	// Use this for initialization
	void Awake () {
		isAlive = true;
		sprite = gameObject.GetComponent<SpriteRenderer> ();
		sprite.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (health <= 0f) {
			isAlive = false;
			Debug.Log ("Died!");
			sprite.enabled = false;
		}

	}

	public void TakeDamage() {
		health -= 1f; 
	}


}
