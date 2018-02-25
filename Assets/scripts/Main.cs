using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Random=System.Random;


public class Main : MonoBehaviour {

    public Sprite A_sprite;
    public Sprite B_sprite;
    public Sprite C_sprite;
    public Sprite D_sprite;
    public Sprite E_sprite;
    public Sprite F_sprite;
    public Sprite G_sprite;
    public Sprite H_sprite;
    public Sprite I_sprite;
    public Sprite J_sprite;
    public Sprite K_sprite;
    public Sprite L_sprite;
    public Sprite M_sprite;
    public Sprite N_sprite;
    public Sprite O_sprite;
    public Sprite P_sprite;
    public Sprite Q_sprite;
    public Sprite R_sprite;
    public Sprite S_sprite;
    public Sprite T_sprite;
    public Sprite U_sprite;
    public Sprite W_sprite;
    public Sprite X_sprite;
    public Sprite V_sprite;
    public Sprite Y_sprite;
    public Sprite Z_sprite;

    private float[] letter_freqs;
    private string[] letters;
    private float total_freq;
    private string[] words;
    private Dictionary<string, Sprite> spriteMap;
    private string[] player1_letters;
    private string[] player2_letters;
    private string player1_word;
    private string player2_word;
    private int turn;
    private bool player1_played;
    private bool player2_played;



	void Start () {


        turn = -1;
        player1_word = "";
        player2_word = "";
        player1_played = false;
        player2_played = false;
        spriteMap = GetSpriteMap();

        words = System.IO.File.ReadAllLines(@"Assets/data/dictionary.txt");
        string[] lf = System.IO.File.ReadAllLines(@"Assets/data/letters.txt");

        letter_freqs = new float[lf.Length];
        letters = new string[lf.Length];
        total_freq = 0.0f;

        for (int i = 0; i < lf.Length; i++) {
            letters[i] = lf[i].Split(' ')[0];
            letter_freqs[i] = Int32.Parse(lf[i].Split(' ')[1]);
            total_freq += letter_freqs[i];
        }

        player1_letters = GetNewLetters(new string[20], new bool[20]);
        player2_letters = GetNewLetters(new string[20], new bool[20]);

        for (int i = 0; i < 20; i++) {
            GameObject.Find(String.Concat("1-", i+1)).GetComponent<SpriteRenderer>().sprite = spriteMap[player1_letters[i]];
            GameObject.Find(String.Concat("2-", i+1)).GetComponent<SpriteRenderer>().sprite = spriteMap[player2_letters[i]];
        }


	}

	// Update is called once per frame
	void Update () {

        if (player1_played && player2_played) {
            if (player1_word.Length > player2_word.Length) {
                Debug.Log("Player 1 won!");
            } else if (player1_word.Length < player2_word.Length) {
                Debug.Log("Player 2 won!");
            } else {
                Debug.Log("It's a tie!");
            }

        }

        if (turn == -1) {
            if (Input.GetKeyDown("left shift") && !player1_played) {
                turn = 1;
                Debug.Log("Player 1's Turn");
            } else if (Input.GetKeyDown("right shift") && !player2_played) {
                turn = 2;
                Debug.Log("Player 2's Turn");
            }
        } else {
            if (Input.GetKeyDown("return")) {
                if (turn  == 1) {
                    player1_played = true;
                    Debug.Log("Player 1's Turn Ended");
                    if (Array.BinarySearch(words, player1_word) < 0) {
                        player1_word = "";
                        Debug.Log("Word is not valid!");
                    }
                } else {
                    player2_played = true;
                    Debug.Log("Player 2's Turn Ended");
                    if (Array.BinarySearch(words, player2_word) < 0) {
                        player2_word = "";
                        Debug.Log("Word is not valid!");
                    }
                }
                turn = -1;
            }
        }

        if (turn != -1) {
            for (int i = 0; i < letters.Length; i++) {
                if (Input.GetKeyDown(letters[i].ToLower())) {
                    if (turn == 1) {
                        if (CanAddLetter(player1_word, player1_letters, letters[i])) {
                            player1_word += letters[i];
                            Debug.Log(player1_word);
                        }
                    } else {
                        if (CanAddLetter(player2_word, player2_letters, letters[i])) {
                            player2_word += letters[i];
                            Debug.Log(player2_word);
                        }
                    }
                }
            }
        }

	}

    bool CanAddLetter (string word, string[] pos_letters, string letter) {
        Dictionary<string, int> pos_map = new Dictionary<string, int>();
        Dictionary<string, int> word_map = new Dictionary<string, int>();
        for (int i = 0; i < pos_letters.Length; i++) {
            string l = pos_letters[i].ToString();
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

    string[] GetNewLetters (string[] ls, bool[] change) {
        Random r = new Random();
        for (int i = 0; i < 20; i++) {
            if (!change[i]) {
                double randomValue = r.NextDouble();

                double cumulative = 0.0;
                for (int j = 0; j < letters.Length; j++)
                {
                    cumulative += letter_freqs[j] / total_freq;
                    if (randomValue < cumulative) {
                        ls[i] = letters[j];
                        break;
                    }
                }
            }
        }
        return ls;
    }

    Dictionary<string, Sprite> GetSpriteMap() {
        Dictionary<string, Sprite> spriteMap = new Dictionary<string, Sprite>();
        spriteMap.Add("A", A_sprite);
        spriteMap.Add("B", B_sprite);
        spriteMap.Add("C", C_sprite);
        spriteMap.Add("D", D_sprite);
        spriteMap.Add("E", E_sprite);
        spriteMap.Add("F", F_sprite);
        spriteMap.Add("G", G_sprite);
        spriteMap.Add("H", H_sprite);
        spriteMap.Add("I", I_sprite);
        spriteMap.Add("J", J_sprite);
        spriteMap.Add("K", K_sprite);
        spriteMap.Add("L", L_sprite);
        spriteMap.Add("M", M_sprite);
        spriteMap.Add("N", N_sprite);
        spriteMap.Add("O", O_sprite);
        spriteMap.Add("P", P_sprite);
        spriteMap.Add("Q", Q_sprite);
        spriteMap.Add("R", R_sprite);
        spriteMap.Add("S", S_sprite);
        spriteMap.Add("T", T_sprite);
        spriteMap.Add("U", U_sprite);
        spriteMap.Add("W", W_sprite);
        spriteMap.Add("X", X_sprite);
        spriteMap.Add("V", V_sprite);
        spriteMap.Add("Y", Y_sprite);
        spriteMap.Add("Z", Z_sprite);
        return spriteMap;
    }



}
