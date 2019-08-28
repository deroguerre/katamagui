using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public bool isCollectable = false;
	public float resistance = 1;
	public float weight = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void enable(float pResistance, float pWeight) {
		//Debug.Log("Collectable enabled");
		StartCoroutine(enableCollectable(pResistance, pWeight));
	}

	private IEnumerator enableCollectable(float pResistance, float pWeight) {
		yield return new WaitForSeconds(3);
		isCollectable = true;
		resistance = pResistance;
		weight = pWeight;
		//Destroy(gameObject.GetComponent<Rigidbody>());
	}
}
