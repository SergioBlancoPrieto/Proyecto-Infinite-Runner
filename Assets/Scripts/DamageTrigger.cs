using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().DamagePlayer();
            other.gameObject.GetComponent<CombatPlayer>().DoBounce(other.GetContact(0).normal);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().DamagePlayer();
            other.gameObject.GetComponent<CombatPlayer>().DoBounce(transform.position);
        }
    }
}
