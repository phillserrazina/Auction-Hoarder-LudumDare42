using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GarageManager : MonoBehaviour {

	// VARIABLES

	private List<ArtifactsScript> artifactsList;
	private bool allItemsInCheck;
	private bool waitTime = true;
	private int finalScore = 0;

	public GameObject slotPrefab;
	public GridLayoutGroup inventoryPanel;
	public GameObject skipButton;
	public GameObject menuButton;
	public Text finalScoreText;

	// FUNCTIONS

	void Start()
	{
		artifactsList = FindObjectOfType<PlayerScript> ().garage;
	}

	void Update()
	{
		finalScoreText.text = finalScore.ToString ();

		if (allItemsInCheck == false)
		{
			StartCoroutine ("HandleSpace");
			allItemsInCheck = true;
		}
	}

	IEnumerator HandleSpace()
	{
		for (int i = 0; i < artifactsList.Count; i++)
		{
			skipButton.SetActive (true);
			menuButton.SetActive (false);

			slotPrefab.GetComponent<Image> ().sprite = artifactsList [i].graphic;

			var newSlot = Instantiate (slotPrefab, slotPrefab.transform.position, slotPrefab.transform.rotation);
			newSlot.transform.SetParent (inventoryPanel.transform, false);

			finalScore += artifactsList [i].moneyValue;

			if (waitTime)
				yield return new WaitForSeconds (0.4f);
		}

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
