using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    private bool _used;
    public GameObject towerPrefab;
    public GameObject gameManager;
    private GameManager _manager;

    private void Start()
    {
        _used = false;
        _manager = gameManager.GetComponent<GameManager>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && Camera.main != null) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            
            if (Physics.Raycast(ray, out var hit) && _used == false)
            {
                if (_manager.GetTreasuryAmount() < _manager.towerCost)
                {
                    Debug.Log("Cannot afford tower! Tower cost: " + _manager.towerCost);
                }
                else
                {
                    GameObject instance = Instantiate(towerPrefab);
                    var pos = transform.position;
                    instance.transform.position = new Vector3(pos.x, pos.y + 2f, pos.z);
                    _used = true;
                    _manager.TreasurySubtract(_manager.towerCost);
                }
            }
        }
    }
}
