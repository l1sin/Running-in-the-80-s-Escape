using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidKitBehavior : MonoBehaviour
{
    public GameObject PickUpEvent;
    private GameManager gameManager;
    private HealthComponent healthComponent;
    public float pointsOnCollect;
    public float restoreHp;

    private void Start()
    {
        healthComponent = GameObject.Find("Player").GetComponent<HealthComponent>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        healthComponent.hP += restoreHp;
        gameManager.points += pointsOnCollect;

        if (PickUpEvent != null)
        {
            Instantiate(PickUpEvent.transform, collision.transform.position, collision.transform.rotation);
        }

        Destroy(gameObject);
    }
}
