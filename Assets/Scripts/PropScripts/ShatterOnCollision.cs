///////////////////////////////////////////////
// DEPRECATED, CODE MOVED TO COLLECTABLE SCRIPT
///////////////////////////////////////////////
// Shatter Toolkit
// Copyright 2015 Gustav Olsson
using System.Collections;
using UnityEngine;

namespace ShatterToolkit.Helpers {
	public class ShatterOnCollision : MonoBehaviour {
		public float requiredVelocity = 1.0f;
		public float cooldownTime = 0.5f;

		protected float timeSinceInstantiated;

		public void Update() {
			timeSinceInstantiated += Time.deltaTime;
		}

		public void OnCollisionEnter(Collision collision) {

			if (timeSinceInstantiated >= cooldownTime) {
				if (collision.relativeVelocity.magnitude >= requiredVelocity) {

					if (collision.gameObject.tag == "Player") {

						//Debug.Log("hit from player");
						if (gameObject.GetComponent<Collectable>().stuckResistance > collision.gameObject.GetComponent<Player>().stuckPower) {

							if(gameObject.GetComponent<Collectable>().shatterResistance < collision.gameObject.GetComponent<Player>().shatterPower) {
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
}