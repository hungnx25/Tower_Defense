using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //event khi enemy den end point
    public static Action<Enemy> OnEndReached;


    [SerializeField] private float moveSpeed = 5f;

    public float MoveSpeed { get; set; }
    public WayPoint WayPoint { get; set; }
    public EnemyHealth EnemyHealth { get; set; }
    public Vector3 CurrentPointPosition => WayPoint.GetWaypointPosition(_currentWaypointIndex);

    private int _currentWaypointIndex;
    private Vector3 _lastPointPosition;

    private EnemyHealth _enemyHealth;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyHealth = GetComponent<EnemyHealth>();

        _currentWaypointIndex = 0;
        MoveSpeed = moveSpeed;
        _lastPointPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        if (CurrentPointPositionReached())
        {
            UpdateWayPointIndex();
        }
    }

    public void Rotate()
    {
        if(CurrentPointPosition.x > _lastPointPosition.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            CurrentPointPosition, MoveSpeed * Time.deltaTime);
    }

    public void StopMovement()
    {
        MoveSpeed = 0f;
    }

    public void ResumeMovement()
    {
        MoveSpeed = moveSpeed;
    }
    
    //check xem da di den waypoint hien tai chua de di chuyen den waypoint ke tiep 
    private bool CurrentPointPositionReached()
    {
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;
        if(distanceToNextPointPosition < 0.1f)
        {
            _lastPointPosition = transform.position;
            return true;
        }
        return false;
    }

    //Update waypoint index 
    private void UpdateWayPointIndex()
    {
        int lastWaypointIndex = WayPoint.Points.Length - 1;
        if(_currentWaypointIndex < lastWaypointIndex)
        {
            _currentWaypointIndex++;
        }
        else
        {
            EndPointReached();
        }
    }

    //tra gameobject ve pool khi kich hoat su kien
    private void EndPointReached()
    {
        OnEndReached?.Invoke(this);
        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(gameObject);
    }


    //reset current waypoint index ve 0 
    public void ResetEnemy()
    {
        _currentWaypointIndex = 0;
    }
}
