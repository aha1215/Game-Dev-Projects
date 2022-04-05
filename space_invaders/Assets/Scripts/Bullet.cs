using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float speed = 15f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Fire();
    }

    private void Fire()
    {
        _rb.velocity = Vector2.up * speed;
    }
}