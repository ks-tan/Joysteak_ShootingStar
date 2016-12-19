using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public Text scoreText;
	public Text moneyText;
	public GameObject commends;
	public GameObject restartPrompt;
	public GameObject floor;
	public GameObject playerPrefab;
	public GameObject currentPlayer;
	public List<string> commendText = new List<string> ();
	public int platformsPassed;
	public bool isRacing;

	private GameObject racer;
	private float totalScore;
	private float t = 0;
	private bool isCommending;
	private bool isCommendFading;
	private bool isRaceTimerStarted;
	private int highScore;
	private int money;
	private int racingTimer;

	const string PLAYERPREFAB_HIGHSCORE = "HighScore";
	const string PLAYERPREFAB_MONEY = "Money";
	const string PLAYERPREFAB_FIRSTPLAY = "IsFirstPlay";

	void Start() {
		if (instance == null)
			instance = this;
		highScore = PlayerPrefs.GetInt (PLAYERPREFAB_HIGHSCORE);
		money = PlayerPrefs.GetInt (PLAYERPREFAB_MONEY);
		scoreText.transform.FindChild ("HighScore").GetComponent<Text> ().text = highScore.ToString ();
		moneyText.transform.GetComponent<Text> ().text = money.ToString ();
		platformsPassed = 0;
	}

	void Update () {
		if (Input.GetKey (KeyCode.Escape))
			SceneManager.LoadScene ("Menu");

		if (isCommendFading) {
			if (t < 1.0f) {
				t += Time.deltaTime;
				float alphaValue = Mathf.Lerp (1, 0, t);
				commends.GetComponent<Text> ().color = new Color (255, 255, 255, alphaValue);
			} else {
				t = 0;
				isCommendFading = false;
			}
		}

		if (platformsPassed > 0 && platformsPassed % 10 == 0 && !isRacing && 
			!currentPlayer.GetComponent<PlayerManager>().isSuperJump &&
			currentPlayer.transform.position.y > Camera.main.gameObject.transform.position.y-2)
		spawnRacer ();

		if (isRacing && racer!=null) {
			if (currentPlayer.transform.position.x > racer.transform.position.x && racer.GetComponent<SpriteRenderer>().isVisible && isRaceTimerStarted) {
				applySlowMo (0.3f);
				Destroy (racer.gameObject);
				SpecialEffects.instance.makeExplosion (racer.transform.position, Color.red);
				SpecialEffects.instance.makeExplosion (racer.transform.position, Color.red);
				SpecialEffects.instance.makeExplosion (racer.transform.position, Color.red);
				isRacing = false;
				isRaceTimerStarted = false;
				commendPlayer ("YEEE HAWW");
			}
		}
	}

	void spawnRacer() {
		isRacing = true;
		racer = (GameObject) Instantiate(playerPrefab, currentPlayer.transform);
		Vector3 cameraPos = Camera.main.gameObject.transform.position;
		racer.transform.position = new Vector3(cameraPos.x, cameraPos.y, currentPlayer.transform.position.y);
		SpecialEffects.instance.makeExplosion (racer.transform.position, Color.red);
		SpecialEffects.instance.makeExplosion (racer.transform.position, Color.red);
		SpecialEffects.instance.makeExplosion (racer.transform.position, Color.red);
		applySlowMo (0.3f);
		racer.GetComponent<PlayerManager> ().isAuto = true;
		racer.GetComponent<PlayerManager> ().isRacer = true;
		racer.GetComponent<SpriteRenderer> ().color = Color.red;
		racer.transform.FindChild ("Trail").gameObject.SetActive(false);
		racer.transform.FindChild ("RacerTrail").gameObject.SetActive(true);
		commendPlayer ("Race!");
		Invoke ("startRaceTimer", 5);
	}

	void startRaceTimer() {
		isRaceTimerStarted = true;
	}
		
	void applySlowMo(float time) {
		Time.timeScale = 0.2f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		Invoke ("revertSlowMo", time);
	}

	void revertSlowMo() {
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.02f;
	}

	public void commendPlayer(string text = null){
		if (text == null)
			commends.GetComponent<Text> ().text = commendText [Random.Range (0, 3)];
		else
			commends.GetComponent<Text> ().text = text;
		isCommending = true;
		isCommendFading = true;
		t = 0;
	}

	public void updateMoney() {
		moneyText.transform.GetComponent<Text> ().text = (++money).ToString ();
	}

	public void updateScore(float jumpMultiplier){
		if (isCommending) {
			totalScore += jumpMultiplier * 50;
			isCommending = false;
		} else {
			totalScore += jumpMultiplier;
		}
		int totalScoreInt = (int)totalScore;
		scoreText.GetComponent<Text> ().text = totalScoreInt.ToString();
	}

	public IEnumerator restartAfterTime(float time) {
		yield return new WaitForSeconds(time);
		if (totalScore > highScore) PlayerPrefs.SetInt (PLAYERPREFAB_HIGHSCORE, (int)totalScore);
		PlayerPrefs.SetInt (PLAYERPREFAB_MONEY, money);
		SceneManager.LoadScene ("Main");
	}
}
