using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;

    public LayerMask playerMask; 
    public LayerMask obstacleMask;

    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;


    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    private Animator anim;
    
    void Start()
    {
        GameObject originalGameObject = GameObject.Find("EnemyTrippo");
        GameObject child = originalGameObject.transform.GetChild(0).gameObject;

        anim = child.GetComponent<Animator>();

        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;
        NavMeshAgent = GetComponent<NavMeshAgent>();

        NavMeshAgent.isStopped = false;
        NavMeshAgent.speed = speedWalk;
        NavMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        
    }

    void Update()
    {
        EnviromentView();
        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patrolling();
        }
    }

    private void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer)
        {
            Move(speedRun);

            anim.SetBool("isRunning", true);
            anim.SetBool("isWalking", false);

            NavMeshAgent.SetDestination(m_PlayerPosition);
        }
        if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
        {
            if (m_WaitTime <=0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);

                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", true);

                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                NavMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >=2.5f)
                {
                    Stop();
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isWalking", false);
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void Patrolling()
    {
        if (m_PlayerNear)
        {
            if (m_TimeToRotate <=0)
            {
                Move(speedWalk);
                
                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", true);

                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();

                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", false);

                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            NavMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
            {
                if (m_WaitTime<=0)
                {
                    NextPoint();
                    Move(speedWalk);

                    anim.SetBool("isRunning", false);
                    anim.SetBool("isWalking", true);

                    m_WaitTime = startWaitTime;

                }
                else
                {
                    Stop();

                    anim.SetBool("isRunning", false);
                    anim.SetBool("isWalking", false);

                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void Move(float speed)
    {
        NavMeshAgent.isStopped = false;
        NavMeshAgent.speed = speed;
    }

    void Stop()
    {
        NavMeshAgent.isStopped = true;
        NavMeshAgent.speed = 0;
        //playIdleAnim
    }

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex+1)%waypoints.Length;
        NavMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;

    }

    void LookingPlayer(Vector3 player)
    {
        NavMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <=0)
            {
                m_PlayerNear = false;
                Move(speedWalk);

                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", true);

                NavMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else 
            {
                Stop();

                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", false);

                m_WaitTime -= Time.deltaTime;
            }
        }
    }


    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position,viewRadius, playerMask);

        for (int i = 0; i<playerInRange.Length;i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle/2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;

                }
                else
                {
                    m_PlayerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                m_PlayerInRange = false;
            }

        
            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
    }
}
