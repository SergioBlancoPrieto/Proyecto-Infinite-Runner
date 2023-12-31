using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    Animator _animator;
    
    void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CombatPlayer>().DoBounce(transform.position);
            if (!other.gameObject.GetComponent<PlayerController>().IsInvulnerable())
            {
                other.gameObject.GetComponent<PlayerController>().DamagePlayer();
                this.GetComponent<AudioSource>().Play();
                TriggerAnimator();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CombatPlayer>().DoBounce(transform.position);
            if (!other.gameObject.GetComponent<PlayerController>().IsInvulnerable())
            {
                other.gameObject.GetComponent<PlayerController>().DamagePlayer();
                this.GetComponent<AudioSource>().Play();
                TriggerAnimator();
            }
        }
    }

    private void TriggerAnimator() {
        _animator?.SetTrigger("AtackTrigger");
    }
}
