using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferManager : MonoBehaviour {

	// VARIABLES
	public int offerValue;
	public int raiseValue;

	private GameManager gameManager;
	private PlayerScript player;

	// METHODS

	private void Start()
	{
		gameManager = FindObjectOfType<GameManager> ();
		player = FindObjectOfType<PlayerScript> ();
	}

	public void SkipOffer()
	{
		gameManager.AIBuyArtifact ();
		gameManager.auctionIsHappening = false;
		offerValue = 0;
		raiseValue = 1;
	}

	public void PlaceOffer()
	{
		if (offerValue > 0 && player.availableMoney > gameManager.currentOffer && player.availableMoney > offerValue)
		{
			gameManager.playerIsWinning = true;
			gameManager.aiIsWinning = false;
			gameManager.currentOffer += offerValue;
			offerValue = 0;
			raiseValue = 1;
			gameManager.currentState = GameManager.States.PRE_TURN;
		}
		else
		{
			Debug.Log ("Insuficient Funds!");
		}
	}

	public void RaiseOffer()
	{
		offerValue += raiseValue;
	}

	public void DecreaseOffer()
	{
		if ((offerValue - raiseValue) >= 0)
			offerValue -= raiseValue;
		else
			offerValue = 0;
	}

	public void ChangeRaiseValue()
	{
		switch (raiseValue) 
		{
		case 1:
			raiseValue = 10;
			break;
		case 10:
			raiseValue = 100;
			break;
		case 100:
			raiseValue = 1000;
			break;
		case 1000:
			raiseValue = 10000;
			break;
		case 10000:
			raiseValue = 1;
			break;
		default:
			Debug.LogWarning ("Unrecognized raise value! Set to 1 by default.");
			raiseValue = 1;
			break;
		}
	}

	public int GetNewOffer()
	{
		bool makeUnfairOffer = false;
		bool makePerfectOffer = false;
		int offer = 0;

		// 15% chance of making an unfair offer
		// i.e. a rusty sword being mistaken by a rare sword
		// and gets auctioned at a higher price.
		if (Random.value < 0.15)
		{
			makeUnfairOffer = true;
		}

		// 15% chance of making a perfect offer
		// i.e. a rare sword being mistaken by a rusty sword
		// and gets auctioned at a smaller price.
		if (Random.value < 0.15 && makeUnfairOffer == false)
		{
			makePerfectOffer = true;
		}

		// Get value for <$500 Artifacts
		if (gameManager.currentArtifact.moneyValue <= 500)
		{
			offer = Random.Range (20, 150);

			// If it is an unfair offer
			if (makeUnfairOffer)
			{
				offer = Random.Range (100, 300);
			}
		}

		// Get value for <$1000 Artifacts
		else if (gameManager.currentArtifact.moneyValue <= 1000)
		{
			offer = Random.Range (150, 300);

			// If it is an unfair offer
			if (makeUnfairOffer)
			{
				offer = Random.Range (500, 800);
			}

			// If it is a perfect offer
			else if (makePerfectOffer)
			{
				offer = Random.Range (50, 201);
			}
		}

		// Get value for <$5000 Artifacts
		else if (gameManager.currentArtifact.moneyValue <= 5000)
		{
			offer = Random.Range (600, 3000);

			// If it is an unfair offer
			if (makeUnfairOffer)
			{
				offer = Random.Range (2000, 3500);
			}

			// If it is a perfect offer
			else if (makePerfectOffer)
			{
				offer = Random.Range (50, 201);
			}
		}

		// Get value for <$10,000 Artifacts
		else if (gameManager.currentArtifact.moneyValue <= 10000)
		{
			offer = Random.Range (1000, 3000);

			// If it is an unfair offer
			if (makeUnfairOffer)
			{
				offer = Random.Range (5000, 7000);
			}

			// If it is a perfect offer
			else if (makePerfectOffer)
			{
				offer = Random.Range (500, 1500);
			}
		}

		// Get value for <$50,000 Artifacts
		else if (gameManager.currentArtifact.moneyValue <= 50000)
		{
			offer = Random.Range (5000, 10000);

			// If it is an unfair offer
			if (makeUnfairOffer)
			{
				offer = Random.Range (20000, 30000);
			}

			// If it is a perfect offer
			else if (makePerfectOffer)
			{
				offer = Random.Range (1000, 4000);
			}
		}

		// Get value for <$100,000 Artifacts
		else if (gameManager.currentArtifact.moneyValue <= 100000)
		{
			offer = Random.Range (10000, 30000);

			// If it is a perfect offer
			if (makePerfectOffer)
			{
				offer = Random.Range (5000, 15000);
			}
		}

		return offer;
	}
}
