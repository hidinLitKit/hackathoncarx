using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyControllerAi : MonoBehaviour
{
    public float RunSpeed;
    NavMeshAgent agent ;
    Animator anim;
    GameObject player;
    public float AttackDist;
    PlayerHealth playerHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        AttackPlayer();
    }


    void Update()
    {
        
        if( Vector3.Distance(transform.position, player.transform.position)>AttackDist)
        {
            if(agent.speed != RunSpeed)
                agent.speed = RunSpeed;
            agent.SetDestination(player.transform.position);

        }
        else
            AttackPlayer();
        
    }


    public void AttackPlayer()
    {
        agent.speed = 0;
        playerHealth.TakeDamage(1);
    }

}
