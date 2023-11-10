using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMovement : MonoBehaviour
{
    public bool turnAround;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (turnAround)
        {
            turnAround = false;
        }
        else
        {
            turnAround = true;
        }
    }
}
