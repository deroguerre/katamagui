using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float stuckPower = 3;

	private Vector3 _scale = new Vector3(1, 1, 1);

	private List<GameObject> playerChilds = new List<GameObject>();

	// Start is called before the first frame update
	void Start() {
		//_scale = new Vector3(scale, scale, scale);
	}

	// Update is called once per frame
	void Update() {
		//if (transform.localScale != _scale) {
		//	transform.localScale = Vector3.Lerp(transform.localScale, _scale, 2.0f * Time.deltaTime);
		//}

	}

	private void OnCollisionEnter(Collision other) {

		if (other.gameObject.GetComponent<Collectable>() != null) {

			float shatterResistance = other.gameObject.GetComponent<Collectable>().shatterResistance;
			float stuckResistance = other.gameObject.GetComponent<Collectable>().stuckResistance;

			bool isCollectable = other.gameObject.GetComponent<Collectable>().isCollectable;

			//stick to player
			if (stuckResistance < stuckPower && isCollectable) {

				if(other.gameObject.GetComponent<MeshCollider>().convex) {
					other.collider.isTrigger = true;
				}
				Destroy(other.rigidbody);

				if (other.gameObject.GetComponent<ShatterToolkit.Helpers.HierarchyHandler>() != null)
					other.gameObject.GetComponent<ShatterToolkit.Helpers.HierarchyHandler>().enabled = false;

				if (other.gameObject.GetComponent<ShatterToolkit.Helpers.ShatterOnCollision>() != null)
					other.gameObject.GetComponent<ShatterToolkit.Helpers.ShatterOnCollision>().enabled = false;

				if (other.gameObject.GetComponent<ShatterToolkit.TargetUvMapper>() != null)
					other.gameObject.GetComponent<ShatterToolkit.TargetUvMapper>().enabled = false;

				if (other.gameObject.GetComponent<ShatterToolkit.ShatterTool>() != null)
					other.gameObject.GetComponent<ShatterToolkit.ShatterTool>().enabled = false;

				if (other.gameObject.GetComponent<MeshCollider>() != null)
					other.gameObject.GetComponent<MeshCollider>().enabled = false;

				//remove trigger on collider
				//StartCoroutine(SetTrigger(collision.gameObject));

				other.gameObject.GetComponent<Collectable>().isCollected = true;
				//other.gameObject.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
				playerChilds.Add(other.gameObject);

				//_scale.x += 0.01f;
				//_scale.y += 0.01f;
				//_scale.z += 0.01f;
				_scale *= 1.01f;

				foreach (GameObject child in playerChilds) {
					child.transform.parent = null;
				}

				gameObject.transform.localScale = _scale;
				//transform.localScale = Vector3.Lerp(transform.localScale, _scale, 2.0f * Time.deltaTime);

				foreach (GameObject child in playerChilds) {
					child.transform.parent = gameObject.transform;
				}

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
