﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelTimer : MonoBehaviour {
	
	public Text timerText;
	public float sceneTime; // seconds
	private float timeRemaining;
	private GameManager gm;
	private HuntBoardManager huntBoard;

	void Start() {		
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		huntBoard = GameObject.Find ("HuntScene").GetComponent<HuntBoardManager> ();
		timeRemaining = sceneTime;
		StartCoroutine (Countdown ());
	}

	IEnumerator Countdown() {	
		while (timeRemaining >= 0f) {
			timeRemaining -= Time.deltaTime;
			int seconds = Mathf.CeilToInt (timeRemaining);
			timerText.text = string.Format ("0:{0}", seconds.ToString("D2"));
			yield return null;
		}
		Debug.Log ("End Scene!");		
		huntBoard.LoadLabScene ();
	}



}
