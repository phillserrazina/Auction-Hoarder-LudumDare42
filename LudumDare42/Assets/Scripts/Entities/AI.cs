using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

	// VARIABLES

	private GameManager gameManager;
	public bool madeChoice;
	public GameObject bidGraphic;

	private void Start()
	{
		gameManager = FindObjectOfType<GameManager> ();
	}

	IEnumerator RaiseOffer(int value)
	{
		madeChoice = true;

		float timeToWait = Random.Range (1.5f, 4.0f);

		yield return new WaitForSeconds (timeToWait);

		ShowBidGraphic ();

		gameManager.currentOffer += value;
		gameManager.aiIsWinning = true;
		gameManager.playerIsWinning = false;
		gameManager.currentState = GameManager.States.PRE_TURN;
	}

	private void ShowBidGraphic()
	{
		float leastX = -7;
		float mostX = 7;

		float leastY = -3;
		float mostY = 3;

		Vector3 pos = new Vector3 (Random.Range (leastX, mostX), Random.Range (leastY, mostY), bidGraphic.transform.position.z);

		Instantiate (bidGraphic, pos, bidGraphic.transform.rotation);
	}
}
