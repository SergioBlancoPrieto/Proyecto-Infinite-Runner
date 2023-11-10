using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelGenerator.sharedInstance.AddNewBlock();
            LevelGenerator.sharedInstance.RemoveOldBlock();
        }
    }
}
