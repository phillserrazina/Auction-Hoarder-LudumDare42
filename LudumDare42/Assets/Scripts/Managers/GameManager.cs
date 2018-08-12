using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// VARIABLES

	public enum States
	{
		PRE_TURN,
		TURN
	}

	public States currentState;

	public float timerTotal = 7;
	private float timer;
	public bool playerIsWinning;
	public bool aiIsWinning;
	public ArtifactsScript[] artifacts;

	[Header("Auction Variables")]
	public bool auctionIsHappening;
	public int initialOffer;
	public int currentOffer;
	public string artifactDescription;
	public Image artifactGraphic;
	public ArtifactsScript currentArtifact;

	[Header("Auction Interface")]
	public Text offerText;
	public Text raiseValueText;
	public Text raiseUnitText;
	public Text timerText;
	public Text artifactDescriptionText;

	[Header("Player Interface")]
	public Text playerMoneyText;

	private bool allArtifactsHaveBeenBought = false;
	private bool gameOver;

	private PlayerScript player;
	private OfferManager offerManager;
	private AI enemy;
	private AudioManager audioManager;

	// FUNCTIONS

	// Use this for initialization
	void Start () 
	{
		player = FindObjectOfType<PlayerScript> ();
		offerManager = FindObjectOfType<OfferManager> ();
		enemy = FindObjectOfType<AI> ();
		audioManager = FindObjectOfType<AudioManager> ();

		player.availableMoney = player.totalMoney;

		gameOver = false;
		currentState = States.PRE_TURN;

		ResetArtifacts ();
	}

	// Update is called once per frame
	void Update () 
	{
		// UI Updates
		offerText.text = currentOffer.ToString ();
		raiseValueText.text = offerManager.offerValue.ToString ();
		raiseUnitText.text = offerManager.raiseValue.ToString ();
		playerMoneyText.text = player.availableMoney.ToString ();
		timerText.text = timer.ToString ("F0");
		artifactDescriptionText.text = artifactDescription;

		if (Input.GetKeyUp(KeyCode.Escape))
		{
			SceneManager.LoadScene ("MainMenuScene");
		}

		if (gameOver != true)
		{
			// Decrease Timer Overtime
			timer -= Time.deltaTime;

			StateManager ();
		}
		else
		{
			SceneManager.LoadScene ("GarageScene");
		}
			
	}

	private void StateManager()
	{
		
		switch (currentState) 
		{
		// Before the actual turn time starts ...
		case States.PRE_TURN:

			// ... reset the timer ...
			timer = timerTotal;

			// ... and the AI ...
			enemy.madeChoice = false;
			enemy.StopAllCoroutines ();

			// ... and check how the auction is going.
			AuctionManager ();

			// Then, go to the actual turn;
			currentState = States.TURN;

			break;
		case States.TURN:

			CalculateAIMove ();

			// If the timer reaches 0 ...
			if (timer <= 0)
			{
				// ... and the Player is winning the bidding ...
				if (playerIsWinning)
				{
					// ... buy the artifact.
					BuyArtifact ();
				}

				// ... and the AI is winning the bidding ...
				else if (aiIsWinning)
				{
					// Skip the offer.
					AIBuyArtifact();
				}
			}

			// If the auction isn't happening anymore ...
			if (auctionIsHappening == false)
			{
				// ... make a new turn.
				currentState = States.PRE_TURN;
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

				// Update the "Current Artifact" sprite,
				artifactGraphic.sprite = currentArtifact.graphic;

				// And now the auction is happening.
				auctionIsHappening = true;
			}
			else
			{
				Debug.Log ("No Artifacts Left!");
				gameOver = true;
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
			// ... take that money away, ...
			player.availableMoney -= currentOffer;

			// ... put the artifact in the Player's garage ...
//			player.garageSpaceOccupied += currentArtifact.spaceNeeded;
			player.garage.Add (currentArtifact);

			audioManager.Play ("WonBidSE");

			// ... and mark the artifact has bought.
			currentArtifact.hasBeenBought = true;
			auctionIsHappening = false;
		}
	}

	public void AIBuyArtifact()
	{
		audioManager.Play ("LoseBidSE");

		// Mark the artifact has bought.
		currentArtifact.hasBeenBought = true;
		auctionIsHappening = false;
	}

	private void CalculateAIMove()
	{
		// Formula for the ammount of money that the AI bids.
		int aiOffer = (int)((float)initialOffer * ((float)initialOffer / (float)currentOffer - Random.Range (0.0f, ((float)initialOffer / (float)currentOffer))));

		float howBigIsCurrentOffer = ((float)initialOffer / (float)currentOffer);

		// If the AI hasn't made a choice yet ...
		if (enemy.madeChoice == false)
		{
			// ... and the offer is already at least 80% bigger than
			// what the original offer was, ...
			if (howBigIsCurrentOffer < 0.2)
			{
				// ... there's a 40% chance ...
				if (Random.value < 0.4)
				{
					// ... that the AI raises the offer.
					enemy.StartCoroutine ("RaiseOffer", aiOffer);
				}
				else
				{
					// AI folds.
					enemy.madeChoice = true;
				}
			}

			// ... and the offer is already at least 60% bigger than
			// what the original offer was, ...
			else if (howBigIsCurrentOffer < 0.4)
			{
				// ... there's a 60% chance ...
				if (Random.value < 0.6)
				{
					// ... that the AI raises the offer.
					enemy.StartCoroutine ("RaiseOffer", aiOffer);
				}
				else
				{
					// AI folds.
					enemy.madeChoice = true;
				}
			}

			// ... and the offer is already at least 40% bigger than
			// what the original offer was, ...
			else if (howBigIsCurrentOffer < 0.6)
			{
				// ... there's a 80% chance ...
				if (Random.value < 0.8)
				{
					// ... that the AI raises the offer.
					enemy.StartCoroutine ("RaiseOffer", aiOffer);
				}
				else
				{
					// AI folds.
					enemy.madeChoice = true;
				}
			}

			// ... and the offer is already at least 20% bigger than
			// what the original offer was, ...
			else if (howBigIsCurrentOffer < 0.8)
			{
				// ... there's a 95% chance ...
				if (Random.value < 0.95)
				{
					// ... that the AI raises the offer.
					enemy.StartCoroutine ("RaiseOffer", aiOffer);
				}
				else
				{
					// AI folds.
					enemy.madeChoice = true;
				}
			}

			// ... and the offer is still the original offer ...
			else if (howBigIsCurrentOffer <= 1.0)
			{
				// ... the AI will always raise the offer.
				enemy.StartCoroutine ("RaiseOffer", aiOffer);
			}
		}
	}

	private ArtifactsScript GetNewArtifact()
	{
		List<ArtifactsScript> availableArtifactsList = new List<ArtifactsScript>();

		// Go through all the artifacts
		for (int i = 0; i < artifacts.Length; i++)
		{
			// If the artifact has NOT been bought ...
			if (!artifacts[i].hasBeenBought)
			{
				// ... and the player has money and space for it ...
				if (player.availableMoney >= artifacts[i].moneyValue)
					// Add it to the available list
					availableArtifactsList.Add (artifacts [i]);
			}
		}

		ArtifactsScript newArtifact = availableArtifactsList [Random.Range (0, availableArtifactsList.Count)];

		return newArtifact;
	}

	private bool CheckForAvailableArtifacts()
	{
		// Go through all the artifacts
		for (int i = 0; i < artifacts.Length; i++)
		{
			// If there is at least one that hasn't been bought, ...
			if (artifacts[i].hasBeenBought == false)
			{
				// ... and the player has money and space for it ...
				if (player.availableMoney >= artifacts[i].moneyValue)
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
