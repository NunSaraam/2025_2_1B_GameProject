using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 50f;
    public float attackRange = 2f;

    private NavMeshAgent agent;
    private float distanceToPlayer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);           //플레이어와의 거리 측정

        if (distanceToPlayer <= chaseRange)             //추적 범위에 들어오면 추적 시작
        {
            ChasePlayer();
        }
        else
        {
            StopChasing();
        }

        if (distanceToPlayer <= attackRange)            //공격 범위에 들어오면 공격
        {
            Attack();
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void StopChasing()
    {
        agent.isStopped = true;
    }

    void Attack()
    {
        agent.isStopped = true;
        transform.LookAt(player);
        Debug.Log("플레이어 공격");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);                  //추적 범위를 노란색 원으로 표시

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);                 //공격 범위를 빨간색 원으로 표시
    }
}
