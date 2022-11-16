using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTrigger : MonoBehaviour
{
	public float splashVelocityThreshold;
	public static SplashTrigger Singleton;

	[NonSerialized]
	public float waterLevel;
	
	private void Awake()
	{
		Singleton = this;
		Transform t = transform;
		waterLevel = t.position.y + t.localScale.y / 2;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Buoyancy>() == null) return;

		Rigidbody rb = other.attachedRigidbody;
		if (!(rb.velocity.magnitude > splashVelocityThreshold ||
		      rb.angularVelocity.magnitude > splashVelocityThreshold)) return;
		
		SplashManager.Singleton.OnSplash(other.gameObject,
			other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position),
			rb.velocity);
	}
}
