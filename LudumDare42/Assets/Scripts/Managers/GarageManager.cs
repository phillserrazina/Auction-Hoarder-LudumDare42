using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GarageManager : MonoBehaviour {

	// VARIABLES

	private List<ArtifactsScript> artifactsList;
	public bool allItemsInCheck;
	private bool waitTime = true;
	private int finalScore = 0;
	public int spaceOccupied = 0;
	private int spaceTotal;
	private bool canLeave = false;
	private bool onSpaceHandle = false;
	private bool onMoneyHandle = false;

	[Header("Objects")]
	public GameObject slotPrefab;
	public GridLayoutGroup inventoryPanel;
	public GameObject skipButton;
	public GameObject menuButton;
	public GameObject continueButton;
	public GameObject artifactDisplayObject;
	public GameObject spaceOccupiedObject;
	public GameObject artifactWorthObject;
	public GameObject finalScoreObject;

	[Header("Texts")]
	public Text finalScoreText;
	public Text garageText;
	public Text artifactNameText;
	public Text artifactDescriptionText;
	public Image artifactGraphic;
	public Text artifactWorthText;

	private List<GameObject> slotPrefabList = new List<GameObject>();
	public GameObject currentSlot;
	private PlayerScript player;
	private AudioManager audioManager;

	// FUNCTIONS

	void Start()
	{
		artifactsList = FindObjectOfType<PlayerScript> ().garage;
		spaceTotal = FindObjectOfType<PlayerScript> ().totalGarageSpace;
		player = FindObjectOfType<PlayerScript> ();
		audioManager = FindObjectOfType<AudioManager> ();
	}

	void Update()
	{
		finalScoreText.text = finalScore.ToString ();
		garageText.text = string.Format ("{0}/{1}", spaceOccupied, spaceTotal);

		if (allItemsInCheck == false)
		{
			if (slotPrefabList != null)
			{
				for (int i = 0; i < slotPrefabList.Count; i++)
				{
					Destroy (slotPrefabList [i]);
				}

				slotPrefabList.Clear ();
			}

			StartCoroutine ("HandleSpace");
			allItemsInCheck = true;
		}

		if ((spaceOccupied <= spaceTotal) && !onMoneyHandle && !onSpaceHandle)
			canLeave = true;
		else
			canLeave = false;

		if (canLeave)
			continueButton.SetActive (true);
	}

	IEnumerator HandleSpace()
	{
		onSpaceHandle = true;

		spaceOccupied = 0;

		for (int i = 0; i < artifactsList.Count; i++)
		{
			skipButton.SetActive (true);
			continueButton.SetActive (false);

			slotPrefab.GetComponent<GarageSlot> ().assignedArtifact = artifactsList [i];
			slotPrefab.GetComponent<Image> ().sprite = artifactsList [i].graphic;

			var newSlot = Instantiate (slotPrefab, slotPrefab.transform.position, slotPrefab.transform.rotation);
			newSlot.transform.SetParent (inventoryPanel.transform, false);

			slotPrefabList.Add (newSlot);

			spaceOccupied += artifactsList[i].spaceNeeded;

			if (waitTime)
			{
				audioManager.Play ("PlaceBidSE");
				yield return new WaitForSeconds (0.4f);
			}
		}

		waitTime = false;

		skipButton.SetActive (false);

		onSpaceHandle = false;
	}

	IEnumerator HandleMoney()
	{
		onMoneyHandle = true;

		waitTime = true;

		for (int i = 0; i < artifactsList.Count; i++)
		{
			skipButton.SetActive (true);
			menuButton.SetActive (false);

			slotPrefab.GetComponent<GarageSlot> ().assignedArtifact = artifactsList [i];
			slotPrefab.GetComponent<Image> ().sprite = artifactsList [i].graphic;

			var newSlot = Instantiate (slotPrefab, slotPrefab.transform.position, slotPrefab.transform.rotation);
			newSlot.transform.SetParent (inventoryPanel.transform, false);

			slotPrefabList.Add (newSlot);

			finalScore += artifactsList [i].moneyValue;

			if (waitTime)
			{
				audioManager.Play ("PlaceBidSE");
				yield return new WaitForSeconds (0.4f);
			}
				
		}

		finalScore -= (player.totalMoney - player.availableMoney);

		skipButton.SetActive (false);
		menuButton.SetActive (true);

		onMoneyHandle = false;
	}

	public void SkipWaitTime()
	{
		waitTime = false;
		skipButton.SetActive (false);

		if (onSpaceHandle && canLeave)
			continueButton.SetActive (true);

		if (onMoneyHandle)
			menuButton.SetActive (true);
	}

	public void RemoveSlot()
	{
		ArtifactsScript artifact = currentSlot.GetComponent<GarageSlot> ().assignedArtifact;
		FindObjectOfType<PlayerScript> ().garage.Remove (artifact);

		artifactWorthText.text = artifact.moneyValue.ToString();

		artifactWorthObject.SetActive (true);
		artifactDisplayObject.SetActive (false);
	}

	public void Back()
	{
		menuButton.SetActive (true);
		spaceOccupiedObject.SetActive (true);
		artifactDisplayObject.SetActive (false);
	}

	public void ContinueAfterShowValue()
	{
		artifactWorthObject.SetActive (false);
		spaceOccupiedObject.SetActive (true);

		allItemsInCheck = false;
	}

	public void ContinueAfterEnoughSpace()
	{
		spaceOccupiedObject.SetActive (false);
		finalScoreObject.SetActive (true);
		menuButton.SetActive (true);

		if (slotPrefabList != null)
		{
			for (int i = 0; i < slotPrefabList.Count; i++)
			{
				Destroy (slotPrefabList [i]);
			}

			slotPrefabList.Clear ();
		}

		StartCoroutine ("HandleMoney");
	}

	public void GoToScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}
}
