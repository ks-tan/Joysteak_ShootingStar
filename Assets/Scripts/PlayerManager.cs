using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager instance;

	public float jumpHeight;
	public float jumpMultiplier;
	public bool isGrounded;
	public bool onWater;
	public float horizontalSpeed;
	public float verticalSpeed;
	public float maxOverallSpeed;
	public float maxHorizontalSpeed;
	public float maxWaterSpeed;
	public bool isDead;
	public bool isAuto;
	public bool isRacer;
	public bool isSuperJump;

	FloorManager floorManager;
	GameObject floor;
	Rigidbody2D rgb;
	bool canDoubleJump;
	int superJumpCount;
	int prevPlatformIndex;
	int autoHorizontalSpeed = 20;

	void Start () {
		if (instance == null) instance = this;
		rgb = GetComponent<Rigidbody2D> ();
		isGrounded = true;
		onWater = false;
		isDead = false;
		superJumpCount = 0;
		prevPlatformIndex = 0;
		verticalSpeed = 40.0f;
		rgb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			Physics2D.IgnoreCollision (other.collider, this.GetComponent<BoxCollider2D>());
		}

		if (other.gameObject.tag == "Floor") {
			floor = other.gameObject;
			isGrounded = true;

			//player jumped across 1 or more platforms
			if (Mathf.Abs (GameManager.instance.platformsPassed - prevPlatformIndex) > 0) {
				if (!isAuto) GameManager.instance.commendPlayer ();
				GameManager.instance.updateScore (Mathf.Abs (GameManager.instance.platformsPassed - prevPlatformIndex));
				prevPlatformIndex = GameManager.instance.platformsPassed;
			}

			//reset isSuperJump effects once landed
			if (isSuperJump) {
				isSuperJump = false;
				transform.FindChild ("Particles").gameObject.SetActive (false);
			}

			if (isAuto && getJumpMultiplier() > 1.5) executeJump ();
		}
	}


	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.tag == "Water") {
			onWater = true;
			SpecialEffects.instance.makeSmallSplash (this.transform.position);
			SpecialEffects.instance.makeBigSplash (this.transform.position);
			//Limit speed in water
			if(rgb.velocity.magnitude > maxWaterSpeed){
				rgb.velocity = rgb.velocity.normalized * maxWaterSpeed;
			}
			if (isAuto) executeJump ();
		}

		if (other.gameObject.tag == "Death" && !isAuto) {
			isDead = true;
			if (!isAuto) {
				StartCoroutine (GameManager.instance.restartAfterTime (0.4f));
			}
		}

	}

	void OnBecameInvisible() {
		//hot fix for bug when racer strangely doesn't detect water and drop off the screen
		if (Camera.main != null) {
			if (isAuto && transform.position.y < Camera.main.gameObject.transform.position.y) {
				Destroy (gameObject);
				GameManager.instance.isRacing = false;
			}
		}
	}


	// Character movement and physics
	void Update() {
		// Game speed gets faster until max speed
		if (horizontalSpeed <= maxHorizontalSpeed) {
			horizontalSpeed += Time.deltaTime;
		}

		//autoplayer behaviors
		if (isAuto) {
			horizontalSpeed = autoHorizontalSpeed;
			if (isRacer) {
				Vector3 cameraPos = Camera.main.gameObject.transform.position;
				transform.position = new Vector3 (cameraPos.x, transform.position.y, transform.position.z);
			}
		}
			
		if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetMouseButtonDown(0)) executeJump ();

		//player rotation speed
		rgb.angularVelocity = -horizontalSpeed * 45f;

		if (isGrounded || onWater) {
			//player horizontal speed when grounded
			rgb.velocity = new Vector2 (horizontalSpeed, rgb.velocity.y);
		} else {
			//player horizontal speed when jumping
			rgb.velocity = new Vector2 (horizontalSpeed * jumpMultiplier * jumpMultiplier * 0.8f, rgb.velocity.y);
		}

	}


	void executeJump() {
		if (isGrounded || onWater) {
			if (!onWater) {
				jumpMultiplier = getJumpMultiplier ();
				isSuperJump = checkSuperJump ();
			} else {
				if (!isAuto) GameManager.instance.commendPlayer ();
				jumpMultiplier = 1.2f;
				isSuperJump = false;
				onWater = false;
			}

			if (isSuperJump) {
				superJumpCount++;
				transform.FindChild ("Particles").gameObject.SetActive (true);
				SpecialEffects.instance.makeExplosion (transform.position, Color.white);
				if (!isAuto) GameManager.instance.commendPlayer ();
			} else {
				superJumpCount = 0;
			}

			floor = null;
			isGrounded = false;
			rgb.AddForce (new Vector2 (0f, jumpHeight * verticalSpeed * 1.5f));
			canDoubleJump = true;

		} else if (canDoubleJump) {
			canDoubleJump = false;
			rgb.velocity = new Vector2 (rgb.velocity.x, 0);
			rgb.AddForce (new Vector2 (0f, jumpHeight * verticalSpeed * 1.5f));
			if (isSuperJump) SpecialEffects.instance.makeExplosion (transform.position, Color.white);
		}

		//Limit speed
		if(rgb.velocity.magnitude > maxOverallSpeed){
			rgb.velocity = rgb.velocity.normalized * maxOverallSpeed;
		}
	}


	// Checks how much is the jump multiplier by measuring position of player on platform
	public float getJumpMultiplier() {
		float jumpMultiplier = 1;
		if (floor != null) {
			if (transform.position.x - floor.transform.position.x > 0) {
				float width = floor.GetComponent<SpriteRenderer> ().bounds.size.x;
				jumpMultiplier += (transform.position.x - floor.transform.position.x) / (width / 2);
			}
		}
		return jumpMultiplier;
	}


	public bool checkSuperJump(){
		float jumpMultiplier = getJumpMultiplier ();
		if (jumpMultiplier > 1.9) return true;
		else return false;
	}
}
