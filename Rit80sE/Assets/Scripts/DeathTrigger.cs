using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private GameManager gameManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.deathReason = "Вы разбились!";
        collision.gameObject.GetComponent<HealthComponent>().hP = 0;
    }
}
