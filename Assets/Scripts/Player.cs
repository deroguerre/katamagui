using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float stuckPower = 5;
	public float shatterPower = 3;

	private float nextActionTime = 0.0f;
	public float period = 0.1f;
	public int maxDebrisCollected = 100;

	private float _score = 0;
	private float _scoreStep = 10;
	private float _scoreToNextLevel = 20;
	private float _level = 0;
	private Vector3 _scale = new Vector3(1, 1, 1);
	//private List<GameObject> _collectedDebrisList = new List<GameObject>();
	private Queue<GameObject> _collectedDebrisQueue = new Queue<GameObject>();

	private void Update() {

		//fade out collected debris after x seconds
		if (Time.time > nextActionTime) {
			nextActionTime += period;
			//Debug.Log("Next Action !!");
			while(_collectedDebrisQueue.Count > maxDebrisCollected) {
				_collectedDebrisQueue.Peek().AddComponent<MeshFader>();
				_collectedDebrisQueue.Peek().GetComponent<MeshFader>().startFade = true;
				_collectedDebrisQueue.Dequeue();
			}
		}
	}

	//Stuck On Collision
	private void OnCollisionEnter(Collision other) {

		if (other.gameObject.GetComponent<Collectable>() != null) {

			float shatterResistance = other.gameObject.GetComponent<Collectable>().shatterResistance;
			float stuckResistance = other.gameObject.GetComponent<Collectable>().stuckResistance;

			bool isCollectable = other.gameObject.GetComponent<Collectable>().isCollectable;

			//stick to player
			if (stuckResistance < stuckPower && isCollectable) {

				if (other.gameObject.GetComponent<MeshCollider>() != null && other.gameObject.GetComponent<MeshCollider>().convex) {
					other.collider.isTrigger = true;
				}

				//disable useless components
				//disableComponent(other.gameObject);
				destroyComponent(other.gameObject);


				//set transform of collider to player
				other.gameObject.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
				_collectedDebrisQueue.Enqueue(other.gameObject);

				//calculate score/level
				_score += other.gameObject.GetComponent<Collectable>().stuckResistance;
				//_level = 1 + (float)Mathf.Round((_score / xpRateDivider) * 10f) / 10f;

				if(_score >= _scoreToNextLevel) {
					levelUp();
				}

			}

		}
	}

	private void levelUp() {

		_level++;
		_score = 0;
		_scoreStep *= 1.2f;
		_scoreToNextLevel += _scoreStep;

		_scale.x += 2f;
		_scale.y += 2f;
		_scale.z += 2f;

		stuckPower += (_scoreStep / 6);
		shatterPower += (_scoreStep / 6);
		//gameObject.GetComponent<Rigidbody>().mass += (_scoreStep / 4);

		gameObject.GetComponent<MSCameraController>().CameraSettings.orbital.minDistance += 10f;
		gameObject.GetComponent<MSCameraController>().CameraSettings.orbital.maxDistance += 10f;

		//set null the debris of player before scale it
		foreach (GameObject debris in _collectedDebrisQueue) {
			debris.transform.parent = null;
		}

		gameObject.transform.localScale = _scale;
		//transform.localScale = Vector3.Lerp(transform.localScale, _scale, 2.0f * Time.deltaTime);

		foreach (GameObject child in _collectedDebrisQueue) {
			child.transform.parent = gameObject.transform;
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

		//if (go.GetComponent<MeshCollider>() != null)
		//	go.GetComponent<MeshCollider>().enabled = false;

		if (go.GetComponent<Collider>() != null)
			go.GetComponent<Collider>().enabled = false;
	}

	private void destroyComponent(GameObject go) {

		Destroy(go.GetComponent<Rigidbody>());

		if (go.GetComponent<ShatterToolkit.Helpers.HierarchyHandler>() != null)
			Destroy(go.GetComponent<ShatterToolkit.Helpers.HierarchyHandler>());

		if (go.GetComponent<ShatterToolkit.Helpers.ShatterOnCollision>() != null)
			Destroy(go.GetComponent<ShatterToolkit.Helpers.ShatterOnCollision>());

		if (go.GetComponent<ShatterToolkit.TargetUvMapper>() != null)
			Destroy(go.GetComponent<ShatterToolkit.TargetUvMapper>());

		if (go.GetComponent<ShatterToolkit.ShatterTool>() != null)
			Destroy(go.GetComponent<ShatterToolkit.ShatterTool>());

		if (go.GetComponent<Collider>() != null)
			Destroy(go.GetComponent<Collider>());

		if (go.GetComponent<Collectable>() != null)
			Destroy(go.GetComponent<Collectable>());
	}

	void OnGUI() {
		GUI.Label(new Rect(10, 10, 200, 20), "score: " + _score);
		GUI.Label(new Rect(10, 20, 200, 20), "score to next level: " + _scoreToNextLevel);
		GUI.Label(new Rect(10, 30, 200, 20), "level: " + _level);
		GUI.Label(new Rect(10, 40, 200, 20), "scale: " + _scale);

		GUI.Label(new Rect(10, 60, 200, 20), "stuck power: " + stuckPower);
		GUI.Label(new Rect(10, 70, 200, 20), "shatter power: " + shatterPower);
	}
}
