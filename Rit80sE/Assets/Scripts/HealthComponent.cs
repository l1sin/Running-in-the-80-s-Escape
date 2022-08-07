using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private GameManager gameManager;
    public float pointsOnDeath;
    public float hP;
    public float maxHP;
    public bool isDamaged;
    public GameObject deathEvent;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hP = maxHP;
    }
    private void Update()
    {
        if (hP <= 0)
        {
            if (deathEvent != null)
            {
                Instantiate(deathEvent, gameObject.transform.position, gameObject.transform.rotation);
            }
            gameManager.points += pointsOnDeath;
            Destroy(gameObject);
        }
        if (hP > maxHP)
        {
            hP = maxHP;
        }
    }
}
