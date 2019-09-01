// Shatter Toolkit
// Copyright 2015 Gustav Olsson
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

						Debug.Log("hit from player");
						if (gameObject.GetComponent<Collectable>().stuckResistance > collision.gameObject.GetComponent<Player>().stuckPower) {
							StartShatter(collision);
						}

					}

					if (collision.gameObject.tag != "Player") {
						StartShatter(collision);
					}

					//Check if collectable component exist and the object is collectable
					//if (gameObject.GetComponent<Collectable>() != null) {
					//	if (gameObject.GetComponent<Collectable>().isCollectable == false) {
					//		StartShatter(collision);
					//	}
					//} else {
					//	StartShatter(collision);
					//}
				}
			}
		}

		public void StartShatter(Collision collision) {

			if (gameObject.GetComponent<Rigidbody>().isKinematic) {
				gameObject.GetComponent<Rigidbody>().isKinematic = false;
				gameObject.GetComponent<MeshCollider>().convex = true;
			}

			if (gameObject.GetComponent<AudioSource>() != null) {
				AudioSource sound = gameObject.GetComponent<AudioSource>();
				sound.Play();
			}

			// Find the new contact point
			foreach (ContactPoint contact in collision.contacts) {
				// Make sure that we don't shatter if another object in the hierarchy was hit
				if (contact.otherCollider == collision.collider) {
					//if (gameObject.GetComponent<Rigidbody>().isKinematic) {
					//	gameObject.GetComponent<Rigidbody>().isKinematic = false;
					//}
					contact.thisCollider.SendMessage("Shatter", contact.point, SendMessageOptions.DontRequireReceiver);

					break;
				}
			}

		}
	}
}