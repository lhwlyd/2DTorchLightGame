﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * This is the manager of the entire game, it initializes the board using the 
 * board manager, and handles re-play and game informations.
 */
public class GameManager : MonoBehaviour {

	//No more than one game manager
	public static GameManager instance = null;

	public BoardManager boardScript;

	private int level = 1;

	public float turnDelay = 0.1f;

    public int playerHealth = 100;

	[HideInInspector]public int mapSize = 30;

	[HideInInspector]public bool playersTurn = false;

	private List<Enemy> enemies;

	//UI control
	public float levelStartingDelay = 2f;
	private Text levelText;
	private GameObject levelImage;
	public bool doingSetup;

	public GameObject miniMap;

    // Records
    private float bestRecord;
    public float survivedTime;

	// Use this for initialization
	void Awake () {

        // Keep only 1 instance of the game manager.
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);


		enemies = new List<Enemy> ();
		boardScript = GetComponent<BoardManager> ();
        level = 1;
        survivedTime = 0;
		InitGame ();
	}

	void InitGame(){
		doingSetup = true;

		mapSize = level * 5 + 30;

		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text>();
        levelText.text = "Galaxy #" + (char)(Random.Range(33, 126)) + (char)(Random.Range(33, 126)) + (char)(Random.Range(48, 126)) + (char)(Random.Range(33, 126));	
		levelImage.SetActive(true);
		miniMap = GameObject.Find ("MiniMap");
		//miniMap.SetActive (false);
		Invoke ("HideLevelImage", levelStartingDelay);

        enemies.Clear();

		boardScript.SetUpScene (level, mapSize);
	}

	private void HideLevelImage(){
		levelImage.SetActive (false);
		doingSetup = false;
	}

	public void GameOver(){
        if( survivedTime >= bestRecord ){
            bestRecord = survivedTime;
        }

        doingSetup = true;
        levelText.text = "Better luck next time, astronaut!\n\nYou have survived: " + survivedTime +" seconds\n" +
            "Your best Record is : " + bestRecord + " seconds, keep it up!";
		levelImage.SetActive (true);
		

        StartCoroutine(TryAgain());

	}

    private IEnumerator TryAgain(){
        yield return new WaitForSeconds(1.5f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));

        level = 0;
        playerHealth = 100;
        survivedTime = 0;
        boardScript.mapGenerator.seed = "";
        SceneManager.LoadScene(0);
    }

	public void AddEnemyToList(Enemy enemy){
		enemies.Add (enemy);
	}

	void OnLevelWasLoaded(int index){
		level++;
		InitGame ();
	}

}
