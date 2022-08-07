using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyController : MonoBehaviour
{
    private SpriteRenderer mesh;
    private Collider2D col;
    private FixedShooterBehaviour fixedShooter;
    private GameManager gameManager;
    public GameObject reflectSound;
    public GameObject projectileVsProjectileSound;
    public GameObject hitSound;
    private Vector2 direction;
    public float projectileSpeed;
    private float damage;
    private bool reflected;
    public float lifeTime;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        reflected = false;
        fixedShooter = gameObject.transform.parent.gameObject.GetComponent<FixedShooterBehaviour>();
        mesh = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        damage = fixedShooter.damage;
        direction = Vector2.right;
    }

    void FixedUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;
        if (!reflected)
        {
            transform.Translate(direction * projectileSpeed * Time.fixedDeltaTime, Space.Self);
        }
        else
        {
            transform.Translate(direction * projectileSpeed * 3 * Time.fixedDeltaTime, Space.World);
        }

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
            gameManager.deathReason = "Застрелена!";
        }
        if (collision.gameObject.CompareTag("Sword"))
        {
            direction = (transform.position - collision.transform.position).normalized;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
            reflected = true;
            Instantiate(reflectSound, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
        }
        else if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(mesh);
            Destroy(col);
            Destroy(gameObject, 2);
            Instantiate(projectileVsProjectileSound, transform.position, transform.rotation);
        }
        else
        {
            Destroy(mesh);
            Destroy(col);
            Destroy(gameObject, 2);
            Instantiate(hitSound, transform.position, transform.rotation);
        }
        
    }
}