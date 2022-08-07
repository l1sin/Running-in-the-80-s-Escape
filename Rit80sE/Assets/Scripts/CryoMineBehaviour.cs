using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryoMineBehaviour : MonoBehaviour
{
    public float damage;
    public float freezeTime;
    private PlayerController player;
    private GameManager gameManager;
    public GameObject FreezingSound;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            if (collision.gameObject.TryGetComponent(out HealthComponent healthComponent))
            {
                healthComponent.hP -= damage;
                healthComponent.isDamaged = true;
                gameManager.deathReason = "Заморожена насмерть!";
            }
            player.isFreezed = true;
            player.resetFreeze = freezeTime;

            Destroy(gameObject);
            Instantiate(FreezingSound, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
        }
    }
}
