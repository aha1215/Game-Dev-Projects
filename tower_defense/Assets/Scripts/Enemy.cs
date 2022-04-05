using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 5;
    public float speed = 1f;
    public int coins = 3;

    public List<Transform> wayPointList;
    private int _targetWaypointIndex;

    public delegate void EnemyDied(Enemy deadEnemy); // delegate and event to know when enemy dies
    public event EnemyDied OnEnemyDied;
    
    private Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    private Vector3 _targetPosition;
    private Vector3 _movementDirBefore;
    private Vector3 _enemyPos;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        transform.position = wayPointList[0].position; // Enemy starts at spawn point
        _targetPosition = wayPointList[0].position;
        _targetWaypointIndex = 0;
    }
    
    void Update()
    {
        _movementDirBefore = (_targetPosition - transform.position).normalized;
        transform.position += _movementDirBefore * speed * Time.deltaTime;
        _enemyPos = transform.position;
        var movementDirAfter = (_targetPosition - _enemyPos).normalized; // after movement
        
        if (Vector3.Dot(_movementDirBefore, movementDirAfter) < 0 && _targetWaypointIndex < wayPointList.Count)
        {
            transform.position = _targetPosition; // Clamp position
            _targetPosition = wayPointList[_targetWaypointIndex++].position;
            transform.LookAt(new Vector3(_targetPosition.x, _enemyPos.y, _enemyPos.z));
        }
        if (this.health < 0)
        {
            OnEnemyDied?.Invoke(this);
            Destroy(this.gameObject);
        }
        _animator.SetFloat(Speed, speed);
        
        // For video showcase only (loops enemy)
        if (_targetWaypointIndex >= 28)  
        {  
            transform.position = wayPointList[0].position; _targetPosition = wayPointList[0].position; _targetWaypointIndex = 0;
        }
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && Camera.main != null) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            
            if (Physics.Raycast(ray, out var hit))
            {
                health--;
            }
        }
    }
}