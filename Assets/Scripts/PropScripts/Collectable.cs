using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
	public bool isCollectable = true;
	//public bool isCollected = false;
	public float shatterResistance = 1;
	public float stuckResistance = 1;
	public float earnPoints = 1;

	private Vector3 _scale;

	public void enable(float pResistance, float pWeight) {
		StartCoroutine(enableCollectable(pResistance, pWeight));
	}

	private IEnumerator enableCollectable(float pShatterResistance, float pStuckResistance) {
		yield return new WaitForSeconds(3);
		gameObject.tag = "Collectable";
		isCollectable = true;
		shatterResistance = pShatterResistance;
		stuckResistance = pStuckResistance;
	}
}
