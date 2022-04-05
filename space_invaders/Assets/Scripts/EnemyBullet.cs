using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float speed = 3f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Fire();
    }

    private void Fire()
    {
        _rb.velocity = Vector2.down * speed;
    }
}
