using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	GameObject player;
	bool isCollected;
	bool isReturning;
	float t;

	void Start() {
		isCollected = false;
		isReturning = false;
		t = 0;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.gameObject.tag == "Player") {
			player = other.gameObject;
			if (player.GetComponent<PlayerManager> ().isAuto == false) {
				isCollected = true;
				if (!isReturning)
					GameManager.instance.updateMoney ();
			}
		}
	}

	void Update() {
		if (isCollected) {
			if (t < 1) {
				t += Time.deltaTime * 3;
				Transform playerTransform = player.transform;
				Vector3 target = new Vector3 (playerTransform.position.x - 3, playerTransform.position.y + 3, playerTransform.position.z);
				transform.position = Vector3.Lerp (transform.position, target, t);
			} else {
				t = 0;
				isCollected = false;
				isReturning = true;
			}
		}
		if (isReturning) {
			if (t < 1) {
				t += Time.deltaTime;
				transform.position = Vector3.Lerp (transform.position, player.transform.position, t);
			} else {
				Destroy (this.gameObject);
			}
		}
	}

	void OnBecameInvisible() {
		Destroy (this.gameObject);
	}

}
