using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHider : MonoBehaviour
{

	private GameObject mainCamera;
	private RaycastHit oldHit;

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

		hits = Physics.RaycastAll(transform.position, direction * -1);

		Debug.Log("nb hits = " + hits.Length);

		foreach(RaycastHit hit in hits) {

			if(!hit.transform.gameObject.GetComponent<Collectable>().isHidden) {
				StartCoroutine(setMaterialTransparent(hit.transform.gameObject));
				hit.transform.gameObject.GetComponent<Collectable>().isHidden = true;
			}
		}

	}

	IEnumerator setMaterialTransparent(GameObject go) {

		Debug.Log("start hidding");

		float fadeTime = 1.0f;
		var rend = go.GetComponent<Renderer>();

		//set material to transparent
		StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Fade);

		var startColor = Color.white;
		var endColor = new Color(1, 1, 1, 0);

		for (float t = 0.0f; t < fadeTime; t += Time.deltaTime) {
			rend.material.color = Color.Lerp(startColor, endColor, t / fadeTime);
			yield return null;
		}

	}

	private void setMaterialOpaque() {

	}
}
