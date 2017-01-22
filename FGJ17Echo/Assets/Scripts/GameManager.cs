using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public enum gameState
	{
		DEFAULT,
		GAMESTART,
		PLAYING,
		GAMEOVER,
		MENU
	};
	public gameState gs = gameState.DEFAULT;

	void Update () {
		switch (gs)
		{
		case gameState.GAMESTART:
			Debug.Log("Load all the assets");
			break;
		case gameState.PLAYING:
			Debug.Log("We are playing now");
			break;
		case gameState.GAMEOVER:
			Debug.Log("Game over");
			break;
		case gameState.MENU:
			Debug.Log("Menu");
			break;
		default:
			Debug.Log("ERROR: Unknown game state: " + gs);
			break;
		}

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(1);
        }
    }
}
