using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public event EnemyHit OnEnemyHit;
    public delegate void EnemyHit(Enemy enemy);
    
    public GameObject enemyBullet;
    public Transform shootingOffset;

    public int ikaValue = 30;
    public int kaniValue = 20;
    public int kuraValue = 10;
    public int shipValue = 45;

    private GameObject _scoreManagerObject;
    private ScoreManager _scoreManager;

    private GameObject _enemyBox;
    private EnemyBoxMove _enemyBoxMove;

    private bool _enemyFiring = false;
    [FormerlySerializedAs("maxFireRate")] public int maxWaitSeconds = 20;
    [FormerlySerializedAs("minFireRate")] public int minWaitSeconds = 5;

    private Animator _enemyAnimator;
    private static readonly int Shoot1 = Animator.StringToHash("Shoot");
    private static readonly int Explode = Animator.StringToHash("Explode");
    
    private AudioSource _audioSource;
    public AudioClip shootClip;
    public AudioClip explodeClip;

    void Start()
    {
        _scoreManagerObject = GameObject.Find("ScoreManager");
        _scoreManager = _scoreManagerObject.GetComponent<ScoreManager>();
        
        _enemyBox = GameObject.Find("EnemyBox");
        _enemyBoxMove = _enemyBox.GetComponent<EnemyBoxMove>();
        _enemyAnimator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_enemyFiring)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        _enemyFiring = true;
        yield return new WaitForSeconds(Random.Range(minWaitSeconds, maxWaitSeconds));
        _audioSource.clip = shootClip;
        _audioSource.Play();
        _enemyAnimator.SetTrigger(Shoot1);
        GameObject shot = Instantiate(enemyBullet, shootingOffset.position, Quaternion.identity);
        Destroy(shot, 2f);
        _enemyFiring = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet"))
        {
            if (this.CompareTag("ika"))
            {
                _scoreManager.AddScore(ikaValue);
            }
            else if (this.CompareTag("kani"))
            {
                _scoreManager.AddScore(kaniValue);
            } 
            else if (this.CompareTag("kura"))
            {
                _scoreManager.AddScore(kuraValue);
            } 
            else if (this.CompareTag("ship"))
            {
                _scoreManager.AddScore(shipValue);    
            }
        }
        
        if (!other.CompareTag("enemyBullet") && !other.CompareTag("leftwall") && !other.CompareTag("rightwall") && !other.CompareTag("player"))
        {
            _audioSource.clip = explodeClip;
            _audioSource.Play();
            _enemyAnimator.SetTrigger(Explode);
            Destroy(other.gameObject); // destroy bullet that hit enemy
            OnEnemyHit?.Invoke(this);
            //Destroy(this.gameObject); //destroy enemy
            StartCoroutine(DestroyEnemyDelay(0.35f));
            if (_enemyBoxMove.speed < 750f)
            {
                _enemyBoxMove.speed += 25f;
            }

            if (_enemyBoxMove.moveSpeedTimeDelay > 0)
            {
                _enemyBoxMove.moveSpeedTimeDelay -= 0.25f;
            }
        }
    }

    private IEnumerator DestroyEnemyDelay(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        Destroy(this.gameObject);
    }
}
