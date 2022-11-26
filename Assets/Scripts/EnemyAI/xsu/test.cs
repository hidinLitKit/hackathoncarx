using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class test : MonoBehaviour
{
    State currentState;
    GameObject player;
    public NavMeshAgent agent;
    public float viewRadius;
    public float viewAngle;
    public float attackDist;
    public float timeToAct;
    public float speedWalk;
    public float speedRun;
    public float timeToUnAct;
    public Rigidbody rb;
    public string state;
    public Vector3 defaultPos;
    public float patrolRadius;

    public List<Transform> appearPoints;
    public List<Transform> patrolPoints;

    private SkinnedMeshRenderer skin;

    public GameObject monsterSkinObj;
    public Animator animator;

    public GameObject ScreamerC;
    public GameObject hunt;
    public GameObject patrol;
    public GameObject attack;
    private static ScreamerConroller scrc;
    private static AudioSource Ahunt;
    private static AudioSource Apatrol;
    private static AudioSource Aattack;

    void Start()
    {
        scrc = ScreamerC.GetComponent<ScreamerConroller>();
        Apatrol = patrol.GetComponent<AudioSource>();
        Ahunt = hunt.GetComponent<AudioSource>();
        Aattack = attack.GetComponent<AudioSource>();
        skin = monsterSkinObj.GetComponent<SkinnedMeshRenderer>();
        //animator = monsterSkinObj.GetComponent<Animator>();

        if (appearPoints.Count == 0)
            Debug.Log("No appear points");

        player = GameObject.FindGameObjectWithTag("Player");
        currentState = new Patrol(this);

    }

    void Update()
    {
        currentState = currentState.Process();
        state = currentState.name.ToString();
    }

    public class State
    {
        public enum STATE
        {
            PATROL, CHASE, ATTACK, MOVELESS
        }
        public enum EVENT
        {
            ENTER, UPDATE, EXIT
        }


        public STATE name;
        protected EVENT stage;
        protected State nextState;
        protected GameObject player;
        protected GameObject enemy;
        protected NavMeshAgent agent;
        protected float viewRadius;
        protected float viewAngle;
        protected float attackDist;
        protected float timeToAct;
        protected float speedWalk;
        protected float speedRun;
        protected float timeToUnAct;
        protected Rigidbody rb;
        protected test enemySc;
        protected Vector3 defaultPos;
        protected float patrolRadius;

        protected SkinnedMeshRenderer skin;
        protected LayerMask UnactiveLayer = LayerMask.NameToLayer("UnactiveLayer");
        protected LayerMask ActiveLayer = LayerMask.NameToLayer("Default");
        protected LayerMask obstacleMask = LayerMask.NameToLayer("Obstacle");
        protected LayerMask playerMask = LayerMask.NameToLayer("Player");
        protected int layerMask = 1 <<11;

        
        protected List<Transform> appearPoints;
        protected List<Transform> patrolPoints;
        protected Animator animator;

        public State(test enemyController)
        {
            enemySc = enemyController;
            stage = EVENT.ENTER;
            player = enemyController.player;
            agent = enemyController.agent;
            rb = enemyController.GetComponent<Rigidbody>();
            timeToAct = enemyController.timeToAct;
            viewRadius = enemyController.viewRadius;
            viewAngle = enemyController.viewAngle;
            timeToUnAct = enemyController.timeToUnAct;
            appearPoints = enemyController.appearPoints;
            enemy = enemyController.transform.gameObject;
            speedWalk = enemyController.speedWalk;
            speedRun = enemyController.speedRun;
            attackDist= enemyController.attackDist;
            defaultPos = enemyController.defaultPos;
            patrolPoints= enemyController.patrolPoints;
            patrolRadius= enemyController.patrolRadius;
            skin= enemyController.skin; 
            animator = enemyController.animator;
        }

        public virtual void Enter() { stage = EVENT.UPDATE; }
        public virtual void Update() { stage = EVENT.UPDATE; }
        public virtual void Exit() { stage = EVENT.EXIT; }

        public State Process()
        {
            if (stage == EVENT.ENTER) Enter();
            if (stage == EVENT.UPDATE) Update();
            if (stage == EVENT.EXIT)
            {
                Exit();
                return nextState;
            }
            return this;
        }

        
        public bool CanSeePlayer()
        {
            Vector3 direction = player.transform.position - enemy.transform.position;
            float angle = Vector3.Angle(enemy.transform.forward,direction.normalized);
            if (direction.magnitude < viewRadius && angle < viewAngle && !(Physics.Raycast(enemy.transform.position, direction.normalized, viewRadius, layerMask)))
            {
                 Debug.DrawRay(enemy.transform.position, direction.normalized, Color.yellow);
                Debug.Log("Did Hit");
                return true;
            }
            return false;


        //     Collider[] playerInRange = Physics.OverlapSphere(enemy.transform.position,viewRadius, playerMask);

        //     for (int i = 1; i<playerInRange.Length;i++)
        //     {
        //         Transform player = playerInRange[i].transform;
        //         Vector3 dirToPlayer = (player.position - enemy.transform.position).normalized;
        //         if (Vector3.Angle(enemy.transform.forward, dirToPlayer) < viewAngle/2)
        //         {
        //             float dstToPlayer = Vector3.Distance(enemy.transform.position, player.position);
        //             if (!Physics.Raycast(enemy.transform.position, dirToPlayer, dstToPlayer, obstacleMask))
        //             {
        //                 return true;
        //             }
        //             else
        //             {
        //                 return false;
        //             }
        //         }
        //         else
        //         {
        //             return false;
        //         }
        //     }
        //     return false;
         }

        public bool CanAttackPlayer()
        {
            Vector3 direction = player.transform.position - enemy.transform.position;
            if (direction.magnitude < attackDist)
            {
                return true;
            }
            return false;
        }

        public void AttackPlayer()
        {
            scrc.ScrEvent();
        }
        
    }

    public class Patrol : State
    {
        float timer;
        Vector3 lastPlayerPosition;
        bool getLastPlayerPosition = false;
        Transform nextDestination;
        int currentIndex = 1000;

        public Patrol(test enemyController) : base(enemyController)
        {
            name = STATE.PATROL;
            
        }

        public override void Enter()
        {
            Apatrol.Play();
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);

            timer = timeToAct;
            agent.speed = speedWalk;
            lastPlayerPosition = player.transform.position;

            int indexPoint = Random.Range(0,appearPoints.Count-1);

            enemy.transform.position = appearPoints[indexPoint].transform.position;
            agent.SetDestination(lastPlayerPosition);

            base.Enter();
        }


        public override void Update()
        {
            if (CanSeePlayer())
            {
                nextState = new Chase(enemySc, timer);
                stage = EVENT.EXIT;
            }
            else if(!getLastPlayerPosition)
            {
                if(Vector3.Distance(enemy.transform.position, lastPlayerPosition) < attackDist)
                {
                    nextDestination = GetNextDestination(ref currentIndex);
                    if (nextDestination != null)
                    {
                        agent.SetDestination(nextDestination.position);
                        getLastPlayerPosition = true;
                    }
                    else
                    {
                        Debug.Log("����� ��� ����� � ������� ����� �������������, ������� ���� � ������ ���� ��");
                        lastPlayerPosition = player.transform.position;
                        agent.SetDestination(lastPlayerPosition);
                    }
                }
            }
            else
            {
                    if (Vector3.Distance(enemy.transform.position, nextDestination.position) < attackDist)
                    {
                        nextDestination = GetNextDestination(ref currentIndex);

                        if (nextDestination != null)
                        {
                            agent.SetDestination(nextDestination.position);
                        }
                        else
                        {
                            Debug.Log("����� ��� ����� � ������� ����� �������������, ������� ���� � ������ ���� ��");
                            nextDestination = player.transform;
                            agent.SetDestination(nextDestination.position);
                        }
                    }

                
            }
            if(timer < 0)
            {
                nextState = new Moveless(enemySc);
                stage = EVENT.EXIT;
            }

            timer -= Time.deltaTime;
        }
        public override void Exit()
        {
            Apatrol.Stop();
            base.Exit();
        }

        public Transform GetNextDestination(ref int index)
        {
            List<Transform> availablePoints = new List<Transform>();
            Transform enemyPosition = enemy.transform;

            foreach (var point in patrolPoints)
            {
                if(Vector3.Distance(point.position, enemyPosition.position) < patrolRadius)
                    availablePoints.Add(point);
            }

            if (availablePoints.Count == 0)
                return null;

            var nextPointIndex = Random.Range(0, availablePoints.Count - 1);

            if (availablePoints.Count == 1 && nextPointIndex == index && index != 1000)
                return null;

            else if (availablePoints.Count > 1 && nextPointIndex == index)
            {
                nextPointIndex = Random.Range(0, availablePoints.Count - 1);
            }

            index = nextPointIndex;

            Debug.Log(nextPointIndex);

            var nextPoint = availablePoints[nextPointIndex];
            return nextPoint;
        }
    }

    public class Chase : State
    {
        float timer;

        public Chase(test enemyController, float timer) : base(enemyController)
        {
            name = STATE.CHASE;
            this.timer = timer;
        }

        public override void Enter()
        {
            Ahunt.Play();
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            agent.speed = speedRun;
            
            base.Enter();
        }


        public override void Update()
        {
            agent.SetDestination(player.transform.position);

            if(CanAttackPlayer())
            {
                nextState = new Attack(enemySc, timer);
                stage = EVENT.EXIT;
            }
            if(timer < 0)
            {
                nextState = new Moveless(enemySc);
                stage = EVENT.EXIT;
            }

            timer -= Time.deltaTime;
        }
        public override void Exit()
        {
            Ahunt.Stop();
            base.Exit();
        }
    }
    public class Attack : State
    {
        float timer;

        public Attack(test enemyController, float timer) : base(enemyController)
        {
            name = STATE.ATTACK;
            this.timer = timer;
        }

        public override void Enter()
        {
            Aattack.Play();
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);

            agent.speed = 0f;
            AttackPlayer();
            base.Enter();
        }


        public override void Update()
        {

        }
        public override void Exit()
        {
            Aattack.Stop();
            base.Exit();
        }
    }
    public class Moveless : State
    {
        float timer;
        public Moveless(test enemyController) : base(enemyController)
        {
            name = STATE.MOVELESS;
           
        }

        public override void Enter()
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);

            enemy.gameObject.layer = UnactiveLayer;
            skin.enabled = false;

            timer = timeToUnAct;
            agent.speed = 0f;
            //�������
            enemy.transform.position = defaultPos;
            base.Enter();
        }


        public override void Update()
        {
            if(timer < 0)
            {
                nextState = new Patrol(enemySc);
                stage = EVENT.EXIT;
            }
            timer -= Time.deltaTime;

        }
        public override void Exit()
        {
            enemy.gameObject.layer = ActiveLayer;
            skin.enabled = true;

            base.Exit();
        }
    }


}




