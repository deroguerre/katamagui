using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public bool isCollectable = false;
	public bool isCollected = false;
	public float resistance = 1;
	public float weight = 1;

	private Vector3 _scale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//if(isCollected) {
		//	transform.localScale = new Vector3(1,1,1);
		//}
    }

	public void enable(float pResistance, float pWeight) {
		//Debug.Log("Collectable enabled");
		StartCoroutine(enableCollectable(pResistance, pWeight));
	}

	private IEnumerator enableCollectable(float pResistance, float pWeight) {
		yield return new WaitForSeconds(2);
		gameObject.tag = "Collectable";
		isCollectable = true;
		resistance = pResistance;
		weight = pWeight;
		//Destroy(gameObject.GetComponent<Rigidbody>());
	}
}
