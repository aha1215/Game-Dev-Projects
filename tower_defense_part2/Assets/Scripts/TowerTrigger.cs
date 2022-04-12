using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerTrigger : MonoBehaviour
{
    private Tower _parent;
    private static List<Collider> _enemiesInView = new List<Collider>();

    private void Start()
    {
        _parent = transform.parent.GetComponent<Tower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            // Let enemy know which tower is closest (so it can shoot at tower!)
            other.transform.GetComponent<Enemy>().closestTower = _parent.transform;
            _enemiesInView.Add(other);
            other.GetComponent<Enemy>().OnEnemyDied += OnEnemyDied;
        }

        if (_enemiesInView.Contains(other) && _parent.lookAtEnemy == null)
        {
            _parent.lookAtEnemy = other;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            other.transform.GetComponent<Enemy>().closestTower = null;
            _enemiesInView.Remove(other);
            if (_enemiesInView.Any())
            {
                _parent.lookAtEnemy = _enemiesInView[0];
            }
            else
            {
                _parent.triggeredTower = false;
                _parent.lookAtEnemy = null;
            }
        }
    }
    
    void OnEnemyDied(Enemy deadEnemy)
    {
        deadEnemy.OnEnemyDied -= OnEnemyDied;
        if (_enemiesInView.Contains(deadEnemy.GetComponent<Collider>()))
        {
            _enemiesInView.Remove(deadEnemy.GetComponent<Collider>());
        }

        if (_enemiesInView.Any())
        {
            _parent.lookAtEnemy = _enemiesInView[0];
        }
    }
}
