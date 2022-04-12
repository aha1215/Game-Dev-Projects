using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public bool used;
    public GameObject towerPrefab;
    public GameObject gameManager;
    private GameManager _manager;

    public GameObject towers;
    private Transform _towerRoot;

    private void Start()
    {
        GameManager.onGameOver += ResetTowerUsed;
        _towerRoot = towers.GetComponent<Transform>();
        used = false;
        _manager = gameManager.GetComponent<GameManager>();
    }

    void ResetTowerUsed()
    {
        used = false;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && Camera.main != null) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            
            if (Physics.Raycast(ray, out var hit) && used == false)
            {
                if (_manager.GetTreasuryAmount() < _manager.towerCost)
                {
                    Debug.Log("Cannot afford tower! Tower cost: " + _manager.towerCost);
                }
                else
                {
                    GameObject instance = Instantiate(towerPrefab, _towerRoot);
                    var pos = transform.position;
                    instance.transform.position = new Vector3(pos.x, pos.y + 2f, pos.z);
                    instance.GetComponent<Tower>().currentTowerBase = transform.gameObject;
                    used = true;
                    _manager.TreasurySubtract(_manager.towerCost);
                }
            }
        }
    }
}
