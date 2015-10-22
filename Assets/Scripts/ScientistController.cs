using UnityEngine;
using System.Collections;

public class ScientistController : MonoBehaviour {

	public GameObject bulletTile;

	public Sprite frontImage;
	public Sprite backImage;
	public Sprite leftImage;
	public Sprite rightImage;

	public float walkSpeed = 1f;
	public float tolerance = 0.5f;

	private bool isWalking = false;
	private Vector2 faceDirection = Vector2.up;
	private Vector3 moveToLocation = Vector3.zero;

	private SpriteRenderer spriteRenderer;

	private BoardManager boardManager;

	// Use this for initialization
	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		GameObject gm = GameObject.Find ("GameManager");
		boardManager = gm.GetComponent<BoardManager> ();
	}


	
	// Update is called once per frame
	void Update () {


		Vector2 moveDir = Vector2.zero;
		if (Input.GetKeyDown("left")) {
			moveDir = Vector2.left;
		} else if (Input.GetKeyDown("right")) {
			moveDir = Vector2.right;
		} else if (Input.GetKeyDown("up")) {
			moveDir = Vector2.up;
		} else if (Input.GetKeyDown("down")) {
			moveDir = Vector2.down;
		} else if (Input.GetKeyDown("space")) {
			Shoot ();
		}

		if (Input.GetKeyDown ("left") || Input.GetKeyDown ("right") || 
		    Input.GetKeyDown ("up") || Input.GetKeyDown ("down")) {
//			if (faceDirection == moveDir && !isWalking) { // facing right dir already, make walk
//				isWalking = true;
//				float newX = transform.position.x + moveDir.x;
//				float newY = transform.position.y + moveDir.y;
//				moveToLocation = new Vector3(newX, newY, 0f);
//				Debug.Log ("iswalking");
//			} else 
			if (faceDirection != moveDir) { // stop walking, turn to face direction
				isWalking = false;
				faceDirection = moveDir;
				Turn (faceDirection);
				Debug.Log ("isturning\t");
			}
		}

		if (isWalking) 
			Walk ();


		// shoot
		if (Input.GetKeyDown ("space")) {
			Shoot();
		}
	}

	private void Shoot() {
		Debug.Log ("shoot!");
		GameObject instance = Instantiate (bulletTile, gameObject.transform.position, Quaternion.identity) as GameObject;
		instance.transform.SetParent (gameObject.transform);
		BulletController bullet = instance.GetComponent<BulletController> ();

		Vector3 shootDir = new Vector3 (faceDirection.x, faceDirection.y, 0f);
		bullet.Fire (shootDir);

	}

	public void Turn(Vector2 dir) {
		if (dir == Vector2.left) 
			spriteRenderer.sprite = leftImage;
		else if (dir == Vector2.right) 
			spriteRenderer.sprite = rightImage;
		else if (dir == Vector2.up) 
			spriteRenderer.sprite = backImage;
		else if (dir == Vector2.down) 
			spriteRenderer.sprite = frontImage;
		else
			Debug.Log ("No dir match");
	}


	public void Walk() {

		if (Mathf.Abs(transform.position.x - moveToLocation.x) <= tolerance &&
		    Mathf.Abs(transform.position.y - moveToLocation.y) <= tolerance ) {
			isWalking = false;
			transform.position = moveToLocation;
			return;
		}

		Debug.Log ("Walking to " + moveToLocation);
		gameObject.transform.position = Vector2.Lerp (transform.position, moveToLocation, walkSpeed);

		//		Vector2 direction = faceDirection;
//
//		int layerMask = 1 << 8; // bit shift layer index to get mask 
//
//		RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, direction, 1f, layerMask);
//		if (hit.collider != null) {
//			float distance = Mathf.Abs (hit.point.y - gameObject.transform.position.y);
//			Debug.Log ("Hit " + hit.collider.gameObject.name + ", distance to hit: " + distance);
//			if (hit.collider.gameObject.tag == "Obstacle") {
//				return;
//			}
//		} else { 
//
////			float newX = gameObject.transform.position.x + direction.x;
////			float newY = gameObject.transform.position.y + direction.y;
////			Vector3 newPos = new Vector3 (newX, newY, gameObject.transform.position.z);
////			gameObject.transform.position = newPos;
//			Vector3 newPos = Vector2.Lerp (transform.position, moveToLocation, walkSpeed * Time.deltaTime);
//			gameObject.transform.position = newPos;
	//			
		// if scientist has moved within edge of board, move camera in that direction too
		float leftSideOfScreen = Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height;
		float rightSideOfScreen = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height;
		float topOfScreen = Camera.main.transform.position.y + Camera.main.orthographicSize;
		float bottomOfScreen = Camera.main.transform.position.y - Camera.main.orthographicSize;
		Debug.Log ("leftSideOfScreen: " + leftSideOfScreen);
		Debug.Log ("rightSideOfScreen: " + rightSideOfScreen);
		Debug.Log ("topOfScreen: " + topOfScreen);
		Debug.Log ("bottomOfScreen: " + bottomOfScreen);

		float offset = 3f;
		if (gameObject.transform.position.x <= leftSideOfScreen + offset) {
			Vector3 newPos = new Vector3 (Camera.main.transform.position.x - 1f, Camera.main.transform.position.y, Camera.main.transform.position.z);
			Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, newPos, walkSpeed);
		} else if (gameObject.transform.position.x >= rightSideOfScreen - offset) {
			Vector3 newPos = new Vector3 (Camera.main.transform.position.x + 1f, Camera.main.transform.position.y, Camera.main.transform.position.z);
			Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, newPos, walkSpeed);
		} else if (gameObject.transform.position.y <= bottomOfScreen + offset) {
			Vector3 newPos = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 1f, Camera.main.transform.position.z);
			Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, newPos, walkSpeed);
		} else if (gameObject.transform.position.y >= topOfScreen - offset) {
			Vector3 newPos = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + 1f, Camera.main.transform.position.z);
			Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, newPos, walkSpeed);			
		}

	}


}
