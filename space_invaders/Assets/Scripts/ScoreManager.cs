using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private int _score = 0;
    private int _highScore;
    
    private void Start()
    {
        _highScore = PlayerPrefs.GetInt("high_score"); // get high score
        highScoreText.text = _highScore.ToString("D4");
    }

    public void AddScore(int scoreAmount)
    {
        _score += scoreAmount;
        scoreText.text = _score.ToString("D4");

        if (_score > _highScore)
        {
            _highScore = _score;
            highScoreText.text = _highScore.ToString("D4");
            
            PlayerPrefs.SetInt("high_score", _highScore); // save high score to player preferences
        }
    }

    public void ResetScore()
    {
        _score = 0;
        scoreText.text = _score.ToString("D4");
    }
}
