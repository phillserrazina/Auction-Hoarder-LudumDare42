using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// VARIABLES

	public ArtifactsScript[] artifacts;

	[Header("Auction Variables")]
	public bool auctionIsHappening;
	public Image artifactGraphic;
	public ArtifactsScript currentArtifact;

	private bool allArtifactsHaveBeenBought = false;

	private PlayerScript player;

	// FUNCTIONS

	// Use this for initialization
	void Start () 
	{
		player = FindObjectOfType<PlayerScript> ();

		ResetArtifacts ();
	}

	// Update is called once per frame
	void Update () 
	{
		AuctionManager ();
	}

	private void AuctionManager()
	{
		allArtifactsHaveBeenBought = !CheckForAvailableArtifacts ();

		// If no auctions are currently happening ...
		if (auctionIsHappening == false)
		{
			if (!allArtifactsHaveBeenBought)
			{
				// ... get a new artifact
				currentArtifact = GetNewArtifact ();

				// Update the "Current Artifact" sprite,
				artifactGraphic.sprite = currentArtifact.graphic;

				// And now the auction is happening.
				auctionIsHappening = true;
			}
			else
			{
				Debug.Log ("No Artifacts Left!");
			}
		}

		// If an auction is already happening then ...
		else
		{
			// ... and the Current Artifact has already been bought ...
			if (currentArtifact.hasBeenBought)
			{
				// ... then end this auction
				auctionIsHappening = false;
			}

			// If the Current Artifact hasn't been bought yet, then wait.
		}
	}

	public void BuyArtifact()
	{
		// If the player has enough money ...
		if (player.availableMoney > currentArtifact.moneyValue)
		{
			// ... remove the cost from the Player's funds ...
			player.availableMoney -= currentArtifact.moneyValue;

			// ... and mark the artifact has bought.
			currentArtifact.hasBeenBought = true;
		}

		else
		{
			Debug.Log ("Insuficient Funds!");
		}
	}


	private ArtifactsScript GetNewArtifact()
	{
		ArtifactsScript newArtifact = artifacts [Random.Range (0, artifacts.Length)];

		if (newArtifact.hasBeenBought)
		{
			GetNewArtifact ();
		}

		return newArtifact;
	}

	private bool CheckForAvailableArtifacts()
	{
		// Go through all the artifacts
		for (int i = 0; i < artifacts.Length; i++)
		{
			// If there is at least one that hasn't been bought ...
			if (artifacts[i].hasBeenBought == false)
			{
				// ... there are artifacts available
				return true;
			}
		}

		// If all artifacts have been bought, then
		// there are no available artifacts
		return false;
	}

	public void ResetArtifacts()
	{
		for (int i = 0; i < artifacts.Length; i++)
		{
			artifacts [i].hasBeenBought = false;
		}
	}
}
