using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// VARIABLES

	public enum States
	{
		PRE_TURN,
		TURN
	}

	public States currentState;

	public float timer;
	public bool playerIsWinning;
	public bool aiIsWinning;
	public ArtifactsScript[] artifacts;

	[Header("Auction Variables")]
	public bool auctionIsHappening;
	public int initialOffer;
	public int currentOffer;
	public Image artifactGraphic;
	public ArtifactsScript currentArtifact;

	[Header("Auction Interface")]
	public Text offerText;
	public Text raiseValueText;
	public Text raiseUnitText;
	public Text timerText;

	[Header("Player Interface")]
	public Text playerMoneyText;
	public Text garageText;

	private bool allArtifactsHaveBeenBought = false;

	private PlayerScript player;
	private OfferManager offerManager;
	private AI enemy;

	// FUNCTIONS

	// Use this for initialization
	void Start () 
	{
		player = FindObjectOfType<PlayerScript> ();
		offerManager = FindObjectOfType<OfferManager> ();
		enemy = FindObjectOfType<AI> ();

		currentState = States.PRE_TURN;

		ResetArtifacts ();
	}

	// Update is called once per frame
	void Update () 
	{
		offerText.text = currentOffer.ToString ();
		raiseValueText.text = offerManager.offerValue.ToString ();
		raiseUnitText.text = offerManager.raiseValue.ToString ();
		playerMoneyText.text = player.availableMoney.ToString ();
		garageText.text = string.Format ("{0}/{1}", player.garageSpaceOccupied, player.totalGarageSpace);
		timerText.text = timer.ToString ("F0");

		timer -= Time.deltaTime;

		StateManager ();
	}

	private void StateManager()
	{
		
		switch (currentState) 
		{
		case States.PRE_TURN:

			timer = 10;
			AuctionManager ();

			currentState = States.TURN;

			break;
		case States.TURN:

			int aiOffer = (int)((float)initialOffer * ((float)initialOffer / (float)currentOffer + (Random.Range (0.5f, 1.5f))));

			if (enemy.madeChoice == false)
			{
				Debug.Log (aiOffer);

				if ((initialOffer / currentOffer) < 0.2)
				{
					if (Random.value < 0.25)
					{
						enemy.StartCoroutine ("RaiseOffer", aiOffer);
					}
				}
				else if ((initialOffer / currentOffer) < 0.4)
				{
					if (Random.value < 0.5)
					{
						enemy.StartCoroutine ("RaiseOffer", aiOffer);
					}
				}
				else if ((initialOffer / currentOffer) < 0.6)
				{
					if (Random.value < 0.7)
					{
						enemy.StartCoroutine ("RaiseOffer", aiOffer);
					}
				}
				else if ((initialOffer / currentOffer) < 0.8)
				{
					if (Random.value < 0.9)
					{
						enemy.StartCoroutine ("RaiseOffer", aiOffer);
					}
				}
				else if ((initialOffer / currentOffer) <= 1.0)
				{
					enemy.StartCoroutine ("RaiseOffer", aiOffer);
				}
			}

			if (timer <= 0)
			{
				if (playerIsWinning)
				{
					BuyArtifact ();
				}
				else if (aiIsWinning)
				{
					offerManager.SkipOffer ();
				}
			}

			break;
		default:

			Debug.LogWarning ("Unrecognized state! Set to Start by default.");

			break;
		}
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

				// Get value of Initial Offer
				currentOffer = offerManager.GetNewOffer();
				initialOffer = currentOffer;

				player.availableMoney -= currentOffer;

				Debug.Log (string.Format("Current Artifacts Value: {0}; Current Offer: {1}", currentArtifact.moneyValue, currentOffer));

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
			// ... put the artifact in the Player's garage ...
			player.garageSpaceOccupied += currentArtifact.spaceNeeded;
			player.garage.Add (currentArtifact);

			// ... and mark the artifact has bought.
			currentArtifact.hasBeenBought = true;
			auctionIsHappening = false;
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
			if (artifacts[i].hasBeenBought == false && player.availableMoney >= artifacts[i].moneyValue && (player.totalGarageSpace - player.garageSpaceOccupied) >= artifacts[i].spaceNeeded)
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
