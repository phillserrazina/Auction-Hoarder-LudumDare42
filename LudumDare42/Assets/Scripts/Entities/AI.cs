using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour {

	// VARIABLES

	private GameManager gameManager;
	public bool madeChoice;
	public GameObject bidGraphic;

	public Sprite[] bidGrahicSprites;

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
		float mostX = -3;

		float leastY = -2;
		float mostY = 2;

		Sprite newGraphic = bidGrahicSprites [Random.Range (0, bidGrahicSprites.Length - 1)];

		bidGraphic.GetComponentInChildren<Image> ().sprite = newGraphic;

		Vector3 pos = new Vector3 (Random.Range (leastX, mostX), Random.Range (leastY, mostY), bidGraphic.transform.position.z);

		Instantiate (bidGraphic, pos, bidGraphic.transform.rotation);
	}
}
