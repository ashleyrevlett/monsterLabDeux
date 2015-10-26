using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	private Vector3 moveDirection = Vector3.zero;
	private bool isMoving = false;
	public float moveSpeed;
	private Vector2 boardSize;
	
	// Use this for initialization
	void Start () {
		HuntBoardManager boardManager = GameObject.Find("HuntScene").GetComponent<HuntBoardManager> ();
		boardSize = new Vector2 (boardManager.columns, boardManager.rows);
	}

	public void Hit() {
		Debug.Log ("Hit!");
		isMoving = false;
		Destroy (gameObject);
	}
	
	public void Fire(Vector3 dir) {
		Debug.Log ("Firing in direction: " + dir);
		moveDirection = dir.normalized;
		isMoving = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			gameObject.transform.position = Vector3.Lerp(
				gameObject.transform.position, 
				(gameObject.transform.position + moveDirection),
				Time.deltaTime * moveSpeed);	
		}
	
		if (gameObject.transform.position.x < -boardSize.x * 256f || gameObject.transform.position.x > boardSize.x * 256f || 
		    gameObject.transform.position.y < -boardSize.y * 256f || gameObject.transform.position.y > boardSize.y * 256f) {
			Debug.Log("Remove object");
			isMoving = false;
		}


	}

	
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Trigger entered on bullet");
		if (other.gameObject.tag == "Monster") {
			MonsterController m = other.gameObject.GetComponent<MonsterController>();
			m.Die();
			Hit ();	
		}
	}

}
