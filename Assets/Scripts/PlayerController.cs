using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// extra effects to be instantiated
	public Transform exploPrefab;
	public Transform smokePrefab;

	public float speed;
	public float increaseSizeSpeed = 0.05f;
	public float power = 1;
	public float weight = 3;

	private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		rb.AddForce(movement * speed);
	}

	private void OnCollisionEnter(Collision collision) {

		if (collision.gameObject.GetComponent<Collectable>() != null) {

			float resistance = collision.gameObject.GetComponent<Collectable>().resistance;
			float collisionWeight = collision.gameObject.GetComponent<Collectable>().weight;

			bool isCollectable = collision.gameObject.GetComponent<Collectable>().isCollectable;


			//stick to player
			if (collisionWeight < weight && isCollectable) {

				//Debug.Log("Stuck");

				Destroy(collision.rigidbody);
				collision.collider.isTrigger = true;

				//remove trigger on collider
				//StartCoroutine(SetTrigger(collision.gameObject));

				collision.gameObject.transform.SetParent(gameObject.transform);

			}

		}
	}

	IEnumerator SetTrigger(GameObject piece) {
		yield return new WaitForSeconds(3);
		//piece.collider = "Collectable";
		piece.GetComponent<Collider>().isTrigger = false;
	}

	IEnumerator SetCollectable(GameObject piece, float resistance, float weight) {
		yield return new WaitForSeconds(3);
		piece.tag = "Collectable";
		piece.AddComponent<Collectable>();
		piece.GetComponent<Collectable>().resistance = resistance;
		piece.GetComponent<Collectable>().weight = weight;
	}

}
