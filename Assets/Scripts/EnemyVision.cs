using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private GameObject currentHitObject;
    [SerializeField] private float circleRadius;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask layerMask;

    private EnemyController enemyController;
    private Vector2 origin;
    private Vector2 direction;
    private float currentHitDistance;
    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }
    private void Update()
    {
        origin = transform.position;


        if (enemyController.IsFacingRight)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }


        RaycastHit2D hit = Physics2D.CircleCast(origin, circleRadius, direction, maxDistance, layerMask);
        if (hit)
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
            if (currentHitObject.CompareTag("Player"))
            {
                enemyController.StartChaisingPlayer();
            }
        }
        else
        {
            currentHitObject = null;
            currentHitDistance = maxDistance;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, circleRadius);
    }
}
