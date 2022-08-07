using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2d;
    private PlayerController playerController;
    public float projectileSpeed;
    private float damage;
    public float lifeTime;
    public GameObject projectileVsProjectileSound;
    public GameObject hitSound;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
        damage = playerController.pistolDamage;
    }

    void FixedUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;
        transform.Translate(Vector2.right * projectileSpeed * Time.fixedDeltaTime, Space.Self);
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthComponent healthComponent))
        {
            healthComponent.hP -= damage;
            healthComponent.isDamaged = true;
        }

        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            Instantiate(projectileVsProjectileSound, transform.position, transform.rotation);
            Destroy(spriteRenderer);
            Destroy(collider2d);
            Destroy(gameObject, 2);
        }
        else
        {
            Instantiate(hitSound, transform.position, transform.rotation);
            Destroy(spriteRenderer);
            Destroy(collider2d);
            Destroy(gameObject, 2);
        }     
    }
}
