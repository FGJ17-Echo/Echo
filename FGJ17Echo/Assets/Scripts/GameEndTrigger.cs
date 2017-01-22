using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var go = collider.attachedRigidbody ? collider.attachedRigidbody.gameObject : collider.gameObject;

        var bat = go.GetComponent<BatController>();

        if (bat != null)
        {
            SceneManager.LoadScene(4);
        }
    }
}
