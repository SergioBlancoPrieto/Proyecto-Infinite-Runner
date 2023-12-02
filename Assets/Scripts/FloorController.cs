using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (this.gameObject.CompareTag("FloorSlime"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController.sharedInstance.RalentizarSpeed();
            }
        }
        else
        {
            PlayerController.sharedInstance.RestaurarSpeed();
        }
    }
}
