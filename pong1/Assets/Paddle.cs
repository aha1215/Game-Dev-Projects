using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 movement;
    private Rigidbody _rb;
    public bool leftPaddle;

    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (leftPaddle) 
        {
            movement = new Vector3(0f, 0f, Input.GetAxis("LeftPaddle"));
        }
        else
        {
            movement = new Vector3(0f, 0f, Input.GetAxis("RightPaddle"));
        }
    }
    
    void FixedUpdate()
    {
        //dont use translate. It ignores collisions
        _rb.MovePosition(transform.position + (movement * (speed * Time.deltaTime)));
    }
}