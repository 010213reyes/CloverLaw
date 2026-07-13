using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GhostController;

public class HellGatoController : MonoBehaviour, ITargetCombat
{

    public enum HellGatoState
    {
        Inactive,
        ChasePalyer,
        WalkInTransformRight,
        Turn,
        Destruction
    }

    [SerializeField] int health = 1;

    [SerializeField] HellGatoState hellGatoState = HellGatoState.Inactive;

    [SerializeField] AnimatorController animatorController;

    [SerializeField] DamageFeedbackEffects damageFeedbackEffects;

    [SerializeField] float speed = 1f;

    [SerializeField] GameObject destructionEffect;

    [SerializeField] AudioClip destructionSfx;

    [SerializeField] LayerChecker groundChecker;

    [SerializeField] LayerChecker blockChecker;

    [SerializeField] LayerChecker visionRange;

    private Rigidbody2D rigidbody2D;

    private bool active;

    private bool isExecutingState = false;

    private void Awake()
    {
        hellGatoState = HellGatoState.Inactive;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (animatorController != null)
        {
            animatorController.Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        switch (hellGatoState)
        {
            case HellGatoState.WalkInTransformRight:
                WalkInTransformRight();
                break;

            case HellGatoState.ChasePalyer:
                ChasePlayer();
                break;

            case HellGatoState.Turn:
                Turn();
                break;
        }
    }

    void ChasePlayer()
    {
        var direction = (Vector2)HeroController.instance.transform.position - (Vector2)this.transform.position;

        rigidbody2D.velocity = new Vector2(direction.normalized.x * speed, rigidbody2D.velocity.y);

        if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (!visionRange.isTouching)
        {
            hellGatoState = HellGatoState.WalkInTransformRight;
        }
    }

    void WalkInTransformRight()
    {
        animatorController.Play(AnimationId.Walk);

        float direction = transform.eulerAngles.y == 0 ? 1f : -1f;
        rigidbody2D.velocity = new Vector2(direction * speed, rigidbody2D.velocity.y);

        if (!groundChecker.isTouching || blockChecker.isTouching)
        {
            hellGatoState = HellGatoState.Turn;
        }

        if (visionRange.isTouching)
        {
            hellGatoState = HellGatoState.ChasePalyer;
        }
    }

    void Turn()
    {
        rigidbody2D.velocity = Vector2.zero;

        if (transform.eulerAngles.y == 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        hellGatoState = HellGatoState.WalkInTransformRight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            active = true;

            if (animatorController != null)
            {
                animatorController.UnPause();
            }

            if (hellGatoState == HellGatoState.Inactive)
            {
                hellGatoState = HellGatoState.WalkInTransformRight;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            active = false;

            if (animatorController != null)
            {
                animatorController.Pause();
            }

            rigidbody2D.velocity = Vector2.zero;
        }
    }

    public void TakeDamage(int damagePoints)
    {
        health = Mathf.Clamp(health - damagePoints, 0, 100);
        damageFeedbackEffects.PlayDamageEffect();

        if (health <= 0)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        if (destructionEffect)
        {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }

        AudioManager.instance.PlaySfx(destructionSfx);

        rigidbody2D.velocity = Vector2.zero;
        hellGatoState = HellGatoState.Destruction;

        Object.Destroy(gameObject);
    }
}