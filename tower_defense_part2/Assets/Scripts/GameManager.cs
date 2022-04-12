using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager: MonoBehaviour
{
    [Header ("Enemies")]
    public Transform easyEnemyPrefab;
    public int numEnemiesToSpawn = 1;
    public Transform[] waypointList;

    [Header ("Misc")]
    public TextMeshProUGUI coinsText;

    private int _treasury = 0;
    public int towerCost = 4;

    public Button startBtn;
    public Button restartBtn;
    private int _enemiesDestroyed = 0;
    public bool enemyMadeItToEnd = false;
    
    public GameObject towers;
    public int treasuryAmount;
    
    
    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;
    
    void Start()
    {
        treasuryAmount = 12;
        startBtn.GetComponent<Button>();
        restartBtn.GetComponent<Button>();
        restartBtn.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (enemyMadeItToEnd)
        {
            //Destroy all remaining enemies
            var enemies = GameObject.FindObjectsOfType<Enemy>();
            foreach (var enemy in enemies)
            {
                Destroy(enemy.transform.gameObject);
            }
            
            restartBtn.gameObject.SetActive(true);
            onGameOver?.Invoke();
            enemyMadeItToEnd = false;
        }
    }

    // wrapped IEnumerator inside of void function for use with onClick button
    public void StartGame()
    {
        _enemiesDestroyed = 0;
        
        // Hide button when clicked
        startBtn.gameObject.SetActive(false);
        restartBtn.gameObject.SetActive(false);
        StartCoroutine(LoadEnemies());

        IEnumerator LoadEnemies()
        {
            _treasury = treasuryAmount;
            coinsText.text = $"Coins: {_treasury}";
            int enemiesSpawned = 0;
            while (enemiesSpawned < numEnemiesToSpawn)
            {
                Transform newEnemyTransform = Instantiate(easyEnemyPrefab);
                Enemy newEnemy = newEnemyTransform.GetComponent<Enemy>();
                if (newEnemy)
                {
                    newEnemy.OnEnemyDied += OnEnemyDied;
                    newEnemy.WaypointList = waypointList;
                }
                enemiesSpawned++;

                yield return new WaitForSeconds(2f);
            }
        }
    }
    
    void OnEnemyDied(Enemy deadEnemy)
    {
        deadEnemy.OnEnemyDied -= OnEnemyDied;
        _treasury += deadEnemy.coinReward;
        coinsText.text = $"Coins: {_treasury}";

        _enemiesDestroyed++;
        // All enemies have been killed. Restart?
        if (_enemiesDestroyed >= numEnemiesToSpawn)
        {
            restartBtn.gameObject.SetActive(true);
            onGameOver?.Invoke(); // Let classes subscribed to delegate know game is over.
        }
    }

    public void TreasurySubtract(int amount)
    {
        this._treasury -= amount;
        coinsText.text = $"Coins: {_treasury}";
    }
    
    public int GetTreasuryAmount()
    {
        return _treasury;
    }
}