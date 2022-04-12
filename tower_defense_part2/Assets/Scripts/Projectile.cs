using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityStandardAssets.Effects;
using Vector3 = UnityEngine.Vector3;

public class Projectile : MonoBehaviour
{
    private Rigidbody _rb;

    public float speed = 15f;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Fire();
    }

    private void Fire()
    {
        _rb.velocity = transform.forward * speed;
    }
}
