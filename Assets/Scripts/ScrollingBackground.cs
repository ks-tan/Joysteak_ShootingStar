using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour {

	public GameObject camera;
	float offset;
	int index;
	int prevIndex;
	Rigidbody2D rgb;
	Transform prevBgItem;
	Camera cameraProps;

	void Start() {
		cameraProps = camera.GetComponent<Camera> ();
		offset = 0.1f;
		rgb = this.GetComponent<Rigidbody2D> ();
		index = transform.GetSiblingIndex ();
		prevIndex = index == 0 ? 2 : index - 1;
		foreach (Transform child in transform.parent) {
			if (child.transform.GetSiblingIndex() == prevIndex){
				prevBgItem = child.transform;
			}
		}
	}

	void FixedUpdate() {
		rgb.velocity = new Vector2 (-cameraProps.velocity.x/2, rgb.velocity.y);

	}

	void OnBecameInvisible() {
		float spawnPosX = prevBgItem.position.x + this.GetComponent<SpriteRenderer>().bounds.size.x - offset;
		transform.position = new Vector3(spawnPosX, transform.position.y, transform.position.z);
	}
}
