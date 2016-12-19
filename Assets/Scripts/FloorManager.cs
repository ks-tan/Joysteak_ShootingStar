using UnityEngine;
using System.Collections;

public class FloorManager : MonoBehaviour {

	public GameObject collectible;

	int index;
	int prevIndex;
	Transform prevPlatform;
	bool restartingLevel;

	void Start() {
		restartingLevel = false;
		index = transform.GetSiblingIndex ();
		prevIndex = index == 0 ? 4 : index - 1;
		foreach (Transform child in transform.parent) {
			if (child.transform.GetSiblingIndex() == prevIndex){
				prevPlatform = child.transform;
			}
		}
		spawnCollectibles ();
	}

	void OnBecameInvisible() {
		float prevPlatformPos = prevPlatform.position.x;
		transform.position = new Vector3(prevPlatformPos + Random.Range(20, 30), transform.position.y, transform.position.z);
		spawnCollectibles ();
		GameManager.instance.platformsPassed++;
	}

	void OnApplicationQuit(){
		restartingLevel = true;
	}

	void spawnCollectibles() {
		if (!restartingLevel) {
			float width = this.GetComponent<SpriteRenderer> ().bounds.size.x;
			float gap = width / 10;
			float rightEdgePosX = transform.position.x + width / 2;
			for (int i = 0; i < 5; i++) {
				GameObject newCollectible = Instantiate (collectible);
				float collectiblePosX = rightEdgePosX - (gap * i);
				newCollectible.transform.position = new Vector3 (collectiblePosX, transform.position.y + 1.5f, transform.position.z);
				if (i == 0) {
					newCollectible.GetComponent<SpriteRenderer> ().color = Color.white;
				}
			}
		}
	}
}
