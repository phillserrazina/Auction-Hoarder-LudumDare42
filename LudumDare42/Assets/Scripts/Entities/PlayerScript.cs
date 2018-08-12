using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {

	// VARIABLES

	public int totalMoney;
	public int availableMoney;
	public List<ArtifactsScript> garage;
	public int garageSpaceOccupied;
	public int totalGarageSpace;

	#region Singleton

	public static PlayerScript instance;

	private void Awake()
	{
		if (SceneManager.GetActiveScene().name == "AuctionScene")
		{
			DontDestroyOnLoad (gameObject);
		}
	}

	#endregion
}
