using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	// Use this for initialization
	void Start () {

        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.z = Random.Range(-30, 30);
        transform.rotation = Quaternion.Euler(rotationVector);



	}

	// Update is called once per frame
	void Update () {

	}
}
