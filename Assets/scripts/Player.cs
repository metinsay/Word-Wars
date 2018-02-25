using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Random=System.Random;
using UnityEngine;
using UnityEditor;



public class Player {

    private string word;
    private string[] pos_letters;
    private float[] letter_freqs;
    private float total_freq;
    private string[] words;
    public bool played;
    public string[] letters;


    public Player (string[] pos_letters, float[] letter_freqs, float total_freq, string[] words) {

        this.pos_letters = pos_letters;
        this.letter_freqs = letter_freqs;
        this.total_freq = total_freq;
        this.words = words;
        this.played = false;

        letters = GetNewLetters(new string[20], new bool[20]);
        word = "";


    }

    public string GetWord () {
        return word;
    }

    public void ResetWord () {
        word = "";
    }

    public void AddLetter (string letter) {
        word = word + letter;
    }

    public void RemoveLastLetter () {
        if (word.Length > 0) {
            word = word.Substring(0, word.Length - 1);
        }
    }

    public int Winner (Player p) {
        if (this.word.Length > p.word.Length) {
            return 1;
        } else if (this.word.Length < p.word.Length) {
            return 2;
        } else {
            return 0;
        }
    }

    public string[] GetNewLetters (string[] ls, bool[] change) {
        Random r = new Random();
        for (int i = 0; i < 20; i++) {
            if (!change[i]) {
                double randomValue = r.NextDouble();

                double cumulative = 0.0;
                for (int j = 0; j < pos_letters.Length; j++)
                {
                    cumulative += letter_freqs[j] / total_freq;
                    if (randomValue < cumulative) {
                        ls[i] = pos_letters[j];
                        break;
                    }
                }
            }
        }
        return ls;
    }

    public bool HasValidWord () {
        return Array.BinarySearch(words, word) < 0;
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
