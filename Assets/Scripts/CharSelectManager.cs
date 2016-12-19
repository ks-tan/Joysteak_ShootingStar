using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class CharSelectManager : MonoBehaviour {

	public Texture2D progressBar;

	private AsyncOperation async = null;

	public void startGame() {
		StartCoroutine(LoadLevel("Main"));
	} 

	private IEnumerator LoadLevel(string Level) {
		async = SceneManager.LoadSceneAsync(Level);
		yield return async;
	}

	private void OnGUI() {
		if (async != null) {
			GUI.DrawTexture (new Rect (0, 0, Camera.current.pixelWidth * async.progress, 10), progressBar);
		}
	}

}
