using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}
}
