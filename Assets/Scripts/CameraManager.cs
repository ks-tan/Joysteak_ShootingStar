using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public static CameraManager instance;
	public Transform target;
	public float dampTime = 0.15f;

	private Camera cam;
	private Vector3 velocity = Vector3.zero;
	private float zoomIn = 8;

	void Start () {
		if (instance == null) instance = this;
		cam = GetComponent<Camera> ();
		cam.orthographicSize = zoomIn;
	}

	void Update () {
		if (target){
			Vector3 point = Camera.main.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.05f, point.y, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
	}

	public void UpdateTarget(Transform t) {
		target = t;
	}
}
