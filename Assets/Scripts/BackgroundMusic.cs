using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

	public static BackgroundMusic instance;

	void Start() {
		DontDestroyOnLoad (gameObject);
		if (instance == null)
			instance = this;
		else
			Destroy (gameObject);
	}

}
