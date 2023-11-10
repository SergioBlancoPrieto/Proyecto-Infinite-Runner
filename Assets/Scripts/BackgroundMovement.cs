using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private Vector2 _movementVelocity;
    private Vector2 _offset;
    private Material _material;
    [SerializeField] private Rigidbody2D _playerRB;

    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        _offset = (_playerRB.velocity.x * 0.1f) * _movementVelocity * Time.deltaTime;
        _material.mainTextureOffset += _offset;
    }
}
