using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectHider : MonoBehaviour
{

	private GameObject mainCamera;
	private List<GameObject> oldHits = new List<GameObject>();

	// private List<RaycastHit> OLLLDHits = new List<RaycastHit>();

	// Start is called before the first frame update
	void Start()
    {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}

    // Update is called once per frame
    void Update()
    {
		RaycastHit[] hits;

		Vector3 direction = transform.position - mainCamera.transform.position;
		float distance = Vector3.Distance(transform.position, mainCamera.transform.position);

		Debug.DrawRay(transform.position, direction * -1, Color.red);

		hits = Physics.RaycastAll(transform.position, direction * -1, distance);

		//set transparency on hit
		foreach(RaycastHit hit in hits) {

			if(hit.transform.gameObject.GetComponent<Collectable>() != null) {
				if(!hit.transform.gameObject.GetComponent<Collectable>().isHidden) {

					hit.transform.gameObject.GetComponent<Collectable>().isHidden = true;
					StartCoroutine(setMaterialTransparent(hit.transform.gameObject));
					oldHits.Add(hit.transform.gameObject);

				}
			}
		}

		for(int i = 0; i < oldHits.Count(); i++) {

			bool isHitten = false;

			//check object exit raycast
			for(int j = 0; j < hits.Count(); j++) {

				if(hits[j].transform.gameObject == oldHits[i]) {
					isHitten = true;
				}
			}

			//set opaque on exit raycast
			if(!isHitten) {

				if(oldHits[i] == null) {
					oldHits.Remove(oldHits[i]);
				} else {
					setMaterialOpaque(oldHits[i]);
					if(oldHits[i].GetComponent<Collectable>() != null) {
						oldHits[i].GetComponent<Collectable>().isHidden = false;
					}
					oldHits.Remove(oldHits[i]);
					isHitten = false;
				}
			}
		}

	}

	IEnumerator setMaterialTransparent(GameObject go) {

		Debug.Log("start hidding");

		float fadeTime = 0.3f;
		Renderer rend = go.GetComponent<Renderer>();

		//set material to transparent
		StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Fade);

		Color startColor = Color.white;
		Color endColor = new Color(1, 1, 1, 0);

		for (float t = 0.0f; t < fadeTime; t += Time.deltaTime) {
			rend.material.color = Color.Lerp(startColor, endColor, t / fadeTime);
			yield return null;
		}

	}

	private void setMaterialOpaque(GameObject go) {

		var rend = go.GetComponent<Renderer>();

		//set material to opaque
		StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Opaque);
		rend.material.color = Color.white;
	}
}
