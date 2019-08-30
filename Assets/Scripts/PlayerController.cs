using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// extra effects to be instantiated
	public Transform exploPrefab;
	public Transform smokePrefab;

	public Camera MainCamera;
	public float moveSpeed = 50.0f;
	public float drag = 0.5f;
	public float terminalRotationSpeed = 25.0f;
	public Vector3 MoveVector { set; get; }
	public float increaseSizeSpeed = 0.05f;
	public float power = 1;
	public float weight = 3;

	public float jumpHeight = 7f;
	public bool isGrounded;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;

	private Rigidbody thisRigibody;
	private Transform camTransform;
	private Transform CamTarget;

	void Start() {
		thisRigibody = GetComponent<Rigidbody>();
		thisRigibody.maxAngularVelocity = terminalRotationSpeed;
		thisRigibody.drag = drag;
		CamTarget = gameObject.transform.Find("CamTarget");
	}

	private void FixedUpdate() {
		//float moveHorizontal = Input.GetAxis("Horizontal");
		//float moveVertical = Input.GetAxis("Vertical");

		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		input = Vector2.ClampMagnitude(input, 1);

		Vector3 camF = MainCamera.transform.forward;
		Vector3 camR = MainCamera.transform.right;

		camF.y = 0;
		camR.y = 0;
		camF = camF.normalized;
		camR = camR.normalized;

		//Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		//Vector3 movement2 = new Vector3(moveHorizontal, 0.0f, moveVertical) * Time.deltaTime * moveSpeed;
		//Vector3 movement3 = (camF * movement.y + camR * movement.x) * Time.deltaTime;
		Vector3 movement4 = (camF * input.y + camR * input.x) * Time.deltaTime * moveSpeed * 100;

		//thisRigibody.AddForce(movement * moveSpeed);
		thisRigibody.AddForce(movement4);

		if(Input.GetButtonDown("Jump")) {
			Debug.Log("Jump");
			thisRigibody.AddForce(Vector3.up * jumpHeight * 100);
		}

		if(thisRigibody.velocity.y < 0) {
			thisRigibody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		} else if (thisRigibody.velocity.y > 0 && !Input.GetButton("Jump")) {
			thisRigibody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}

		if (CamTarget != null) {
			Debug.Log("Camera targeted");
			CamTarget.transform.position = gameObject.transform.position;
		}
	}

	private void OnCollisionEnter(Collision other) {

		if (other.gameObject.tag == "Ground") {
			isGrounded = true;
		}

		if (other.gameObject.GetComponent<Collectable>() != null) {

			float resistance = other.gameObject.GetComponent<Collectable>().resistance;
			float collisionWeight = other.gameObject.GetComponent<Collectable>().weight;

			bool isCollectable = other.gameObject.GetComponent<Collectable>().isCollectable;

			//stick to player
			if (collisionWeight < weight && isCollectable) {

				//Debug.Log("Stuck");

				Destroy(other.rigidbody);
				other.collider.isTrigger = true;

				//remove trigger on collider
				//StartCoroutine(SetTrigger(collision.gameObject));

				other.gameObject.transform.SetParent(gameObject.transform);

			}

		}
	}

	private void OnCollisionExit(Collision other) {
		if (other.gameObject.tag == "Ground") {
			isGrounded = false;
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
