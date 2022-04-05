using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private int _coinsScore;
    private int _score;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI scoreText;
    
    public Timer timer;
    public LevelParser level;
    public GameObject mario;

    public GameObject background;
    private ChangeBackground _changeBackground;

    private Vector3 _originalMarioPos;

    private void Start()
    {
        _coinsScore = 0;
        _score = 0;
        _originalMarioPos = mario.transform.position;
        
        background = GameObject.Find("BackgroundTemplate");
        _changeBackground = background.GetComponent<ChangeBackground>();
    }

    // Update is called once per frame
    void Update()
    {
        coinText.SetText(_coinsScore.ToString());
        scoreText.SetText("Mario" + "\n" + _score.ToString());
    }

    public void AddCoin()
    {
        _coinsScore += 1;
    }

    public void AddScore()
    {
        _score += 100;
    }

    public IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(5);
        
        level.filename = "level2";
        level.ReloadLevel();
        timer.ResetTimer();
        mario.transform.position = _originalMarioPos;
        _changeBackground.UpdateSprite();
    }

    public void ResetStats()
    {
        _coinsScore = 0;
        _score = 0;
    }
}
