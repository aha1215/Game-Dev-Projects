using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    private int _leftScore;
    private int _rightScore;
    private int _leftWon;
    private int _rightWon;
    
    public Paddle leftPaddle;
    public Paddle rightPaddle;

    public Trigger leftTrigger;
    public Trigger rightTrigger;
    
    public Ball ball;

    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;
    
    void Update()
    {
        leftScoreText.text = _leftScore.ToString();
        rightScoreText.text = _rightScore.ToString();
    }

    public void UpdateScore(bool leftGoal)
    {
        if (leftGoal)
        {
            _rightScore++;
        }
        else
        {
            _leftScore++;
        }
    }

    public void CheckScore()
    {
        Debug.Log("Current Score | Left: " + _leftScore + " | Right: " + _rightScore);

        if (_leftScore > _rightScore)
        {
            leftScoreText.color = Color.green;
            rightScoreText.color = Color.red;
        }
        else if (_rightScore > _leftScore)
        {
            rightScoreText.color = Color.green;
            leftScoreText.color = Color.red;
        }
        else
        {
            rightScoreText.color = Color.green;
            leftScoreText.color = Color.green;
        }

        if (_leftScore >= 11)
        {
            Debug.Log("Game Over, Left Paddle Wins");
            ball.ResetBall("left");
            
            _leftWon++;
            _leftScore = 0;
            _rightScore = 0;
        } 
        else if (_rightScore >= 11)
        {
            Debug.Log("Game Over, Right Paddle Wins");
            ball.ResetBall("right");
            
            _rightWon++;
            _leftScore = 0;
            _rightScore = 0;
        }
    }
}
