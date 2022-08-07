using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathEvent : MonoBehaviour
{
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.dead = true;
    }
}
