using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// extra effects to be instantiated
	public Transform exploPrefab;
	public Transform smokePrefab;

	public Camera mainCamera;
	public float moveSpeed = 50.0f;

	public float jumpHeight = 7f;
	public bool isGrounded;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;

	private Rigidbody thisRigibody;
	private Transform camTransform;
	private Transform camTarget;

	void Start() {
		thisRigibody = GetComponent<Rigidbody>();
		camTarget = gameObject.transform.Find("CamTarget");
	}

	private void FixedUpdate() {
		//float moveHorizontal = Input.GetAxis("Horizontal");
		//float moveVertical = Input.GetAxis("Vertical");

		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		input = Vector2.ClampMagnitude(input, 1);

		Vector3 camF = mainCamera.transform.forward;
		Vector3 camR = mainCamera.transform.right;

		camF.y = 0;
		camR.y = 0;
		camF = camF.normalized;
		camR = camR.normalized;

		//Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		Vector3 movement = (camF * input.y + camR * input.x) * Time.deltaTime * moveSpeed * 100;

		//thisRigibody.AddForce(movement * moveSpeed);
		thisRigibody.AddForce(movement);

		if(Input.GetButtonDown("Jump")) {
			Debug.Log("Jump");
			thisRigibody.AddForce(Vector3.up * jumpHeight * 100);
		}

		if(thisRigibody.velocity.y < 0) {
			thisRigibody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		} else if (thisRigibody.velocity.y > 0 && !Input.GetButton("Jump")) {
			thisRigibody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}

		if (camTarget != null) {
			camTarget.transform.position = gameObject.transform.position;
		}
	}

	private void OnCollisionEnter(Collision other) {

		if (other.gameObject.tag == "Ground") {
			isGrounded = true;
		}
	}

	private void OnCollisionExit(Collision other) {
		if (other.gameObject.tag == "Ground") {
			isGrounded = false;
		}
	}

}
