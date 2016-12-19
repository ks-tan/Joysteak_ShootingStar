using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public GameObject helpOverlay;
	public GameObject startButton;
	public GameObject helpButton;
	bool isHelp = false;

	public void toggleHelp() {
		if (isHelp) {
			helpOverlay.SetActive (false);
			startButton.SetActive (true);
			helpButton.SetActive (true);
			isHelp = false;
		} else {
			helpOverlay.SetActive (true);
			startButton.SetActive (false);
			helpButton.SetActive (false);
			isHelp = true;
		}
	}

	public void startGame() {
		SceneManager.LoadScene ("CharSelect", LoadSceneMode.Single);
	}

}
