using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runForce = 50f;
    public float turboForce = 12f;
    public float jumpForce = 10f;
    public float maxHorizontalSpeed = 6f;
    public float defaultHorizontalSpeed = 6f;
    
    private Rigidbody _rb;
    private float _distanceFromGround;
    private float _distanceFromBlock;

    private GameObject _spawner;
    private Spawner _objectSpawner;

    private Game _gameManager;
    private GameObject _gm;

    private bool _passedFlag;
    private Animator _animComp;
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private bool _facingRight = true;

    private Vector3 _originalPos;
    // Start is called before the first frame update
    void Start()
    {
        _animComp = GetComponent<Animator>();
        _rb = this.GetComponent<Rigidbody>();
        _distanceFromGround = GetComponent<Collider>().bounds.extents.y + 0.1f;
        _distanceFromBlock = GetComponent<Collider>().bounds.extents.y + 0.9f;
        
        _spawner = GameObject.Find("ObjectSpawner");
        _objectSpawner = _spawner.GetComponent<Spawner>();

        _gm = GameObject.Find("GameManager");
        _gameManager = _gm.GetComponent<Game>();
        _originalPos = _rb.transform.position;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            runForce = turboForce;
            maxHorizontalSpeed = turboForce;
        }
        else
        {
            maxHorizontalSpeed = defaultHorizontalSpeed;
        }
        
        var axis = Input.GetAxis("Horizontal");
        _rb.AddForce(Vector3.right * (axis * runForce), ForceMode.Force);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        float xVelocity = Mathf.Clamp(_rb.velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);

        if (Mathf.Abs(axis) < 0.1f)
        {
            xVelocity *= 0.9f;
        }

        _rb.velocity = new Vector3(xVelocity, _rb.velocity.y, _rb.velocity.z);

        if (IsGrounded())
        {
            _animComp.SetFloat(Speed, _rb.velocity.magnitude);
            _animComp.SetBool(Jumping,false);
        }
        else
        {
            _animComp.SetBool(Jumping, true);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && _facingRight)
        {
            _facingRight = false;
            _rb.transform.rotation = new Quaternion(_rb.transform.rotation.x, _rb.transform.rotation.y * -1, _rb.transform.rotation.z, _rb.transform.rotation.w);
        } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) && !_facingRight)
        {
            _facingRight = true;
            _rb.transform.rotation = new Quaternion(_rb.transform.rotation.x, _rb.transform.rotation.y * -1, _rb.transform.rotation.z, _rb.transform.rotation.w);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Physics.Raycast(transform.position, Vector3.up, out var hitBrick, _distanceFromBlock) && other.transform.CompareTag("brick"))
        {
            if (hitBrick.collider.CompareTag("brick"))
            {
                _gameManager.AddScore();
                _objectSpawner.SpawnBricks(hitBrick.transform.position, 3f);
                Destroy(hitBrick.transform.gameObject);
            }
        }

        if (Physics.Raycast(transform.position, Vector3.up, out var hitCoin, _distanceFromBlock) && other.transform.CompareTag("coin"))
        {
            if (hitCoin.collider.CompareTag("coin"))
            {
                _gameManager.AddScore();
                _gameManager.AddCoin();
                _objectSpawner.SpawnCoins(hitBrick.transform.position, 0.5f);
            }
        }
    }

    private Boolean IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _distanceFromGround);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("flag") && _passedFlag == false)
        {
            _passedFlag = true;
            _gameManager.AddScore();
            StartCoroutine(_gameManager.LoadLevel());
        }

        if (other.transform.tag.Equals("water"))
        {
            ResetPlayerPosition();
            Debug.Log("You hit water!");
        }
    }

    public void ResetPlayerPosition()
    {
        _rb.transform.position = _originalPos;
    }
}
