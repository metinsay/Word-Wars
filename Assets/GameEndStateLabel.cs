using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndStateLabel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		switch (GameState.winner) {
		case 1:
			gameObject.GetComponent<Text>().text = "Player 1 won!";
			break;
		case 2:
			gameObject.GetComponent<Text>().text = "Player 1 won!";
			break;
		default:
			gameObject.GetComponent<Text>().text = "It's a tie!";
			break;
		}

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
