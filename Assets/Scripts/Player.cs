using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float score = 0;
	public float stuckPower = 3;
	public float xpRateDivider = 100;

	private float _level = 0;
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

				if (other.gameObject.GetComponent<MeshCollider>().convex) {
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

				other.gameObject.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
				playerChilds.Add(other.gameObject);

				score += other.gameObject.GetComponent<Collectable>().earnPoints;

				_level = 1 + (float)Mathf.Round((score / xpRateDivider) * 10f) / 10f;

				if (_level > _scale.x) {
					_scale.x = _level;
					_scale.y = _level;
					_scale.z = _level;

					//set null the childs of player before scale it
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
	}

	void OnGUI() {
		GUI.Label(new Rect(10, 10, 200, 20), "score: " + score / xpRateDivider);
		GUI.Label(new Rect(10, 20, 200, 20), "level: " + _level);
		GUI.Label(new Rect(10, 30, 200, 20), "scale: " + _scale);
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
