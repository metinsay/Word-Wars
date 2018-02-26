using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Random=System.Random;
using UnityEngine;
 using UnityEngine.UI;
using UnityEditor;



public class Main : MonoBehaviour {

    private Player player1;
    private Player player2;
    private Dictionary<string, Sprite> spriteMap;
    private int turn;
    private string[] letters;
    private int round;
    private float timer;
	private bool gameOver;


	void Start () {


        turn = -1;
        round = 1;
        timer = -1.0f;
		gameOver = false;
        spriteMap = GetSpriteMap();

		Random r = new Random();

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

        player1 = new Player(letters, letter_freqs, total_freq, words, r);
        player2 = new Player(letters, letter_freqs, total_freq, words, r);

        UpdateVisuals(true, false);

	}

    void DecreaseTimer () {
        if (timer > 0) {
            timer -= 1;
        }
    }

	// Update is called once per frame
	void Update () {

        UpdateVisuals(false, false);

        if (timer > 0) {
            timer -= Time.deltaTime;
        }

        if (timer <= 0 && turn != -1) {
			timer = -1;

            if (turn == 1) {
				if (!player2.played) {
					turn = 2;
					timer = 11;
				} else {
					turn = -1;
					timer = -1;
				}
                player1.played = true;
                Debug.Log("Player 1 Timed out!");
            } else {
				if (!player1.played) {
					turn = 1;
					timer = 11;
				} else {
					turn = -1;
					timer = -1;
				}
                player2.played = true;
                Debug.Log("Player 2 Timed out!");
            }
           
        }



        if (player1.played && player2.played) {
            switch (player1.WinnerOfRound(player2)) {
                case 0:
                    player1.IncrementScore();
                    player2.IncrementScore();
                    Debug.Log("It's a tie. " + round.ToString() + ". Player 1: " + player1.GetScore().ToString() + " - Player 2: " + player2.GetScore().ToString());
                    break;
                case 1:
                    player1.IncrementScore();
                    Debug.Log("Player 1 Won! " + round.ToString() + ". Player 1: " + player1.GetScore().ToString() + " - Player 2: " + player2.GetScore().ToString());
                    break;
                case 2:
                    player2.IncrementScore();
                    Debug.Log("Player 2 Won! " + round.ToString() + ". Player 1: " + player1.GetScore().ToString() + " - Player 2: " + player2.GetScore().ToString());
                    break;
                default:
                    break;
            }
            player1.played = false;
            player1.ResetLetters();
            player1.ResetWord();

            player2.played = false;
            player2.ResetLetters();
            player2.ResetWord();

            UpdateVisuals(true, true);

            round += 1;

            if (player1.GetScore() == 3 || player2.GetScore() == 3) {

				gameOver = true;

                if (player1.GetScore() > player2.GetScore()) {
                    Debug.Log("Player 1 won the game!");
                } else if (player1.GetScore() < player2.GetScore()) {
                    Debug.Log("Player 2 won the game!");
                } else if (player1.GetScore() == player2.GetScore()) {
                    Debug.Log("Game compeleted as a tie!");
                }
            }
        }

		if (!gameOver) {
			if (turn == -1) {
				if (Input.GetKeyDown ("left shift") && !player1.played) {
					turn = 1;
					if (!player2.played) {
						timer = 11;
					}
					Debug.Log ("Player 1's Turn");
				} else if (Input.GetKeyDown ("right shift") && !player2.played) {
					turn = 2;
					if (!player1.played) {
						timer = 11;
					}
					Debug.Log ("Player 2's Turn");
				}
			} else {
				if (Input.GetKeyDown ("return")) {
					if (turn == 1) {
						player1.played = true;
						Debug.Log ("Player 1's Turn Ended");

						if (!player2.played) {
							turn = 2;
							timer = 11;
						} else {
							timer = -1;
							turn = -1;
						}
					} else {
						player2.played = true;
						Debug.Log ("Player 2's Turn Ended");

						if (!player1.played) {
							turn = 1;
							timer = 11;
						} else {
							timer = -1;
							turn = -1;
						}
					}

				} else if (Input.GetKeyDown ("backspace")) {
					if (turn == 1) {
						int index = player1.RemoveLastLetter ();
						if (index != -1) {
							GameObject.Find (String.Concat ("1-", index + 1)).GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
						}
						Debug.Log (player1.GetWord ());
					} else {
						int index = player2.RemoveLastLetter ();
						if (index != -1) {
							GameObject.Find (String.Concat ("2-", index + 1)).GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
						}
						Debug.Log (player2.GetWord ());
					}
				}

				for (int i = 0; i < letters.Length; i++) {
					if (Input.GetKeyDown(letters[i].ToLower())) {
						if (turn == 1) {
							if (player1.CanAddLetter(letters[i])) {
								int index = player1.AddLetter(letters[i]);
								GameObject.Find(String.Concat("1-", index+1)).GetComponent<SpriteRenderer>().color = new Color(94.0f/255.0f, 90.0f/255.0f, 90.0f/255.0f, 1.0f);
								Debug.Log(player1.GetWord());
							}
						} else {
							if (player2.CanAddLetter(letters[i])) {
								int index = player2.AddLetter(letters[i]);
								GameObject.Find(String.Concat("2-", index+1)).GetComponent<SpriteRenderer>().color = new Color(94.0f/255.0f, 90.0f/255.0f, 90.0f/255.0f, 1.0f);
								Debug.Log(player2.GetWord());
							}
						}
					}
				}
			}
		}


	}

    void UpdateVisuals (bool updateTiles, bool colorReset) {

        if (updateTiles) {
            for (int i = 0; i < 20; i++) {
                GameObject.Find(String.Concat("1-", i+1)).GetComponent<SpriteRenderer>().sprite = spriteMap[player1.letters[i]];
                GameObject.Find(String.Concat("2-", i+1)).GetComponent<SpriteRenderer>().sprite = spriteMap[player2.letters[i]];

                if (colorReset) {
                    GameObject.Find(String.Concat("1-", i+1)).GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    GameObject.Find(String.Concat("2-", i+1)).GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }



            }
        }

        GameObject.Find("Score").GetComponent<Text>().text = player1.GetScore() + " - " + player2.GetScore();

        GameObject.Find("Player1-Word").GetComponent<Text>().text = player1.GetWord();
        GameObject.Find("Player2-Word").GetComponent<Text>().text = player2.GetWord();

        if (player1.HasValidWord()) {
            GameObject.Find("Player1-Word").GetComponent<Text>().color = Color.green;
        } else {
            GameObject.Find("Player1-Word").GetComponent<Text>().color = Color.red;
        }

        if (player2.HasValidWord()) {
            GameObject.Find("Player2-Word").GetComponent<Text>().color = Color.green;
        } else {
            GameObject.Find("Player2-Word").GetComponent<Text>().color = Color.red;
        }

        if (timer > 0) {
            GameObject.Find("Timer").GetComponent<Text>().text = ((int)timer).ToString();
        } else {
            GameObject.Find("Timer").GetComponent<Text>().text = "";
        }

		if (turn == -1) {
			GameObject.Find("Player1-Turn").GetComponent<Text>().text = "Turn: ";
			GameObject.Find("Player2-Turn").GetComponent<Text>().text = "Turn: ";
		} else if (turn == 1) {
			GameObject.Find("Player1-Turn").GetComponent<Text>().text = "Turn: o";
			GameObject.Find("Player2-Turn").GetComponent<Text>().text = "Turn: ";
		} else {
			GameObject.Find("Player1-Turn").GetComponent<Text>().text = "Turn: ";
			GameObject.Find("Player2-Turn").GetComponent<Text>().text = "Turn: o";
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
