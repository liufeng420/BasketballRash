using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowControl : MonoBehaviour {
	public Transform master;

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 pos = master.position;
		pos.y = 0.01f;
		transform.position = pos;
	}
}
