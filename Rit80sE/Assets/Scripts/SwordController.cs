using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public float damage;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        damage = playerController.swordDamage;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthComponent healthComponent))
        {
            healthComponent.hP -= damage;
            healthComponent.isDamaged = true;
        }
    }
}
