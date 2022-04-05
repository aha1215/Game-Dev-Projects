using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycast : MonoBehaviour
{
    private GameObject _gameManager;
    private Game _game;

    private GameObject _spawner;
    private Spawner _objectSpawner;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager");
        _game = _gameManager.GetComponent<Game>();

        _spawner = GameObject.Find("ObjectSpawner");
        _objectSpawner = _spawner.GetComponent<Spawner>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out var hit) && hit.transform.gameObject.tag.Equals("brick"))
            {
                //Debug.DrawRay(Camera.main.transform.position, hit, Color.black);
                
                _objectSpawner.SpawnBricks(hit.transform.position, 3f);
                Destroy(hit.transform.gameObject);
            }

            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag.Equals("coin"))
            {
                _game.AddCoin();
                _objectSpawner.SpawnCoins(hit.transform.position, 0.5f);
            }
        }
    }
}
