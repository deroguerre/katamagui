using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject player;

	private Vector3 offset;

	private Transform thisTransform;

    // Start is called before the first frame update
    void Start()
    {
		thisTransform = gameObject.GetComponent<Transform>();
		offset = transform.position - player.transform.position;
    }

	private void LateUpdate() {
		transform.position = player.transform.position + offset;
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	private void FixedUpdate() {
		float moveHorizontal = Input.GetAxis("Mouse X");
		float moveVertical = Input.GetAxis("Mouse Y");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		thisTransform.Rotate(movement);

		//thisRigibody.AddForce(movement * moveSpeed);
	}
}
