using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour
{
	public Text TextGoal;
	public AudioSource TransitionSound;

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine("DisplayGoal");
	}

    // Update is called once per frame
    void Update()
    {

	}

	IEnumerator DisplayGoal() {
		yield return new WaitForSeconds(0.5f);
		TransitionSound.Play();
		yield return new WaitForSeconds(1.5f);
		Color whiteColor = Color.white;
		TextGoal.color = whiteColor;
		yield return new WaitForSeconds(5);
		TextGoal.enabled = false;
	}
}
