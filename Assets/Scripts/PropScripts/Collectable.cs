﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
	public bool isCollectable = true;
	public bool isShatterable = false;
	//public bool isCollected = false;
	public float stuckResistance = 1;
	public float shatterResistance = 1;
	public float requiredVelocity = 1.0f;
	[HideInInspector]
	public float cooldownTime = 0.5f;
	public float earnPoints = 1;

	private Vector3 _scale;


	protected float timeSinceInstantiated;

	public void Update() {
		timeSinceInstantiated += Time.deltaTime;
	}

	public void enable(float pResistance, float pWeight) {
		StartCoroutine(enableCollectable(pResistance, pWeight));
	}

	private IEnumerator enableCollectable(float pShatterResistance, float pStuckResistance) {
		yield return new WaitForSeconds(3);
		gameObject.tag = "Collectable";
		isCollectable = true;
		shatterResistance = pShatterResistance;
		stuckResistance = pStuckResistance;
	}

	public void OnCollisionEnter(Collision collision) {

		if (isShatterable && timeSinceInstantiated >= cooldownTime) {
			if (collision.relativeVelocity.magnitude >= requiredVelocity) {

				if (collision.gameObject.tag == "Player") {

					//Debug.Log("hit from player");
					if (gameObject.GetComponent<Collectable>().stuckResistance > collision.gameObject.GetComponent<Player>().stuckPower) {

						if (gameObject.GetComponent<Collectable>().shatterResistance < collision.gameObject.GetComponent<Player>().shatterPower) {
							StartShatter(collision);
						}
					}

				}

				//shatter on clission with all objects
				//if (collision.gameObject.tag != "Player") {
				//	StartShatter(collision);
				//}

			}
		}
	}

	public void StartShatter(Collision collision) {

		if (gameObject.GetComponent<Rigidbody>() != null) {
			if (gameObject.GetComponent<Rigidbody>().isKinematic) {
				gameObject.GetComponent<Rigidbody>().isKinematic = false;
			}
		} else {
			Rigidbody newRb = gameObject.AddComponent<Rigidbody>();
			if (gameObject.GetComponent<Collectable>() != null) {
				newRb.mass = gameObject.GetComponent<Collectable>().stuckResistance;
			}
		}

		if (gameObject.GetComponent<MeshCollider>() == null) {
			gameObject.AddComponent<MeshCollider>();
		}
		gameObject.GetComponent<MeshCollider>().convex = true;

		//Debug.Break();

		// Find the new contact point
		foreach (ContactPoint contact in collision.contacts) {
			// Make sure that we don't shatter if another object in the hierarchy was hit
			if (contact.otherCollider == collision.collider) {

				contact.thisCollider.SendMessage("Shatter", contact.point, SendMessageOptions.DontRequireReceiver);

				break;
			}
		}

		if (gameObject.GetComponent<BoxCollider>() != null) {
			Destroy(gameObject.GetComponent<BoxCollider>());
		}

		if (gameObject.transform.Find("Colliders") != null) {
			Destroy(gameObject.transform.Find("Colliders").gameObject);
		}
	}

}
