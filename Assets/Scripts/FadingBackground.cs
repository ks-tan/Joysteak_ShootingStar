using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadingBackground : MonoBehaviour {

	public GameObject water;

	bool[] isActiveBackground;
	int currentActiveIndex;
	int nextActiveIndex;
	float t;
	Material waterMat;
	Color startColor;

	void Start() {
		isActiveBackground = new bool[transform.childCount];
		for (int i = 0; i < isActiveBackground.Length; i++) {
			if (i == 0) isActiveBackground [i] = true;
			else isActiveBackground [i] = false;
		}
		t = 0;
		currentActiveIndex = 0;
		nextActiveIndex = 1;
		waterMat = water.GetComponent<Renderer> ().material;
		startColor = waterMat.color;
	}

	void Update() {
		if (t < 1.0f) {
			t += Time.deltaTime * 0.1f;
			float lerpValue = Mathf.Lerp (1, 0, t);
			SpriteRenderer currentChildRenderer = transform.GetChild (currentActiveIndex).GetComponent<SpriteRenderer>();
			SpriteRenderer nextChildRenderer = transform.GetChild (nextActiveIndex).GetComponent<SpriteRenderer>();
			currentChildRenderer.color = new Color (255, 255, 255, lerpValue);
			nextChildRenderer.color = new Color (255, 255, 255, 1 - lerpValue);
			if (currentActiveIndex == 0)
				waterMat.SetColor ("_Color", new Color(lerpValue * startColor.r, startColor.g + (1-lerpValue)*(1-startColor.g), startColor.b + (1-lerpValue)*(1-startColor.b)));
			else
				waterMat.SetColor ("_Color", new Color((1-lerpValue) * startColor.r, startColor.g + (1-startColor.g)*(lerpValue), startColor.b + (1-startColor.b)*(lerpValue)));
		} else {
			t = 0;
			isActiveBackground [currentActiveIndex] = false;
			isActiveBackground [nextActiveIndex] = true;
			currentActiveIndex = nextActiveIndex;
			nextActiveIndex = currentActiveIndex == transform.childCount - 1 ? 0 : currentActiveIndex + 1;
		}
	}

}
