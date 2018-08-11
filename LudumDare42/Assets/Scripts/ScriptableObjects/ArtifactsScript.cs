using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Artifact", menuName = "Artifact")]
public class ArtifactsScript : ScriptableObject {

	public string artifactName;
	[TextArea] public string trueDescription;
	[TextArea] public string overDescription;
	[TextArea] public string underDescription;
	public bool hasBeenBought;
	public int moneyValue;
	public int spaceNeeded;
	public CollectionsScript collection;
	public Sprite graphic;
}
