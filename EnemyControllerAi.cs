using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.Events;

public class EnemyControllerAi : MonoBehaviour
{
    public List<GameObject> appearPoints;


    public class EnemyControllerAi : MonoBehaviour
    {
        public float RunSpeed;
        NavMeshAgent agent;
        Animator anim;
        GameObject player;
        public float AttackDist;
        PlayerHealth playerHealth;
        float timer;
        public float TimeToRest;
        public bool IsDead = false;
        Collider collider;
        Renderer rendere;
        [SerializeField] float appearDist;

        [SerializeField] int maxHealth;
        int health;
        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                if (health <= 0)
                    Die();
            }
        }


        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerHealth = player.GetComponent<PlayerHealth>();
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            collider = GetComponent<Collider>();
            rendere = GetComponent<Renderer>();
            Health = maxHealth;
        }

        void Update()
        {
            if (IsDead)
            {
                if (timer > 0)
                    timer -= Time.deltaTime;
                else
                {
                    GetAlive();
                }
                return;
            }

            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            AttackPlayer();
        }


        void Update()
        {

            if (Vector3.Distance(transform.position, player.transform.position) > AttackDist)
            {
                if (agent.speed != RunSpeed)
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
            rendere.enabled = false;

        }
        public void GetAlive()
        {


            List<GameObject> correctPoints = new List<GameObject>();
            foreach (var point in appearPoints)
            {
                if (Vector3.Distance(player.transform.position, point.transform.position) <= appearDist)
                {
                    correctPoints.Add(point);
                }
            }

            int index = Random.Range(0, appearPoints.Count - 1);
            transform.position = correctPoints[index].transform.position;

            IsDead = false;
            Health = maxHealth;
            collider.enabled = true;
            rendere.enabled = true;
        }

        public void AttackPlayer()
        {
            agent.speed = 0;
            playerHealth.TakeDamage(1);
        }
    }
}
