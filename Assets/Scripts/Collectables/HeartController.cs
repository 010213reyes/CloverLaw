using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herat : MonoBehaviour
{
    
    [SerializeField] TagId playerTag;
    [SerializeField] TagId groundTag;

    [SerializeField] AudioClip pickUpSfx;
   


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals(playerTag.ToString()))
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        if (collision.gameObject.tag.Equals(playerTag.ToString()))
        {

            HeroController.instance.GiveHealthPoint();
            AudioManager.instance.PlaySfx(pickUpSfx);
            Destroy(this.gameObject);
        }
    }


}
