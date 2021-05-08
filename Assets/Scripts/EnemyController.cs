using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float walkDistance = 6f;
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float timeToWait = 2f;
    [SerializeField] private float timeToChase = 3f;
    [SerializeField] private float chaisingSpeed = 3f;
    [SerializeField] private float minDistanceToPlayer = 1.5f;
    [SerializeField] Transform enemyModelTransform;

    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private Vector2 _leftBoundaryPosition;
    private Vector2 _rightBoundaryPosition;
    private Vector2 newPos;

    private bool _isFacingRight = true;
    private bool _isWaiting = false;
    private bool _isChaisingPlayer;

    private float _waitTime;
    private float _chaseTime;
    private float _walkSpeed;

    public bool IsFacingRight
    {
        get => _isFacingRight;
    }


    public void StartChaisingPlayer()
    {
        _isChaisingPlayer = true;
        _chaseTime = timeToChase;
        _walkSpeed = chaisingSpeed;
    }
    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _leftBoundaryPosition = transform.position;
        _rightBoundaryPosition = _leftBoundaryPosition + Vector2.right * walkDistance;
        _waitTime = timeToWait;
        _chaseTime = timeToChase;
        _walkSpeed = patrolSpeed;
    }
    private void Update()
    {
        if (_isChaisingPlayer)
        {
            StartChaisingTimer();
        }

        if (_isWaiting && !_isChaisingPlayer)
        { Wait(); }


        if (ShouldWait())
        {
            _isWaiting = true;
        }
    }

    private void FixedUpdate()
    {
        newPos = Vector2.right * _walkSpeed * Time.fixedDeltaTime;
        if (_isChaisingPlayer && Mathf.Abs(DistanceToPlayer()) < minDistanceToPlayer)
        {
            return;
        }

        if (_isChaisingPlayer)
        {
            ChasePlayer();
        }

        if (!_isWaiting && !_isChaisingPlayer)
        {
            Patrol();
        }
    }

    private float DistanceToPlayer()
    {
        return (_playerTransform.position.x - transform.position.x);
    }

    private void Patrol()
    {
        if (!_isFacingRight)
        {
            newPos.x *= -1f;
        }
        _rb.MovePosition((Vector2)transform.position + newPos);
    }

    private void ChasePlayer()
    {
        float distance = DistanceToPlayer();
        if (distance < 0f)
        {
            newPos.x *= -1f;
        }

        if (distance > 0.2f && !_isFacingRight)
        {
            Flip();
        }
        else if (distance < 0.2f && _isFacingRight)
        {
            Flip();
        }
        _rb.MovePosition((Vector2)transform.position + newPos);
    }

    private bool ShouldWait()
    {
        bool isOutOfRightBoundary = _isFacingRight && (transform.position.x >= _rightBoundaryPosition.x);
        bool isOutOfLeftBoundary = !_isFacingRight && (transform.position.x <= _leftBoundaryPosition.x);

        return isOutOfLeftBoundary || isOutOfRightBoundary;
    }

    private void Wait()
    {
        _waitTime -= Time.deltaTime;
        if (_waitTime < 0f)
        {
            _waitTime = timeToWait;
            _isWaiting = false;
            Flip();
        }
    }

    private void StartChaisingTimer()
    {
        _chaseTime -= Time.deltaTime;
        if (_chaseTime < 0f)
        {
            _isChaisingPlayer = false;
            _chaseTime = timeToChase;
            _walkSpeed = patrolSpeed;
        }

    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 enemyScale = enemyModelTransform.localScale;
        enemyScale.x *= -1f;
        enemyModelTransform.localScale = enemyScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_leftBoundaryPosition, _rightBoundaryPosition);
    }
}
