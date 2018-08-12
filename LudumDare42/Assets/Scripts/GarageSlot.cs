using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageSlot : MonoBehaviour {

	// VARIABLES

	public ArtifactsScript assignedArtifact;

	private GarageManager garageManager;

	// METHOD

	void Start()
	{
		garageManager = FindObjectOfType<GarageManager> ();
	}

	public void DisplayItem()
	{
		garageManager.artifactNameText.text = assignedArtifact.artifactName;
		garageManager.artifactDescriptionText.text = assignedArtifact.trueDescription;
		garageManager.artifactGraphic.sprite = assignedArtifact.graphic;

		garageManager.menuButton.SetActive (false);
		garageManager.spaceOccupiedObject.SetActive (false);
		garageManager.artifactDisplayObject.SetActive (true);

		garageManager.currentSlot = this.gameObject;
	}
}
