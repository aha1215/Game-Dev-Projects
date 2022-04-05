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
    
    //-----------------------------------------------------------------------------
    void Start()
    {
        // Create instance of health bar prefab 
        healthBar = Instantiate(healthBar);
        _healthBarScript = healthBar.transform.GetChild(0).GetComponent<HealthBar>();

        myAgent = GetComponent<NavMeshAgent>();

        //Place our enemy at the start point
        Vector3 startingPosition = GetNavmeshPosition(WaypointList[0].transform.position);
        myAgent.Warp(startingPosition);

        transform.LookAt(GetNavmeshPosition(WaypointList[1].transform.position));
        TargetNextWaypoint();
    }

    //-----------------------------------------------------------------------------
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

        UpdateTestDamage();
    }

    //-----------------------------------------------------------------------------
    void UpdateTestDamage()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray pickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(pickRay, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.transform == transform)
                {
                    health -= 1;
                    _healthBarScript.SetHealth(health);
                    if (health <= 0)
                    {
                        OnEnemyDied?.Invoke(this);
                        Destroy(gameObject);
                        Destroy(healthBar);
                    }
                }
            }
        }
    }

    //-----------------------------------------------------------------------------
    Vector3 GetNavmeshPosition(Vector3 samplePosition)
    {
        NavMesh.SamplePosition(samplePosition, out NavMeshHit hitInfo, 100, -1);
        return hitInfo.position;
    }

    //-----------------------------------------------------------------------------
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
}
