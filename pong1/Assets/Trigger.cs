using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public Ball ball;
    public Game game;
    public bool leftTrigger;

    
    public AudioClip scorePoint;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        _audioSource.clip = scorePoint;
        _audioSource.Play();
        
        if (leftTrigger)
        {
            Debug.Log("Right Scored!");
            ball.ResetBall("right");
            game.UpdateScore(leftTrigger);
        }
        else
        {
            Debug.Log("Left Scored!");
            ball.ResetBall("left");
            game.UpdateScore(leftTrigger);
        }
        game.CheckScore();
    }
}
