using UnityEngine;
using System.Collections;

public class ScientistController : MonoBehaviour {

	public GameObject bulletTile;

	public Sprite frontImage;
	public Sprite backImage;
	public Sprite leftImage;
	public Sprite rightImage;

	public float walkSpeed = 1f;
	public float tolerance = 0;

	public bool canWalk = true;
	private bool isWalking = false;
	private Vector2 faceDirection = Vector2.up;
	private Vector3 startMarker;
	private Vector3 endMarker;
	private float walkStartTime;

	private Vector3 camMoveFromLoc;
	private Vector3 camMoveToLoc;
	private float camStartMoveTime;
	private bool isCamMoving = false;


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
			if (faceDirection == moveDir && !isWalking && canWalk) { // facing right dir already, make walk
				isWalking = true;
				float newX = transform.position.x + moveDir.x;
				float newY = transform.position.y + moveDir.y;
				endMarker = new Vector3(newX, newY, 0f);
				startMarker = gameObject.transform.position;
				Debug.Log ("iswalking");
				walkStartTime = Time.time;
			} else if (faceDirection != moveDir) { // stop walking, turn to face direction
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

		// stop walking when we get within tolerable range
		if (Mathf.Abs(transform.position.x - endMarker.x) <= tolerance &&
		    Mathf.Abs(transform.position.y - endMarker.y) <= tolerance ) {
			isWalking = false;
			return;
		}

		// raycast to see if there's an object tagged "Obstacle" in our way
		Vector2 direction = faceDirection;
		bool canMove = true;
		RaycastHit2D[] hits = Physics2D.RaycastAll(gameObject.transform.position, direction, 1f);
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider != null) {
				float distance = Mathf.Abs (hit.point.y - gameObject.transform.position.y);
				Debug.Log ("Hit " + hit.collider.gameObject.tag + ", distance to hit: " + distance);
				if (hit.collider.gameObject.tag == "Obstacle") {
					Debug.Log ("Not moving, object in front");
					canMove = false; // can't just return because we might not be first in list
				}
			}
		}
		if (canMove) {
			Debug.Log ("Clear path, moving");
			// no Obstacle, so smoothly move toward position
			float distCovered = (Time.time - walkStartTime) * walkSpeed;
			float fracJourney = distCovered / 1f; // moving one unit
			gameObject.transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);
		}

	}


}
