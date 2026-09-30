using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Flag : MonoBehaviour
{
    public SpriteRenderer sr;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            sr.enabled = false; //Make flag invisible
            GameController.Instance.gameState = "flagTaken"; 
        }
    }

    
}
