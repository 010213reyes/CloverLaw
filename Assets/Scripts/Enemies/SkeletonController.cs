using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour, ITargetCombat
{

    public enum SkeletonState
    {
        Inactive,
        Rise,
        WalkInTransformRight,
        Turn,
        Destruction
    }

    [SerializeField] int health = 1;

    [SerializeField] SkeletonState skeletonState = SkeletonState.Inactive;

    [SerializeField] AnimatorController animatorController;

    [SerializeField] DamageFeedbackEffects damageFeedbackEffects;

    [SerializeField] float speed = 1f;

    [SerializeField] GameObject destructionEffect;

    [SerializeField] AudioClip destructionSfx;

    [SerializeField] LayerChecker groundChecker;

    [SerializeField] LayerChecker blockChecker;

    private Rigidbody2D rigidbody2D;

    private bool active;

    private bool isExecutingState = false;

    private void Awake()
    {
        skeletonState = SkeletonState.Inactive;
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
        if (active)
        {
            if (skeletonState == SkeletonState.Rise && !isExecutingState)
            {
                Rise();
            }
            else if (skeletonState == SkeletonState.WalkInTransformRight)
            {
                WalkInTransformRight();
            }
        }
    }

    void WalkInTransformRight()
    {
        animatorController.Play(AnimationId.Walk);
        rigidbody2D.velocity = transform.right * speed;

        if (!groundChecker.isTouching || blockChecker.isTouching)
        {
            Turn();
        }
    }

    void Turn()
    {
        if(this.transform.right == Vector3.right)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        skeletonState = SkeletonState.WalkInTransformRight;
    }



    void Rise()
    {
        StartCoroutine(_Rise());
    }

    IEnumerator _Rise() 
    {
        isExecutingState = true;
        animatorController.Play(AnimationId.Rise);
        yield return new WaitForSeconds(1f);

        skeletonState = SkeletonState.WalkInTransformRight;
        isExecutingState = false;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = true;
            
            if (animatorController != null)
            {
                animatorController.UnPause();
            }

            if (skeletonState == SkeletonState.Inactive)
            {
                skeletonState = SkeletonState.Rise;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = false;
            
            if (animatorController != null)
            {
                animatorController.Pause();
            }
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
            Instantiate(destructionEffect, this.transform.position, Quaternion.identity);
        }

        AudioManager.instance.PlaySfx(destructionSfx);

        rigidbody2D.velocity = Vector2.zero;
        skeletonState = SkeletonState.Destruction;
        Object.Destroy(this.gameObject);
    }
}
