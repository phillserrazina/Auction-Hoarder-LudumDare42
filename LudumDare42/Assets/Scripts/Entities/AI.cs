using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

	// VARIABLES

	private GameManager gameManager;
	public bool madeChoice;

	private void Start()
	{
		gameManager = FindObjectOfType<GameManager> ();
	}

	IEnumerator RaiseOffer(int value)
	{
		madeChoice = true;

		float timeToWait = Random.Range (1.5f, 7.5f);

		yield return new WaitForSeconds (timeToWait);

		gameManager.currentOffer += value;
		gameManager.aiIsWinning = true;
		gameManager.playerIsWinning = false;
		gameManager.currentState = GameManager.States.PRE_TURN;
	}
}
