using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random=System.Random;


public class Player {

    private string word;
    private int score;
    private string[] pos_letters;
    private float[] letter_freqs;
    private float total_freq;
    private string[] words;
    public bool played;
    public string[] letters;
    private bool[] lettersUsed;
	private Random r;

	public Player (string[] pos_letters, float[] letter_freqs, float total_freq, string[] words, Random r) {

        this.pos_letters = pos_letters;
        this.letter_freqs = letter_freqs;
        this.total_freq = total_freq;
        this.words = words;
        this.played = false;
		this.r = r;

        lettersUsed = new bool[20];
        for (int i = 0; i < 20; i++) {
            lettersUsed[i] = true;
        }
        letters = new string[20];

        GetNewLetters();
        lettersUsed = new bool[20];
        word = "";
        score = 0;


    }

    public int GetScore () {
        return score;
    }

    public void IncrementScore () {
        score += 1;
    }

    public void ResetScore() {
        score = 0;
    }

    public string GetWord () {
        return word;
    }

    public void ResetWord () {
        word = "";
        lettersUsed = new bool[20];
    }

    public void ResetLetters () {
        GetNewLetters();

    }

    public int AddLetter (string letter) {
        word = word + letter;

        int i = 0;
        foreach (string l in letters) {
            if (letter.Equals(l) && !lettersUsed[i]) {
                lettersUsed[i] = true;
                return i;
            }
            i += 1;
        }
        return -1;
    }

    public int RemoveLastLetter () {
        if (word.Length > 0) {

            int i = 0;
            foreach (string l in letters) {
                if (word[word.Length - 1].ToString().Equals(l) && lettersUsed[i]) {
                    lettersUsed[i] = false;
                    word = word.Substring(0, word.Length - 1);
                    return i;
                }
                i += 1;
            }
        }
        return -1;
    }

    public int WinnerOfRound (Player p) {
        if (this.HasValidWord() && p.HasValidWord()) {
            if (this.word.Length > p.word.Length) {
                return 1;
            } else if (this.word.Length < p.word.Length) {
                return 2;
            } else {
                return 0;
            }
        } else if (this.HasValidWord()) {
            return 1;
        } else if (p.HasValidWord()) {
            return 2;
        } else {
            return 0;
        }
    }

    public void GetNewLetters () {
        
        for (int i = 0; i < 20; i++) {
            if (lettersUsed[i]) {
                double randomValue = r.NextDouble();

                double cumulative = 0.0;
                for (int j = 0; j < pos_letters.Length; j++)
                {
                    cumulative += letter_freqs[j] / total_freq;
                    if (randomValue < cumulative) {
                        letters[i] = pos_letters[j];
                        break;
                    }
                }
            }
        }
    }

    public bool HasValidWord () {
        return Array.BinarySearch(words, word) >= 0;
    }

    public bool CanAddLetter (string letter) {
        Dictionary<string, int> pos_map = new Dictionary<string, int>();
        Dictionary<string, int> word_map = new Dictionary<string, int>();
        for (int i = 0; i < letters.Length; i++) {
            string l = letters[i].ToString();
            if (!pos_map.ContainsKey(l)) {
                pos_map[l] = 0;
            } else {
                pos_map[l] += 1;
            }
        }
        string newWord = word + letter;
        for (int i = 0; i < newWord.Length; i++) {
            string l = newWord[i].ToString();

            if (!word_map.ContainsKey(l)) {
                word_map[l] = 0;
            } else {
                word_map[l] += 1;
            }
        }


        foreach(KeyValuePair<string, int> entry in word_map){

            if (!pos_map.ContainsKey(entry.Key)) {
                return false;
            } else if (pos_map[entry.Key] < entry.Value) {
                return false;
            }
        }

        return true;
    }

}
