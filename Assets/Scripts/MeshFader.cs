using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFader : MonoBehaviour
{
	public bool startFade = false;
	public bool destroyIt = false;
	private bool fadeOut = false;

	void Update() {
		if (fadeOut) return;

		// wait until rigibody is spleeping
		if (startFade) {
			fadeOut = true;
			StartCoroutine(FadeOut());
		}
	}

	IEnumerator FadeOut() {
		float fadeTime = 2.0f;
		var rend = GetComponent<Renderer>();

		//set material to transparent
		StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Fade);

		var startColor = Color.white;
		var endColor = new Color(1, 1, 1, 0);

		for (float t = 0.0f; t < fadeTime; t += Time.deltaTime) {
			rend.material.color = Color.Lerp(startColor, endColor, t / fadeTime);
			yield return null;
		}

		if(destroyIt) {
			Destroy(gameObject);
		}
	}
}
