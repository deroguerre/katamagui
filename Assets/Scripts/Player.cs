using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float increaseSizeSpeed = 0.05f;
	public float power = 1;
	public float weight = 3;


	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter(Collision other) {

		if (other.gameObject.GetComponent<Collectable>() != null) {

			float resistance = other.gameObject.GetComponent<Collectable>().resistance;
			float collisionWeight = other.gameObject.GetComponent<Collectable>().weight;

			bool isCollectable = other.gameObject.GetComponent<Collectable>().isCollectable;

			//stick to player
			if (collisionWeight < weight && isCollectable) {

				//Debug.Log("Stuck");

				other.collider.isTrigger = true;
				Destroy(other.rigidbody);

				other.gameObject.GetComponent<ShatterToolkit.Helpers.HierarchyHandler>().enabled = false;
				other.gameObject.GetComponent<ShatterToolkit.Helpers.ShatterOnCollision>().enabled = false;
				other.gameObject.GetComponent<ShatterToolkit.TargetUvMapper>().enabled = false;
				other.gameObject.GetComponent<ShatterToolkit.ShatterTool>().enabled = false;
				other.gameObject.GetComponent<MeshCollider>().enabled = false;

				//remove trigger on collider
				//StartCoroutine(SetTrigger(collision.gameObject));

				//other.gameObject.transform.SetParent(gameObject.transform);
				other.gameObject.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());


			}

		}
	}

	//IEnumerator SetTrigger(GameObject piece) {
	//	yield return new WaitForSeconds(3);
	//	//piece.collider = "Collectable";
	//	piece.GetComponent<Collider>().isTrigger = false;
	//}

	//IEnumerator SetCollectable(GameObject piece, float resistance, float weight) {
	//	yield return new WaitForSeconds(3);
	//	piece.tag = "Collectable";
	//	piece.AddComponent<Collectable>();
	//	piece.GetComponent<Collectable>().resistance = resistance;
	//	piece.GetComponent<Collectable>().weight = weight;
	//}
}
