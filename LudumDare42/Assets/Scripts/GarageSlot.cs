using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageSlot : MonoBehaviour {

	// VARIABLES

	public ArtifactsScript assignedArtifact;

	private GarageManager garageManager;
	private PlayerScript player;

	// METHOD

	void Start()
	{
		garageManager = FindObjectOfType<GarageManager> ();
		player = FindObjectOfType<PlayerScript> ();
	}

	public void RemoveSlot()
	{
		player.garage.Remove (this.assignedArtifact);

		garageManager.allItemsInCheck = false;
	}
}
