using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorComponent : MonoBehaviour
{
    public Color defaultColor;
    public Color damagedColor;
    private Color currentColor;
    private bool setDamageTime;
    private float damagedColorTimer;
    public float damagedColorTime;
    private bool isDamaged;
    private SpriteRenderer spriteRenderer;
    private HealthComponent health;
    public GameObject hitSound;

    private void Start()
    {
        currentColor = defaultColor;
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<HealthComponent>();
    }

    private void Update()
    {
        ColorController();
    }
    private void ColorController()
    {
        isDamaged = health.isDamaged;
        spriteRenderer.color = currentColor;

        if (isDamaged)
        {
            if (!setDamageTime)
            {
                damagedColorTimer = damagedColorTime;
                setDamageTime = true;
                Instantiate(hitSound, transform.position, transform.rotation);
            }

            currentColor = damagedColor;
            damagedColorTimer -= Time.deltaTime;

            if (damagedColorTimer <= 0)
            {
                currentColor = defaultColor;
                isDamaged = false;
                setDamageTime = false;
                health.isDamaged = false;
            }
        }
        else
        {
            currentColor = defaultColor;
        }
    }
}
