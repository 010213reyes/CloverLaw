using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroController : MonoBehaviour, ITargetCombat
{
    [Header("Power Up")]
    [SerializeField] private PowerUpId currentPowerUp;
    [SerializeField] private int powerUpAmount;
    [SerializeField]
    SpellLauncherController bluePotionLauncher;
    [SerializeField] 
    SpellLauncherController redPotionLauncher;

    [Header("Health Variables")]    
    [SerializeField]  int health = 10;
    [SerializeField] DamageFeedbackEffects damageFeedbackEffects;

    [Header("Attack Variables")]
    [SerializeField] SwordController swordController;

    [Header("Animation Variables")]
    [SerializeField] AnimatorController animatorController;

    [Header("Checker Variables")]
    [SerializeField] LayerChecker footA;
    [SerializeField] LayerChecker footB;


    [Header("Boolean Variables")]
    public bool playerIsAttacking;
    public bool playerIsUsingPowerUp;
    public bool playerIsRecovering;
    public bool canDoubleJump;
    public bool isLookingRight;


    [Header("Interruption Variables")]
    public bool canCheckGround;
    public bool canMove;
    public bool canFlip;

    [Header("Rigid Variables")]

    [SerializeField] private float damageForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private float damageForceUp;

    [SerializeField] private float speed;


    [Header("Audio")]
    [SerializeField] AudioClip attackSfx;

    // Control Variables
    [SerializeField] private Vector2 movementDirection;
    private bool jumpPressed = false;
    private bool attackPressed = false;
    private bool usePowerUpPressed = false;
    private int coins;


    private bool playerIsOnGround;


    private Rigidbody2D rigidbody2D;
    


 
    public static HeroController instance;


    private void Awake()
    {
        if (instance == null) { 
         instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        canCheckGround = true;
        canMove = true;
        canFlip = true;
        isLookingRight = true;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animatorController.Play(AnimationId.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        HandleIsGrounding();
        HandleControls();
        HandleMovement();
        HandleFlip();
        HandleJump();
        HandleAttack();
        HandleUsePowerUp();
    }


    public void GiveCoin()
    {
        coins = Mathf.Clamp(coins + 1, 0, 9999999);
    }

    public void GiveHealthPoint()
    {
                health = Mathf.Clamp(health + 1, 0, 10);
    }


    public void ChangePowerUp(PowerUpId powerUpId, int amount)
    {
        currentPowerUp= powerUpId;
        powerUpAmount= amount;
        Debug.Log(currentPowerUp);
    }

    void HandleIsGrounding()
    {
        if (!canCheckGround) return;
        playerIsOnGround = footA.isTouching || footB.isTouching;
    }

    void HandleControls() 
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        jumpPressed = Input.GetButtonDown("Jump");
        attackPressed = Input.GetButtonDown("Attack");
        usePowerUpPressed = Input.GetButtonDown("UsePowerUp");
    }   

    void HandleMovement()
    {
        if (!canMove) return;
        rigidbody2D.velocity = new Vector2(movementDirection.x * speed, rigidbody2D.velocity.y);


        if (playerIsOnGround)
        {
            if (Mathf.Abs(rigidbody2D.velocity.x) > 0.01f)
            {
                animatorController.Play(AnimationId.Run);
            }
            else
            {
                animatorController.Play(AnimationId.Idle);
            }
        }
    }

    void HandleFlip()
    {
        if (!canFlip) return;
        
       
        if (movementDirection.x > 0.01f)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            isLookingRight = true;
        }
       
        else if (movementDirection.x < -0.01f)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
            isLookingRight = false;
        }
    }

    void HandleJump()
    {

        if(canDoubleJump && jumpPressed && !playerIsOnGround)
        {
            this.rigidbody2D.velocity = Vector2.zero;
            this.rigidbody2D.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
            StartCoroutine(HandleJumpAnimation());
            canDoubleJump = false;
        }

        if (jumpPressed && playerIsOnGround)
        {
            this.rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(HandleJumpAnimation());
            canDoubleJump = true;
        }

    }

    void HandleAttack()
    {
        if(attackPressed && !playerIsAttacking)
        {
            if (playerIsOnGround)
            {

                rigidbody2D.velocity = Vector2.zero;
            }
            AudioManager.instance.PlaySfx(attackSfx);
            animatorController.Play(AnimationId.Attack);
            playerIsAttacking = true;
            swordController.Attack(0.1f, 0.3f);
            StartCoroutine(RestoreAttack());
        }
    }


    IEnumerator RestoreAttack()
    {
        if(playerIsOnGround)
            canMove = false;
        yield return new WaitForSeconds(0.3f);
        playerIsAttacking = false;
        if (!playerIsAttacking)
            animatorController.Play(AnimationId.Jump);
        canMove = true; 
    }

    void HandleUsePowerUp()
    {
        if (usePowerUpPressed && !playerIsUsingPowerUp && currentPowerUp != PowerUpId.Nothing)
        {
            if (playerIsOnGround)
            {
                rigidbody2D.velocity = Vector2.zero;
            }
            AudioManager.instance.PlaySfx(attackSfx);
            animatorController.Play(AnimationId.UsePowerUp);
            playerIsUsingPowerUp = true;

            // swordController.Attack(0.1f, 0.3f);


            if (currentPowerUp == PowerUpId.BluePotion)
            {
                // Removido " + Vector2.up * 0.3f"
                bluePotionLauncher.Launch((Vector2)transform.right);
            }
            else if (currentPowerUp == PowerUpId.RedPotion)
            {
                redPotionLauncher.Launch(transform.right);
            }
            StartCoroutine(RestoreUsePowerUp());


            powerUpAmount--;

            if(powerUpAmount <= 0)
            {
                currentPowerUp = PowerUpId.Nothing;
            }
        }
    }


    IEnumerator RestoreUsePowerUp()
    {
        if (playerIsOnGround)
            canMove = false;
        yield return new WaitForSeconds(0.3f);
        playerIsUsingPowerUp = false;
        if (!playerIsOnGround)
            animatorController.Play(AnimationId.Jump);
        canMove = true;
    }

    
    IEnumerator HandleJumpAnimation()
    {
        canCheckGround = false;
        playerIsOnGround = false;
        if(!playerIsAttacking)
                animatorController.Play(AnimationId.PrepareJump);
        yield return new WaitForSeconds(0.3f);
        if(!playerIsAttacking)
            animatorController.Play(AnimationId.Jump);
        canCheckGround= true;
    }

    public void TakeDamage(int damagePoints)
    {
        if (!playerIsRecovering)
        {
            health = Mathf.Clamp(health - damagePoints, 0, 10);
            // Agregamos esta línea para iniciar la recuperación y el parpadeo
            StartCoroutine(StartPlayerRecover());
            if (isLookingRight)
            {
                rigidbody2D.AddForce(Vector2.left * damageForce + Vector2.up * damageForceUp, ForceMode2D.Impulse);

            }
            else
            {
                rigidbody2D.AddForce(Vector2.right * damageForce + Vector2.up * damageForceUp, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator StartPlayerRecover() { 
        canMove = false;
        canFlip = false;
        animatorController.Play(AnimationId.Hurt);
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        canFlip = true;
        rigidbody2D.velocity = Vector2.zero;
        playerIsRecovering = true;
        damageFeedbackEffects.PlayBlinkDamageEffect();
        yield return new WaitForSeconds(2f);
        damageFeedbackEffects.StopBlinkDamageEffect();
        playerIsRecovering = false;
    }
}
