using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BidDestroyer : MonoBehaviour {

	private float timer = 1.0f;

	// Update is called once per frame
	void Update () 
	{
		this.timer -= Time.deltaTime;

		if (this.timer <= 0)
		{
			Destroy (this.gameObject);
		}
	}
}
