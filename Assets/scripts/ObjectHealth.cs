using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
   public int health = 100;//vida
   public ParticleSystem destroyEffect;

    public void applyDamage(int damage) //perde vida
    {
        health -= damage;
        if(health <= 0) 
        {
            destroyEffect.Play();
            Destroy(gameObject, 0.2f);
        }
    }
}
