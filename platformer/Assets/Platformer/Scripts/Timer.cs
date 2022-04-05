using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float accumulatedTime = 0f;
    public float startingTime = 100;

    private float _timeLimit = 101;

    public TextMeshProUGUI worldTime;

    private bool _gameOver = false;

    public PlayerController player;
    public Game game;

    public LevelParser level;
    // Update is called once per frame
    void Update()
    {
        accumulatedTime += Time.deltaTime;

        if (accumulatedTime > 1f && !_gameOver)
        {
            startingTime -= 1f;
            accumulatedTime = 0f;
            worldTime.SetText($"Time \n {Math.Floor(startingTime)}");
        }
        

        if (Time.timeSinceLevelLoad >= _timeLimit && !_gameOver)
        {
            Debug.Log("Game over! You Lost.");
            _gameOver = true;
            player.ResetPlayerPosition();
            ResetTimer();
            game.ResetStats();
            level.ReloadLevel();
        }
        
    }

    public void ResetTimer()
    {
        startingTime = 100;
        accumulatedTime = 0f;
        _timeLimit += 100f;
        _gameOver = false;
    }
}
