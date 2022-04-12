using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour
{
    public int health = 3;
    public int coinReward = 2;
    public float lookAheadDistance = 2f;

    public Transform[] WaypointList { get; set; }

    public delegate void EnemyDied(Enemy deadEnemy);
    public event EnemyDied OnEnemyDied;

    public bool showDebugVisuals;
    public Vector3 debugVec;

    private NavMeshAgent myAgent;
    private Vector3 targetPosition;
    private int index = 0;
    
    public GameObject healthBar;
    private HealthBar _healthBarScript;

    private AudioSource _audioSource;
    public AudioClip deathClip;

    private GameManager _gameManager;
    
    public GameObject projectile;
    private bool _shoot = false;
    public Transform closestTower;
    public float waitToShootSeconds = 4;
    private Transform _shootOffset;

    void Start()
    {
        _shootOffset = transform.GetChild(0).GetComponent<Transform>();
        _gameManager = FindObjectOfType<GameManager>();
        // Death sound
        _audioSource = GetComponent<AudioSource>();

        // Create instance of health bar prefab 
        healthBar = Instantiate(healthBar, transform);
        _healthBarScript = healthBar.transform.GetChild(0).GetComponent<HealthBar>();

        myAgent = GetComponent<NavMeshAgent>();

        //Place our enemy at the start point
        Vector3 startingPosition = GetNavmeshPosition(WaypointList[0].transform.position);
        myAgent.Warp(startingPosition);

        transform.LookAt(GetNavmeshPosition(WaypointList[1].transform.position));
        TargetNextWaypoint();
    }
    
    void Update()
    {
        // update health bar position
        var pos = transform.position;
        healthBar.transform.position = new Vector3(pos.x, pos.y + 2f, pos.z);
        
        Vector3 position2D = transform.position;
        position2D.z = targetPosition.z;

        Vector3 directionToTarget = targetPosition - position2D;
        Vector3 newPosition = position2D + directionToTarget.normalized * lookAheadDistance;
        Vector3 newDirectionToTarget = targetPosition - newPosition;

        if (showDebugVisuals)
        {
            Debug.DrawRay(position2D, directionToTarget, Color.green);
            Debug.DrawRay(position2D,newDirectionToTarget, Color.red);
            DebugDraw.DrawSphere(newPosition, 0.5f, Color.red);
        }

        debugVec = directionToTarget.normalized;

        // Check if we passed our target
        if (Vector3.Dot(directionToTarget, newDirectionToTarget) < 0f)
        {
            // Debug.Break();
            TargetNextWaypoint();
        }
        
        // Enemy reached the end, game over
        if (index == WaypointList.Length - 1 && Vector3.Dot(directionToTarget, newDirectionToTarget) < 0f && _gameManager.enemyMadeItToEnd == false)
        {
            _gameManager.enemyMadeItToEnd = true;
        }
        if (!_shoot && closestTower != null) 
        {
            StartCoroutine(EnemyShoot());
        }

        if (closestTower != null)
        {
            transform.LookAt(closestTower.position);
        }

        UpdateTestDamage();
    }
    private IEnumerator EnemyShoot()
    {
        _shoot = true;
        yield return new WaitForSeconds(waitToShootSeconds);
        GameObject proj = Instantiate(projectile, _shootOffset.position, transform.rotation);
        proj.GetComponent<Renderer>().material.color = Color.red;
        if (proj != null)
        {
            Destroy(proj, 2f);
        }
        _shoot = false;
    }
    
    void UpdateTestDamage()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray pickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(pickRay, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.transform == transform)
                {
                    UpdateHealth();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile>() != null && !other.CompareTag("enemyBullet"))
        {
            UpdateHealth();
        }
    }

    void UpdateHealth()
    {
        health--;
        _healthBarScript.SetHealth(health);
        if (health <= 0)
        {
            StartCoroutine(EnemyDeath());
        }
    }
    
    Vector3 GetNavmeshPosition(Vector3 samplePosition)
    {
        NavMesh.SamplePosition(samplePosition, out NavMeshHit hitInfo, 100, -1);
        return hitInfo.position;
    }
    
    private void TargetNextWaypoint()
    {
        if (index < WaypointList.Length - 1)
        {
            index += 1;
            targetPosition = GetNavmeshPosition(WaypointList[index].transform.position);
            myAgent.SetDestination(targetPosition);

            //Debug.Log($"Set Destination to point {index}");
        }
    }

    IEnumerator EnemyDeath()
    {
        _audioSource.clip = deathClip;
        _audioSource.Play();
        while (_audioSource.isPlaying)
        {
            yield return null;
        }
        OnEnemyDied?.Invoke(this);
        EnemyDestroy();
    }

    void EnemyDestroy()
    {
        Destroy(healthBar);
        Destroy(gameObject);
    }
}
