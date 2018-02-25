using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class Main : MonoBehaviour {

    private Player player1;
    private Player player2;
    private Dictionary<string, Sprite> spriteMap;
    private int turn;
    private string[] letters;



	void Start () {


        turn = -1;
        spriteMap = GetSpriteMap();

        string[] words = System.IO.File.ReadAllLines(@"Assets/data/dictionary.txt");
        string[] lf = System.IO.File.ReadAllLines(@"Assets/data/letters.txt");

        float[] letter_freqs = new float[lf.Length];
        letters = new string[lf.Length];
        float total_freq = 0.0f;

        for (int i = 0; i < lf.Length; i++) {
            letters[i] = lf[i].Split(' ')[0];
            letter_freqs[i] = Int32.Parse(lf[i].Split(' ')[1]);
            total_freq += letter_freqs[i];
        }

        player1 = new Player(letters, letter_freqs, total_freq, words);
        player2 = new Player(letters, letter_freqs, total_freq, words);

        for (int i = 0; i < 20; i++) {
            GameObject.Find(String.Concat("1-", i+1)).GetComponent<SpriteRenderer>().sprite = spriteMap[player1.letters[i]];
            GameObject.Find(String.Concat("2-", i+1)).GetComponent<SpriteRenderer>().sprite = spriteMap[player2.letters[i]];
        }


	}

	// Update is called once per frame
	void Update () {

        if (player1.played && player2.played) {
            switch (player1.Winner(player2)) {
                case 0:
                    Debug.Log("It's a tie!");
                    break;
                case 1:
                    Debug.Log("Player 1 Won!");
                    break;
                case 2:
                    Debug.Log("Player 2 Won!");
                    break;
                default:
                    break;
            }
        }

        if (turn == -1) {
            if (Input.GetKeyDown("left shift") && !player1.played) {
                turn = 1;
                Debug.Log("Player 1's Turn");
            } else if (Input.GetKeyDown("right shift") && !player2.played) {
                turn = 2;
                Debug.Log("Player 2's Turn");
            }
        } else {
            if (Input.GetKeyDown("return")) {
                if (turn  == 1) {
                    player1.played = true;
                    Debug.Log("Player 1's Turn Ended");
                    if (player1.HasValidWord()) {
                        player1.ResetWord();
                        Debug.Log("Word is not valid!");
                    }
                } else {
                    player2.played = true;
                    Debug.Log("Player 2's Turn Ended");
                    if (player2.HasValidWord()) {
                        player2.ResetWord();
                        Debug.Log("Word is not valid!");
                    }
                }
                turn = -1;
            } else if (Input.GetKeyDown("backspace")) {
                if (turn  == 1) {
                    player1.RemoveLastLetter();
                    Debug.Log(player1.GetWord());
                } else {
                    player2.RemoveLastLetter();
                    Debug.Log(player2.GetWord());
                }
            }
        }

        if (turn != -1) {
            for (int i = 0; i < letters.Length; i++) {
                if (Input.GetKeyDown(letters[i].ToLower())) {
                    if (turn == 1) {
                        if (player1.CanAddLetter(letters[i])) {
                            player1.AddLetter(letters[i]);
                            Debug.Log(player1.GetWord());
                        }
                    } else {
                        if (player2.CanAddLetter(letters[i])) {
                            player2.AddLetter(letters[i]);
                            Debug.Log(player2.GetWord());
                        }
                    }
                }
            }
        }

	}



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
