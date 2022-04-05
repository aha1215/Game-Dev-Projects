using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootingOffset;
    public float speed = 5f;
    public ScoreManager scoreManager;
    
    private GameObject _enemyBox;
    private EnemyBoxMove _enemyBoxMove;

    private Vector3 _movement;
    private Rigidbody2D _rb;

    private Vector2 _originalPos;

    private GameObject _gm;
    private GameManager _gameManager;
    
    private Animator _playerAnimator;
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Explode = Animator.StringToHash("Explode");

    private AudioSource _audioSource;
    public AudioClip shootClip;
    public AudioClip explodeClip;

    public ParticleSystem particles;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalPos = transform.position;
        
        _enemyBox = GameObject.Find("EnemyBox");
        _enemyBoxMove = _enemyBox.GetComponent<EnemyBoxMove>();

        _gm = GameObject.Find("GameManager");
        _gameManager = _gm.GetComponent<GameManager>();

        _playerAnimator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            particles.Play();
            _audioSource.clip = shootClip;
            _audioSource.Play();
            _playerAnimator.SetTrigger(Shoot);
            GameObject shot = Instantiate(bullet, shootingOffset.position, Quaternion.identity);
            Debug.Log("pew");
            Destroy(shot, 2f);
        }

        Vector3 pos = this.transform.position;
        this.transform.position = new Vector3(pos.x + Input.GetAxis("Horizontal") * speed * Time.deltaTime, pos.y, pos.z);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.transform.CompareTag("enemyBullet"))
        {
            _audioSource.clip = explodeClip;
            _audioSource.Play();
            _playerAnimator.SetTrigger(Explode);
            StartCoroutine(ExplodePlayer(0.5f));
            Debug.Log("YOU DIED!");
        }
        if (other.transform.CompareTag("ika") || other.transform.CompareTag("kani") || other.transform.CompareTag("kura"))
        {
            _playerAnimator.SetTrigger(Explode);
            StartCoroutine(ExplodePlayer(0.5f));
            Debug.Log("GAME OVER!");
            scoreManager.ResetScore();
            _enemyBoxMove.Reset();
            transform.position = _originalPos;
        }
    }

    private IEnumerator ExplodePlayer(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        Destroy(this.gameObject);
        _gameManager.LoadScene("Credits");
    }
}
