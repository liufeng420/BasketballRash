using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketRing : MonoBehaviour {
	public ParticleSystem effectObject;
	// Use this for initialization
	void Start () {
		if(effectObject)
		{
			effectObject.Stop();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(effectObject && other.gameObject.tag == "Ball")
		{
			effectObject.Play();
		}
	}
}
