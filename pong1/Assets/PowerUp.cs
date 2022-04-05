using UnityEngine;


public class PowerUp : MonoBehaviour
{
    public Ball ball;
    public Paddle leftPaddle;
    public Paddle rightPaddle;

    public bool powerup1;
    public bool powerup2;

    private float _ballTimer;
    private float _paddleTimer;

    private bool _ballPowerUp = false;
    private bool _paddlePowerUp = false;

    private Vector3 _originalPaddleScale;
    private Vector3 _originalBallScale;

    public float multiplierSmall = 2.0f;
    void Start()
    {
        _originalPaddleScale = leftPaddle.transform.localScale;
        _originalBallScale = ball.transform.localScale;
    }

    private void Update()
    {
        if (_ballPowerUp)
        {
            _ballTimer += Time.deltaTime;

            if (_ballTimer > 5.0f)
            {
                _ballPowerUp = false;
                _ballTimer = 0f;
                ball.transform.localScale = _originalBallScale;
            }
        }

        if (_paddlePowerUp)
        {
            _paddleTimer += Time.deltaTime;

            if (_paddleTimer > 5.0f)
            {
                _paddlePowerUp = false;
                _paddleTimer = 0f;
                leftPaddle.transform.localScale = _originalPaddleScale;
                rightPaddle.transform.localScale = _originalPaddleScale;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (powerup1)
        {
            ball.transform.localScale *= 1.2f;
            _ballPowerUp = true;
            _ballTimer = 0f;
        } 
        else if (powerup2)
        {
            int value = UnityEngine.Random.Range(1, 3);

            if (value == 1)
            {
                leftPaddle.transform.localScale = new Vector3(leftPaddle.transform.localScale.x, leftPaddle.transform.localScale.y * 1.5f, leftPaddle.transform.localScale.z);
            }

            if (value != 1)
            {
                rightPaddle.transform.localScale = new Vector3(rightPaddle.transform.localScale.x, rightPaddle.transform.localScale.y * 1.5f, rightPaddle.transform.localScale.z);
            }
            
            
            _paddlePowerUp = true;
            _paddleTimer = 0f;
        }
    }
}
