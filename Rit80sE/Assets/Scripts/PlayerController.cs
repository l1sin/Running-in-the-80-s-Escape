using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Color defaultColor;
    public Color freezedColor;
    public Color damagedColor;
    private Color currentColor;
    private SpriteRenderer rightHandSwordSpriteRenderer;
    private SpriteRenderer leftHandGunSpriteRenderer;
    private SpriteRenderer headSpriteRenderer;
    private SpriteRenderer bodySpriteRenderer;
    private Transform leftHandGunTransform;
    private Transform headTransform;
    private Vector3 difference;
    private Animator animator;
    private Animator swordAnimator;
    private Rigidbody2D rb2d;
    public GameObject projectile;
    public GameObject swingSound;
    public Transform projectileSpawn;
    public Transform groundCheckTransform;
    public LayerMask whatIsGround;
    private HealthComponent health;
    public float groundCheckRadius;

    public bool isDamaged;
    private bool setDamageTime;

    public float damagedColorTime;
    private float damagedColorTimer;


    public float movingFrontSpeed;
    public float movingBackSpeed;
    public float currentMovingFrontSpeed;
    public float currentMovingBackSpeed;

    public bool walksForward;
    public bool stay;
    public float jumpForce;
    public bool allowDoubleJump;
    public bool isGrounded;
    public bool isStunned;
    public float resetStun;
    public bool isFreezed;
    public float resetFreeze;

    public bool epinephrine;

    private float myTime = 0;
    public float shootCooldown;
    public float swingCooldown;
    private float currentSwingColldown;
    private float shootTime;
    private float swingTime;

    public float ammo;
    public float maxAmmo;
    public float ammoConsumePerShot;
    public float ammoRestoreSpeed;
    private Image hPLine;
    private Image ammoLine;

    public float pistolDamage;
    public float swordDamage;

    public float epinephrineInterpolator1;
    public float epinephrineInterpolator2;
    public float epinephrineDuration;
    public float epinephrineDurationTime;
    public float epinephrineActivationSpeed;

    public float rotateThreshold;

    public float maxVelocityY;

    public bool levelComplete;
    public bool paused;

    private AudioSource audioSource;
    public GameObject hitSound;

    private void Start()
    {
        setDamageTime = false;
        currentColor = defaultColor;
        rightHandSwordSpriteRenderer = GameObject.Find("RightHand(Sword)").GetComponent<SpriteRenderer>();
        swordAnimator = GameObject.Find("RightHand(Sword)").GetComponent<Animator>();
        leftHandGunSpriteRenderer = GameObject.Find("LeftHand(Gun)").GetComponent<SpriteRenderer>();
        leftHandGunTransform = GameObject.Find("LeftHandPivot").transform;
        headTransform = GameObject.Find("Head").transform;
        headSpriteRenderer = GameObject.Find("Head").GetComponent<SpriteRenderer>();
        bodySpriteRenderer = GameObject.Find("Body").GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthComponent>();
        hPLine = GameObject.Find("HPLine").GetComponent<Image>();
        ammoLine = GameObject.Find("AmmoLine").GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        isStunned = false;
        isFreezed = false;
    }
    private void FixedUpdate()
    {
        Move();
        RotateHandWithGun();
        RotateHead();
    }
    private void Update()
    {
        myTime += Time.deltaTime;
        animator.SetFloat("Velocity", rb2d.velocity.y);
        Shoot();
        DoubleJump();
        Jump();
        Swing();
        hPController();
        ColorController();
    }
    private void RotateHandWithGun()
    {
        difference = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z))) -transform.position;
        float rotationZ = Mathf.Atan2(difference.normalized.y, difference.normalized.x) * Mathf.Rad2Deg;
        leftHandGunTransform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        if ((rotationZ > 90 - rotateThreshold && rotationZ < 180) || (rotationZ < -90 + rotateThreshold && rotationZ > -180))
        {
            if ((rotationZ > 90 + rotateThreshold && rotationZ < 180) || (rotationZ < -90 - rotateThreshold && rotationZ > -180))
            {
                RotateBodyLeft();
            }
            if (transform.localEulerAngles.y == 180)
            {
                leftHandGunTransform.rotation = Quaternion.Euler(180f, 0f, -rotationZ);
            }
        }
        if ((rotationZ < 90 - rotateThreshold && rotationZ > 0) || (rotationZ > -90 + rotateThreshold && rotationZ < 0))
        {
            RotateBodyRight();
        }
    }

    private void RotateHead()
    {
        float rotationZ = Mathf.Atan2(difference.normalized.y, difference.normalized.x) * Mathf.Rad2Deg;
        if (rotationZ > -30 && rotationZ < 30)
        {
            headTransform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        }
        else if (rotationZ > 150 || rotationZ < -150)
        {
            headTransform.rotation = Quaternion.Euler(180f, 0f, -rotationZ);
        }
    }

    private void RotateBodyRight()
    {
        transform.rotation = Quaternion.Euler(new Vector2(0, 0));
        rightHandSwordSpriteRenderer.sortingOrder = 4;
        leftHandGunSpriteRenderer.sortingOrder = 1;
    }
    private void RotateBodyLeft()
    {
        transform.rotation = Quaternion.Euler(new Vector2(0, 180));
        rightHandSwordSpriteRenderer.sortingOrder = 1;
        leftHandGunSpriteRenderer.sortingOrder = 4;
    }

    private void Move()
    {
        if (!isStunned)
        {
            if ((Input.GetAxis("Horizontal") > 0 && transform.eulerAngles.y == 0) || (Input.GetAxis("Horizontal") < 0 && transform.eulerAngles.y == 180))
            {
                rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * currentMovingFrontSpeed, rb2d.velocity.y);
                walksForward = true;
                stay = false;
                animator.SetBool("WalksForward", walksForward);
                animator.SetBool("Stay", stay);
            }
            else if ((Input.GetAxis("Horizontal") > 0 && transform.eulerAngles.y == 180) || (Input.GetAxis("Horizontal") < 0 && transform.eulerAngles.y == 0))
            {
                rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * currentMovingBackSpeed, rb2d.velocity.y);
                walksForward = false;
                stay = false;
                animator.SetBool("WalksForward", walksForward);
                animator.SetBool("Stay", stay);
            }
            if (Input.GetAxis("Horizontal") == 0 && isGrounded)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                stay = true;
                animator.SetBool("Stay", stay);
            }
        }
        else
        {
            resetStun -= Time.deltaTime;
            if (resetStun <= 0)
            {
                isStunned = false;
            }
        }
        if (!isFreezed && !isDamaged && !epinephrine)
        {
            currentMovingFrontSpeed = movingFrontSpeed;
            currentMovingBackSpeed = movingBackSpeed;
            currentColor = defaultColor;
        }
        else if (isFreezed && !isDamaged && !epinephrine)
        {
            currentMovingBackSpeed = movingBackSpeed / 2;
            currentMovingFrontSpeed = movingFrontSpeed / 2;
            currentColor = freezedColor;

            resetFreeze -= Time.deltaTime;
            if (resetFreeze <= 0)
            {
                isFreezed = false;
            }
        }
        if (epinephrine)
        {
            if (epinephrineDuration > epinephrineDurationTime - epinephrineActivationSpeed)
            {
                currentMovingFrontSpeed = Mathf.Lerp(movingFrontSpeed, movingFrontSpeed * 2, epinephrineInterpolator1);
                currentMovingBackSpeed = Mathf.Lerp(movingBackSpeed, movingBackSpeed * 2, epinephrineInterpolator1);
            }
            if (epinephrineDuration <= epinephrineActivationSpeed)
            {
                currentMovingFrontSpeed = Mathf.Lerp(movingFrontSpeed * 2, movingFrontSpeed, epinephrineInterpolator2);
                currentMovingBackSpeed = Mathf.Lerp(movingBackSpeed * 2, movingBackSpeed, epinephrineInterpolator2);
            }
        }
        if (rb2d.velocity.y > maxVelocityY)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, maxVelocityY);
        }
        else if (rb2d.velocity.y < -maxVelocityY)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -maxVelocityY);
        }
    }

    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, whatIsGround);
        if (Input.GetButtonDown("Jump") && isGrounded && !levelComplete && !paused)
        {
            rb2d.velocity = new Vector2(0, jumpForce);
        }
    }
    private void DoubleJump()
    {
        if (isGrounded)
        {
            allowDoubleJump = true;
        }
        if (Input.GetButtonDown("Jump") && allowDoubleJump && !isGrounded && !isStunned && !levelComplete && !paused)
        {
            rb2d.velocity = new Vector2(0, jumpForce);
            allowDoubleJump = false;
            animator.SetTrigger("DoubleJump");
            audioSource.Play();
        }
    }

    private void Shoot()
    {
        ammoLine.fillAmount = ammo / maxAmmo;
        if (ammo >= maxAmmo)
        {
            ammo = maxAmmo;
        }
        else
        {
            ammo += Time.deltaTime * ammoRestoreSpeed;
        }

        if (Input.GetButton("Fire1") && myTime > shootTime && ammo >= ammoConsumePerShot && !levelComplete && !paused)
        {
            shootTime = myTime + shootCooldown;
            ammo -= ammoConsumePerShot;
            Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }

    private void Swing()
    {
        if (!epinephrine)
        {
            currentSwingColldown = swingCooldown;
        }
        else
        {
            currentSwingColldown = swingCooldown / 2;
        }
        if (Input.GetKeyDown(KeyCode.E) && myTime > swingTime && !levelComplete && !paused)
        {
            swingTime = myTime + currentSwingColldown;
            swordAnimator.SetTrigger("Swing");
            Instantiate(swingSound);
        }
    }

    private void hPController()
    {
        hPLine.fillAmount = health.hP / health.maxHP;
        isDamaged = health.isDamaged;
    }

    private void ColorController()
    {
        leftHandGunSpriteRenderer.color = currentColor;
        rightHandSwordSpriteRenderer.color = currentColor;
        headSpriteRenderer.color = currentColor;
        bodySpriteRenderer.color = currentColor;
        if (isDamaged)
        {
            if (!setDamageTime)
            {
                damagedColorTimer = damagedColorTime;
                setDamageTime = true;
                Instantiate(hitSound, transform.position, transform.rotation);
            }

            if (isFreezed)
            {
                currentColor = damagedColor;
                damagedColorTimer -= Time.deltaTime;
                if (damagedColorTimer <= 0)
                {
                    currentColor = freezedColor;
                    isDamaged = false;
                    health.isDamaged = false;
                    setDamageTime = false;
                }
            }
            else
            {
                currentColor = damagedColor;
                damagedColorTimer -= Time.deltaTime;
                if (damagedColorTimer <= 0)
                {
                    currentColor = defaultColor;
                    isDamaged = false;
                    health.isDamaged = false;
                    setDamageTime = false;
                }
            }
        }
    }
}
