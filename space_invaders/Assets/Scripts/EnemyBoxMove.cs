using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBoxMove : MonoBehaviour
{
    public float speed = 200f;
    public GameObject leftWall;
    public GameObject rightWall;
    public bool moveLeft = false;
    public bool moveRight = false;

    public float moveSpeedTimeDelay = 3f;

    private Vector2 _originalPos;

    void Start()
    {
        moveRight = true;
        InvokeRepeating(nameof(MoveEnemies), moveSpeedTimeDelay, moveSpeedTimeDelay);
        _originalPos = transform.position;
    }

    void MoveEnemies()
    {
        var pos = transform.position;

        if (moveRight)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);
        }

        if (moveLeft)
        {
            transform.Translate(-transform.right * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("leftwall"))
        {
            moveRight = true;
            moveLeft = false;
            transform.Translate(-transform.up);
        }
        else if (other.CompareTag("rightwall"))
        {
            moveLeft = true;
            moveRight = false;
            transform.Translate(-transform.up);
        }
    }

    public Vector3 GetOriginalPosition()
    {
        return _originalPos;
    }

    public void Reset()
    {
        transform.position = _originalPos;
        moveSpeedTimeDelay = 3f;
        speed = 200f;
        moveRight = true;
        InvokeRepeating(nameof(MoveEnemies), moveSpeedTimeDelay, moveSpeedTimeDelay);
    }
}
