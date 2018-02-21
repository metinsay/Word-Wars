using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Main : MonoBehaviour {
	// Use this for initialization
	void Start () {
        string[] words = System.IO.File.ReadAllLines(@"Assets/data/dictionary.txt");
        string[] letters = System.IO.File.ReadAllLines(@"Assets/data/letters.txt");

	}

	// Update is called once per frame
	void Update () {

	}
}
