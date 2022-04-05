using System.Collections;
using TMPro;
using UnityEngine;

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

    //-----------------------------------------------------------------------------
    IEnumerator Start()
    {
        _treasury = 4;
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

    //-----------------------------------------------------------------------------
    void OnEnemyDied(Enemy deadEnemy)
    {
        deadEnemy.OnEnemyDied -= OnEnemyDied;
        _treasury += deadEnemy.coinReward;
        coinsText.text = $"Coins: {_treasury}";
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