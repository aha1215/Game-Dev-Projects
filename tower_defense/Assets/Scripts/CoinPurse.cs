using System;
using TMPro;
using UnityEngine;

public class CoinPurse : MonoBehaviour
{
    private int _coins;
    
    public delegate void EnemyDied(Enemy deadEnemy);

    public TextMeshProUGUI coinsText;

    private void Start()
    {
        FindObjectOfType<Enemy>().OnEnemyDied += OnEnemyDeath; // Subscribe to delegate
        coinsText.text = "Coins: " + _coins.ToString("D3");
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        _coins += enemy.coins;
        coinsText.text = "Coins: " + _coins.ToString("D3");
        Debug.Log("TOTAL COINS: " + _coins);
        FindObjectOfType<Enemy>().OnEnemyDied -= OnEnemyDeath; // Unsubscribe to delegate
    }
}
