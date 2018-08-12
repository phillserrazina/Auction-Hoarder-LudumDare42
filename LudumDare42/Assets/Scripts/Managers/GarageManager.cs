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
	private int spaceOccupied = 0;
	private int spaceTotal;

	[Header("Objects")]
	public GameObject slotPrefab;
	public GridLayoutGroup inventoryPanel;
	public GameObject skipButton;
	public GameObject menuButton;

	[Header("Texts")]
	public Text finalScoreText;
	public Text garageText;

	private List<GameObject> slotPrefabList = new List<GameObject>();

	// FUNCTIONS

	void Start()
	{
		artifactsList = FindObjectOfType<PlayerScript> ().garage;
		spaceTotal = FindObjectOfType<PlayerScript> ().totalGarageSpace;
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
	}

	IEnumerator HandleSpace()
	{
		spaceOccupied = 0;

		for (int i = 0; i < artifactsList.Count; i++)
		{
			skipButton.SetActive (true);
			menuButton.SetActive (false);

			slotPrefab.GetComponent<GarageSlot> ().assignedArtifact = artifactsList [i];
			slotPrefab.GetComponent<Image> ().sprite = artifactsList [i].graphic;

			var newSlot = Instantiate (slotPrefab, slotPrefab.transform.position, slotPrefab.transform.rotation);
			newSlot.transform.SetParent (inventoryPanel.transform, false);

			slotPrefabList.Add (newSlot);

			// finalScore += artifactsList [i].moneyValue;
			spaceOccupied += artifactsList[i].spaceNeeded;

			if (waitTime)
				yield return new WaitForSeconds (0.4f);
		}

		waitTime = false;

		skipButton.SetActive (false);
		menuButton.SetActive (true);
	}

	public void SkipWaitTime()
	{
		waitTime = false;
		skipButton.SetActive (false);
		menuButton.SetActive (true);
	}

	public void GoToScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}
}
