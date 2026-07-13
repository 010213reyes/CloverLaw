using System.Collections.Generic;
using UnityEngine;

public class DamageDemo : MonoBehaviour, ITargetCombat
{
    [SerializeField] int health;
    [SerializeField] DamageFeedbackEffects damageFeedbackEffects;
    
    public void TakeDamage(int damagePoints)
    {
        health -= damagePoints;
        damageFeedbackEffects.PlayDamageEffect();

    }
}
