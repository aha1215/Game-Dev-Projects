using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip barricadeHitClip;
    public event BarricadeHit OnBarricadeHit;
    public delegate void BarricadeHit(Barricade barricade);

    public int health = 100;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet") || other.CompareTag("enemyBullet"))
        {
            Destroy(other.gameObject); // Destroy bullet that hit
        }
        
        this.transform.localScale = new Vector3(this.transform.localScale.x - 0.10f, this.transform.localScale.y - 0.10f,
            this.transform.localScale.z);
        
        health -= 10;

        _audioSource.clip = barricadeHitClip;
        _audioSource.Play();
        
        if (health == 0)
        {
            OnBarricadeHit?.Invoke(this);
            Destroy(this.gameObject);
        }
    }
}
