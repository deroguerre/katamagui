﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// extra effects to be instantiated
	public Transform exploPrefab;
	public Transform smokePrefab;

	public float moveSpeed = 50.0f;
	public float drag = 0.5f;
	public float terminalRotationSpeed = 25.0f;
	public Vector3 MoveVector { set; get; }
	public float increaseSizeSpeed = 0.05f;
	public float power = 1;
	public float weight = 3;

	private Rigidbody thisRigibody;
	private Transform camTransform;
	private Transform CamTarget;
	public Camera MainCamera;

	void Start() {
		thisRigibody = GetComponent<Rigidbody>();
		thisRigibody.maxAngularVelocity = terminalRotationSpeed;
		thisRigibody.drag = drag;
		CamTarget = gameObject.transform.Find("CamTarget");
	}

	private void FixedUpdate() {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		input = Vector2.ClampMagnitude(input, 1);

		Vector3 camF = MainCamera.transform.forward;
		Vector3 camR = MainCamera.transform.right;

		camF.y = 0;
		camR.y = 0;
		camF = camF.normalized;
		camR = camR.normalized;

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		Vector3 movement2 = new Vector3(moveHorizontal, 0.0f, moveVertical) * Time.deltaTime * moveSpeed;
		Vector3 movement3 = (camF * movement.y + camR * movement.x) * Time.deltaTime;
		Vector3 movement4 = (camF * input.y + camR * input.x) * Time.deltaTime * moveSpeed * 100;

		//thisRigibody.AddForce(movement * moveSpeed);
		thisRigibody.AddForce(movement4);

		if (CamTarget != null) {
			Debug.Log("Camera targeted");
			CamTarget.transform.position = gameObject.transform.position;
		}
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
