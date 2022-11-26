using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerAi : MonoBehaviour
{
    List<GameObject> appearPoints;
    public float RunSpeed;
    NavMeshAgent agent ;
    Animator anim;
    GameObject player;
    public float AttackDist;
    PlayerHealth playerHealth;
    float timer;
    public float TimeToRest;
    public bool IsDead = false;
    Collider collider;
    Renderer rendere;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();
        rendere = GetComponent<Renderer>();
    }

    void Update()
    {
        if(IsDead)
        {
            if(timer > 0)
                timer -= Time.deltaTime;
            else 
            {
                GetAlive();
            }
            return;
        }

        if( Vector3.Distance(transform.position, player.transform.position)>AttackDist)
        {
            if(agent.speed != RunSpeed)
                agent.speed = RunSpeed;
            agent.SetDestination(player.transform.position);
        }
        else
            AttackPlayer();
        
    }

    public void Die()
    {
        timer = TimeToRest;
        IsDead = true;
        collider.enabled = false;
        rendere.enabled=false;
        
    }
    public void GetAlive()
    {
        IsDead = false;
        
        collider.enabled = true;
        rendere.enabled = true;
    }

    public void AttackPlayer()
    {
        agent.speed = 0;
        playerHealth.TakeDamage(1);
    }
}
