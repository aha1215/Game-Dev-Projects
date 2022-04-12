using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int maxHealth = 6;
    public int health;
    public float waitToShootSeconds = 2;

    public Collider lookAtEnemy = null;
    public bool triggeredTower = false;
    private bool _shoot = false;

    public GameObject projectile;
    private Transform _shootOffset;
    private static List<Collider> _enemiesInView = new List<Collider>();
    private GameManager _gameManager;
    
    public GameObject healthBar;
    public HealthBar healthBarScript;

    public GameObject currentTowerBase;

    private void Start()
    {
        health = maxHealth;
        // Subscribed to delegate onGameOver. Now whenever it's invoked it invokes the function DestroyTower too!
        GameManager.onGameOver += DestroyTower;
        _shootOffset = transform.GetChild(1).GetComponent<Transform>();
        _gameManager = FindObjectOfType<GameManager>();
        
        // Tower health bar
        var pos = transform.position;
        healthBar = Instantiate(healthBar,new Vector3(pos.x, pos.y + 3f, pos.z), Quaternion.identity, transform);
        healthBarScript = healthBar.transform.GetChild(0).GetComponent<HealthBar>();
    }

    private void Update()
    {
        if (lookAtEnemy != null)
        {
            transform.LookAt(lookAtEnemy.transform.position);
            _shootOffset.rotation = transform.rotation;
            if (!_shoot)
            {
                StartCoroutine(TowerShoot());
            }
        }
        IncreaseDefenses();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("enemyBullet"))
        {
            Destroy(collision.transform.gameObject);
        }
    }

    void IncreaseDefenses()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray pickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(pickRay, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.transform == transform)
                {
                    // Help defend tower (increase its health at the cost of 1 coin)
                    if (_gameManager.GetTreasuryAmount() > 0 && health < maxHealth)
                    {
                        health += 2;
                        healthBarScript.SetHealth(health);
                        _gameManager.TreasurySubtract(2);
                    }
                }
            }
        }
    }

    private IEnumerator TowerShoot()
    {
        _shoot = true;
        yield return new WaitForSeconds(waitToShootSeconds);
        GameObject proj = Instantiate(this.projectile, _shootOffset.position, transform.rotation);
        if (proj != null)
        {
            Destroy(proj, 2f);
        }
        _shoot = false;
    }
    public void DestroyTower()
    {
        GameManager.onGameOver -= DestroyTower;
        Destroy(this.transform.gameObject);
    }
}
