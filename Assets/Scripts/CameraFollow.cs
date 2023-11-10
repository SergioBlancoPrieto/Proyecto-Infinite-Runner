using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float separation = 6.0f;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    void Update()
    {
        Vector3 targetPosition = player.TransformPoint(new Vector3(separation+1, -player.transform.position.y, -10));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
