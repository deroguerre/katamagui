using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float stuckPower = 3;
	public float xpRateDivider = 100;

	private float _score = 0;
	private float _level = 0;
	private Vector3 _scale = new Vector3(1, 1, 1);
	private List<GameObject> debrisList = new List<GameObject>();


	//Stuck On Collision
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

				//disable useless components
				disableComponent(other.gameObject);

				//set transform of collider to player
				other.gameObject.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
				debrisList.Add(other.gameObject);

				//calculate score/level
				_score += other.gameObject.GetComponent<Collectable>().earnPoints;
				_level = 1 + (float)Mathf.Round((_score / xpRateDivider) * 10f) / 10f;

				//set scale of player without effect debris
				if (_level > _scale.x) {
					_scale.x = _level;
					_scale.y = _level;
					_scale.z = _level;

					//set null the debris of player before scale it
					foreach (GameObject debris in debrisList) {
						debris.transform.parent = null;
					}

					gameObject.transform.localScale = _scale;
					//transform.localScale = Vector3.Lerp(transform.localScale, _scale, 2.0f * Time.deltaTime);

					foreach (GameObject child in debrisList) {
						child.transform.parent = gameObject.transform;
					}
				}

			}

		}
	}

	private void disableComponent(GameObject go) {

		Destroy(go.GetComponent<Rigidbody>());

		if (go.GetComponent<ShatterToolkit.Helpers.HierarchyHandler>() != null)
			go.GetComponent<ShatterToolkit.Helpers.HierarchyHandler>().enabled = false;

		if (go.GetComponent<ShatterToolkit.Helpers.ShatterOnCollision>() != null)
			go.GetComponent<ShatterToolkit.Helpers.ShatterOnCollision>().enabled = false;

		if (go.GetComponent<ShatterToolkit.TargetUvMapper>() != null)
			go.GetComponent<ShatterToolkit.TargetUvMapper>().enabled = false;

		if (go.GetComponent<ShatterToolkit.ShatterTool>() != null)
			go.GetComponent<ShatterToolkit.ShatterTool>().enabled = false;

		if (go.GetComponent<MeshCollider>() != null)
			go.GetComponent<MeshCollider>().enabled = false;
	}

	void OnGUI() {
		GUI.Label(new Rect(10, 10, 200, 20), "score: " + _score / xpRateDivider);
		GUI.Label(new Rect(10, 20, 200, 20), "level: " + _level);
		GUI.Label(new Rect(10, 30, 200, 20), "scale: " + _scale);
	}
}
