using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public float speed;
    private Vector3 _startPos;
    private Vector3 _savedVelocity;
    private Vector3 _direction;
    private Vector3 _startScale;
    private Rigidbody _rb;

    public AudioClip pongHit;
    public AudioClip wallHit;
    private AudioSource _audioSource;

    public Vector3 currentVelocity;

    public ParticleSystem particles;
    
    void Start()
    {
        _startScale = transform.localScale;
        _rb = this.GetComponent<Rigidbody>();
        _startPos = transform.position;
        speed = 5f;

        int side = Random.Range(1, 3);
        float value = side == 1 ? -1f : 1f;

        _rb.velocity = new Vector3(value * speed, 0f, speed);
        _direction = _rb.velocity;
    }

    void Update()
    {
        currentVelocity = _rb.velocity;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        particles.Play();
        //Random color on every collision
        GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        
        //Gives the ball a predictable trajectory based on collision
        Vector3 normal = other.contacts[0].normal;
        _direction = Vector3.Reflect(_direction, normal);
        _rb.velocity = _direction;
        
        //Audio
        _audioSource.clip = pongHit;

        //increase ball speed.
        var velocity = _rb.velocity;
        if (other.gameObject.name is "leftPaddle")
        {
            velocity = new Vector3(velocity.x + 1f, 0f, velocity.z + 1f);
            _rb.velocity = velocity;
            _audioSource.Play();
            
        }
        else if(other.gameObject.name is "rightPaddle")
        {
            velocity = new Vector3(velocity.x - 1f, 0f, velocity.z - 1f);
            _rb.velocity = velocity;
            _audioSource.Play();
        }
        else
        {
            _audioSource.clip = wallHit;
            _audioSource.Play();
        }

        _direction = _rb.velocity;
    }

    public void ResetBall(String winner)
    {
        speed = 5f;
        transform.position = _startPos;
        transform.localScale = _startScale;
        
        int side = Random.Range(1, 3);
        float value = side == 1 ? -1f : 1f;

        if (winner == "left")
        {
           _rb.velocity = new Vector3(speed * value, 0f, speed);
        }
        else if (winner == "right")
        {
            _rb.velocity = new Vector3(-speed * value, 0f, speed);
        }

        _direction = _rb.velocity;
    }
}