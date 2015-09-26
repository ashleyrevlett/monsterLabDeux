using UnityEngine;
using System.Collections;

public class ScientistController : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("left"))
			Walk (new Vector3(-1f, 0));

		if (Input.GetKeyDown("right"))
			Walk (new Vector3(1f, 0f));

		if (Input.GetKeyDown("up"))
			Walk (new Vector3(0f, 1f));

		if (Input.GetKeyDown("down"))
			Walk (new Vector3(0f, -1f));

	}

	public void Walk(Vector2 direction) {
		Debug.Log ("Walking");

		int layerMask = 1 << 8; // bit shift layer index to get mask 

		RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, direction, 1f, layerMask);
		if (hit.collider != null) {
			float distance = Mathf.Abs (hit.point.y - gameObject.transform.position.y);
			Debug.Log ("Hit " + hit.collider.gameObject.name + ", distance to hit: " + distance);
			if (hit.collider.gameObject.tag == "Obstacle") {
				return;
			}
		} else { 
			float newX = gameObject.transform.position.x + direction.x;
			float newY = gameObject.transform.position.y + direction.y;
			Vector3 newPos = new Vector3 (newX, newY, gameObject.transform.position.z);
			gameObject.transform.position = newPos;
		}
	}



}
