using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    [SerializeField]private AudioClip hit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
			AudioSource.PlayClipAtPoint(hit, transform.position);
            PlayerController.sharedInstance.KillPlayer();
        }
    }
    
}
