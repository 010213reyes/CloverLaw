using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] TagId playerTag;
    [SerializeField] TagId groundTag;

    [SerializeField] AudioClip pickUpSfx;

    // Cambiamos OnTriggerEnter2D por OnCollisionEnter2D
    // Y cambiamos Collider2D por Collision2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals(playerTag.ToString()))
        {
            HeroController.instance.GiveCoin();
            AudioManager.instance.PlaySfx(pickUpSfx);
            Destroy(this.gameObject);
        }
    }
}
