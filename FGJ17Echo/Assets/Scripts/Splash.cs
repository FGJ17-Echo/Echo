using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
	void Start ()
    {
        Invoke("StartGame", 1);
	}

	void StartGame()
    {
        SceneManager.LoadScene(1);
	}
}
