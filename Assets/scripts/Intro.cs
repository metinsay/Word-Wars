using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Intro : MonoBehaviour {


	private bool downloaded;
	public GameObject button;
	// Use this for initialization
	void Start () {
		downloaded = false;
		StartCoroutine(DownloadWords());

	}
	
	// Update is called once per frame
	void Update () {
		button.SetActive (downloaded);
	}


	public void OnMouseUp () {
		Application.LoadLevel("Main");
	}


	IEnumerator DownloadWords()
	{
		WWW w = new WWW("https://raw.githubusercontent.com/metinsay/Word-Wars/master/Assets/data/dictionary.txt");
		yield return w;
		if (w.error != null)
		{
			Debug.Log("Error .. " +w.error);
			// for example, often 'Error .. 404 Not Found'
		}
		else
		{
			Debug.Log("Found ... ==>" +w.text.Length +"<==");
			// don't forget to look in the 'bottom section'
			// of Unity console to see the full text of
			// multiline console messages.
		}

		GameState.words = w.text.Split (new string[] { "\n" }, StringSplitOptions.None);
		StartCoroutine(DownloadLetters ());
	}

	IEnumerator DownloadLetters () {

		WWW w = new WWW ("https://raw.githubusercontent.com/metinsay/Word-Wars/master/Assets/data/letters.txt");
		yield return w;
		if (w.error != null) {
			Debug.Log ("Error .. " + w.error);
			// for example, often 'Error .. 404 Not Found'
		} else {
			Debug.Log ("Found ... ==>" + w.text.Length + "<==");
			// don't forget to look in the 'bottom section'
			// of Unity console to see the full text of
			// multiline console messages.
		}


		GameState.lf = w.text.Split (new string[] { "\n" }, StringSplitOptions.None);
		downloaded = true;
	}
}
