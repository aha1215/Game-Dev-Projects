using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject barricade;
    
    public GameObject ika;
    public GameObject kani;
    public GameObject kura;
    public GameObject spaceship;

    public Transform barricadeBox;
    public Transform enemyBox;
    public Transform shipSpawnPointLeft;
    public Transform shipSpawnPointRight;

    public int enemyColCount = 4;
    public float spaceShipSpeed = 5f;

    private bool _shipSpawnedLeft = false;

    private EnemyBoxMove _enemyBoxMove;
    private void Start()
    {
        _enemyBoxMove = enemyBox.GetComponent<EnemyBoxMove>();
        SpawnBarricades();
        SpawnEnemies(enemyColCount);
    }

    private void Update()
    {
        if (!_shipSpawnedLeft)
        {
            StartCoroutine(SpawnSpaceShip());
        }

        if (enemyBox.transform.childCount == 0)
        {
            enemyBox.position = _enemyBoxMove.GetOriginalPosition();
            _enemyBoxMove.moveRight = true;
            _enemyBoxMove.moveLeft = false;
            SpawnEnemies(enemyColCount);

            if (enemyColCount < 8) // Max size of columns is 9. Could make var but then it goes off screen > 9.
            {
                enemyColCount++;
            }
        }
    }

    public void SpawnBarricades()
    {
        var pos = barricadeBox.position;
        var barrier1 = Instantiate(barricade, new Vector3(pos.x - 6f,pos.y,pos.z), Quaternion.identity, barricadeBox);
        var barrier2 = Instantiate(barricade, new Vector3(pos.x - 3f,pos.y,pos.z), Quaternion.identity, barricadeBox);
        var barrier3 = Instantiate(barricade,new Vector3(pos.x,pos.y - 0,pos.z) , Quaternion.identity, barricadeBox);
        var barrier4 = Instantiate(barricade,new Vector3(pos.x + 3f,pos.y,pos.z) , Quaternion.identity, barricadeBox);
        var barrier5 = Instantiate(barricade,new Vector3(pos.x + 6f,pos.y,pos.z) , Quaternion.identity, barricadeBox);
    }

    public void SpawnEnemies(float col)
    {
        var pos = enemyBox.position;

        //spawn bottom row
        for (float x = 0f; x <= col; x += 1f)
        {
            var enemyNegative = Instantiate(kura, new Vector3(pos.x - x,pos.y,pos.z), Quaternion.identity, enemyBox);
            var enemyPositive = Instantiate(kura, new Vector3(pos.x + x, pos.y, pos.z), Quaternion.identity, enemyBox);
        }
        
        // spawn middle row
        for (float x = 0f; x <= col; x += 1f)
        {
            var enemyNegative = Instantiate(kani, new Vector3(pos.x - x,pos.y + 1f,pos.z), Quaternion.identity, enemyBox);
            var enemyPositive = Instantiate(kani, new Vector3(pos.x + x, pos.y + 1f, pos.z), Quaternion.identity, enemyBox);
        }
        
        // spawn top row
        for (float x = 0f; x <= col; x += 1f)
        {
            var enemyNegative = Instantiate(ika, new Vector3(pos.x - x,pos.y + 2f,pos.z), Quaternion.identity, enemyBox);
            var enemyPositive = Instantiate(ika, new Vector3(pos.x + x, pos.y + 2f, pos.z), Quaternion.identity, enemyBox);
        }
    }

    public IEnumerator SpawnSpaceShip()
    {
        _shipSpawnedLeft = true;
        yield return new WaitForSeconds(Random.Range(15, 30));
        
        var pos = shipSpawnPointLeft.position;
        var ship = Instantiate(spaceship, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity, shipSpawnPointLeft);
        var shipRb = ship.GetComponent<Rigidbody2D>();
        
        shipRb.velocity = Vector2.right * spaceShipSpeed; 
        Destroy(ship, 10f);
        
        _shipSpawnedLeft = false;
    }
}
