using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubleBehaviour : MonoBehaviour
{
    public GameObject PickUpEvent;
    private GameManager gameManager;
    public float pointsOnCollect;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.points += pointsOnCollect;
        gameManager.rublesCollected++;

        if (PickUpEvent != null)
        {
            Instantiate(PickUpEvent, collision.transform.position, collision.transform.rotation);
        }

        Destroy(gameObject);
    }
}
