using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMineBehaviour : MonoBehaviour
{
    public float damage;
    public float knockback;
    public float stunTime;

    private PlayerController player;
    private Rigidbody2D rb2d;
    private GameManager gameManager;
    public GameObject explosiveMineExplosionSound;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            rb2d = collision.gameObject.GetComponent<Rigidbody2D>();

            if (collision.gameObject.TryGetComponent(out HealthComponent healthComponent))
            {
                healthComponent.hP -= damage;
                healthComponent.isDamaged = true;
                gameManager.deathReason = "Подорвалась на мине!";
            }

            player.isStunned = true;
            player.resetStun = stunTime;

            if (collision.gameObject.transform.position.x > transform.localPosition.x)
            {
                rb2d.AddForce(new Vector2(1, 1) * knockback, ForceMode2D.Impulse);
            }
            else
            {
                rb2d.AddForce(new Vector2(-1, 1) * knockback, ForceMode2D.Impulse);
            }

            Destroy(gameObject);
            Instantiate(explosiveMineExplosionSound, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
        }
    }
}
