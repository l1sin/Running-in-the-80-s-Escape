using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ExpiredEpinephrineBehaviour : MonoBehaviour
{
    public GameObject PickUpEvent;
    private GameManager gameManager;
    private HealthComponent healthComponent;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2d;
    public float pointsOnCollect;
    public float cutHp;
    public float timeScale;
    public float duration;
    private float durationTime;
    private bool isCollected;
    private Image SlowEffect;
    public Color disabledColor;
    public Color enabledColor;
    public float activationSpeed;
    private float interpolator1;
    private float interpolator2;
    private bool levelComplete;

    private string pitch = "Pitch";
    public AudioMixer audioMixer;

    private void Start()
    {
        levelComplete = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
        durationTime = duration;
        isCollected = false;
        healthComponent = GameObject.Find("Player").GetComponent<HealthComponent>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SlowEffect = GameObject.Find("SlowEffect").GetComponent<Image>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCollected = true;
        if (playerController.epinephrine)
        {
            healthComponent.hP = 0;
            Destroy(gameObject);
            gameManager.deathReason = "Передозировка эпинефрином!";
        }
        gameManager.points += pointsOnCollect;
        playerController.epinephrine = true;

        if (healthComponent.hP > cutHp)
        {
            healthComponent.hP = cutHp;
        }

        if (PickUpEvent != null)
        {
            Instantiate(PickUpEvent, collision.transform.position, collision.transform.rotation);
        }
        Destroy(spriteRenderer);
        Destroy(collider2d);
    }

    private void Update()
    {
        if (isCollected)
        {
            levelComplete = gameManager.levelComplete;
            playerController.epinephrineDuration = duration;
            playerController.epinephrineDurationTime = durationTime;
            playerController.epinephrineActivationSpeed = activationSpeed;
            duration -= Time.deltaTime;
            if (duration > durationTime - activationSpeed)
            {
                interpolator1 += Time.deltaTime * (1 / activationSpeed);
                playerController.epinephrineInterpolator1 = interpolator1;
                audioMixer.SetFloat(pitch, Mathf.Lerp(1f, 0.5f, interpolator1));
                Time.timeScale = Mathf.Lerp(1, timeScale, interpolator1);
                SlowEffect.color = Color.Lerp(disabledColor, enabledColor, interpolator1);
            }
            if (duration <= activationSpeed)
            {
                interpolator2 += Time.deltaTime * (1 / activationSpeed);
                playerController.epinephrineInterpolator2 = interpolator2;
                audioMixer.SetFloat(pitch, Mathf.Lerp(0.5f, 1f, interpolator2));
                Time.timeScale = Mathf.Lerp(timeScale, 1, interpolator2);
                SlowEffect.color = Color.Lerp(enabledColor, disabledColor, interpolator2);
            }
            if (duration <= 0 || levelComplete)
            {
                playerController.epinephrine = false;
                playerController.epinephrineInterpolator1 = 0;
                playerController.epinephrineInterpolator2 = 0;

                Destroy(gameObject);
            }
        }
    }
}
