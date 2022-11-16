using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Buoyancy>() == null) return;
		
		Destroy(other.gameObject);
	}
}
