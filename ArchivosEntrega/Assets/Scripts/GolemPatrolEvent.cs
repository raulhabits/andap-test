using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GolemPatrolEvent : MonoBehaviour
{
    [Header("References")]
    public Vector3[] patrolPoints;
    public Transform player;

    [Header("Settings")]
    public float chaseRange = 10f;
    public float stopDistance = 2f;
    public float waitTimeAtPoint = 2f;

    private NavMeshAgent agent;
    private int patrolIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    private enum State { Patrolling, Chasing }
    private State currentState = State.Patrolling;

    void Start()
    {
        Debug.Log("Start: " + gameObject.name + "");
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (patrolPoints.Length > 0)
            agent.destination = patrolPoints[0];
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // STATE SWITCHING
        if (distanceToPlayer <= chaseRange)
        {
            currentState = State.Chasing;
        }
        else
        {
            currentState = State.Patrolling;
        }

        if (currentState == State.Chasing)
        {
            ChasePlayer(distanceToPlayer);
        }
        else if (currentState == State.Patrolling)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = waitTimeAtPoint;
            }
            else
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    isWaiting = false;
                    patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                    agent.SetDestination(patrolPoints[patrolIndex]);
                }
            }
        }
    }

    void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > stopDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
            FacePlayer();
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction.magnitude > 0)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }
}