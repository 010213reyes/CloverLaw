using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] PowerUpId powerUpId;
    [SerializeField] TagId playerTag;
    [SerializeField] TagId groundTag;

    [SerializeField] AudioClip pickUpSfx;
    [SerializeField] int maxAmount = 10;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals(playerTag.ToString()))
        {
            HeroController.instance.ChangePowerUp(powerUpId, Random.Range(5, maxAmount));
            AudioManager.instance.PlaySfx(pickUpSfx);
            Destroy(this.gameObject);
        }
    }
}
