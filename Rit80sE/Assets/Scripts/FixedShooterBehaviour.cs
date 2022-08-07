using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedShooterBehaviour : MonoBehaviour
{
    public float shootTime;
    public float myTime;
    public float shootCooldown;
    public GameObject projectile;
    public Transform projectileSpawn;
    public float damage;

    void Update()
    {
        myTime += Time.deltaTime;
        Shoot();
    }

    private void Shoot()
    {
        if (myTime > shootTime)
        {
            shootTime = myTime + shootCooldown;
            GameObject spawnedProjectile = Instantiate (projectile, projectileSpawn.position, projectileSpawn.rotation);
            spawnedProjectile.transform.parent = gameObject.transform;
        }
    }
}
