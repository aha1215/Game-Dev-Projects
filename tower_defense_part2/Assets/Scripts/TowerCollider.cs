using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCollider : MonoBehaviour
{
    private Tower _parent;

    private void Start()
    {
        _parent = transform.parent.GetComponent<Tower>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("enemyBullet"))
        {
            _parent.health--;
            _parent.healthBarScript.SetHealth(_parent.health);
            Destroy(other.gameObject);
        }

        if (_parent.health <= 0)
        {
            _parent.currentTowerBase.GetComponent<TowerBase>().used = false;
            GameManager.onGameOver -= _parent.DestroyTower;
            Destroy(_parent.healthBar);
            Destroy(_parent.gameObject);
        }
    }
}
