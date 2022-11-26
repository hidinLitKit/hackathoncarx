using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyFSM : MonoBehaviour
{

    [SerializeField] float timeToAct;
    [SerializeField] float timeToUnAct;
    [SerializeField] float timeToAttack;
    [SerializeField] float timeToPatrol;
    [SerializeField] AudioSource audio_PrerareTimeToAttack;

    public GameObject[] rooms;

    public NavMeshAgent agent;
    //public float startWaitTime = 4;
    //public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public float attackDist = 7;

    public LayerMask playerMask;
    public LayerMask obstacleMask;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    private Animator anim;

    State currentState;
    public string state;

    Renderer renderer;

    GameObject player;
    //public List<GameObject> waypoints = new List<GameObject>();

    void Start()
    {
        renderer = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<NavMeshAgent>();
        currentState = new UnActive(this);


    }


    void Update()
    {
        currentState = currentState.Process();
        state = currentState.name.ToString();
    }


    public void AttackPlayer()
    {
        
    }

    public GameObject WhereIsAPlayer()
    {
        Collider roomCollider;
        foreach(GameObject room in rooms)
        {
            roomCollider = room.GetComponent<Collider>();
            if (roomCollider.CompareTag("Player"))
                return room; 
        }
        return null;
    }
    public void Attack()
    {

    }

    public class State
    {
        public enum STATE
        {
            UNACTIVE, PREPARATION, ACTIVE, ATTACK, KILL, PATROL
        }
        public enum EVENT
        {
            ENTER, UPDATE, EXIT
        }

        public STATE name;
        protected EVENT stage;
        protected GameObject enemy;
        protected State nextState;
        protected Rigidbody2D rb;
        protected NavMeshAgent agent;
        protected float timer;
        protected GameObject player;
        protected float timeToAct;
        protected float timeToUnAct;
        protected float timeToAttack;
        protected EnemyFSM enemyFsm;
        protected int UnactiveLayer;
        protected int DefaultLayer;
        protected float viewRadius;
        protected float viewAngle;
        protected float attackDist;
        protected float timeToPatrol;
        protected float speedWalk;
        protected float speedRun;

        public State(EnemyFSM enemyFsm)
        {
            stage = EVENT.ENTER;
            agent = enemyFsm.agent;
            enemy = enemyFsm.gameObject;
            rb = enemy.GetComponent<Rigidbody2D>();
            timeToAct = enemyFsm.timeToAct;
            timeToUnAct = enemyFsm.timeToUnAct;
            timeToAttack = enemyFsm.timeToAttack;
            UnactiveLayer = LayerMask.NameToLayer("UnactiveLayer");
            DefaultLayer = LayerMask.NameToLayer("Default");
            player = enemyFsm.player;
            viewRadius = enemyFsm.viewRadius;
            viewAngle = enemyFsm.viewAngle; 
            attackDist = enemyFsm.attackDist;
            timeToPatrol = enemyFsm.timeToPatrol;
            speedWalk = enemyFsm.speedWalk; 
            speedRun = enemyFsm.speedRun;

            agent = enemyFsm.agent;

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
            float angle = Vector3.Angle(direction, enemy.transform.forward);
            if (direction.magnitude < viewRadius && angle < viewAngle)
            { 
                return true;
            }
            return false;
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
    }

    public class UnActive : State
    {

        public UnActive(EnemyFSM enemyFsm):base(enemyFsm)
        {
            name = STATE.UNACTIVE;
            agent.speed = 0f;
        }

        public override void Enter()
        {
            base.Enter();
            timer = timeToAct;

            //Set enemy invisible and uninteractive with player collider, without setActive(false)
            enemyFsm.renderer.enabled = false;
            enemyFsm.gameObject.layer = UnactiveLayer;
        }

        public override void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                nextState = new Preparation(enemyFsm);
                stage = EVENT.EXIT;
            }
        }

        public override void Exit()
        {
            //Activate enemy
            enemyFsm.gameObject.layer = DefaultLayer;
            enemyFsm.renderer.enabled = true;
            base.Exit();
        }
    }

    public class Active : State
    {
        public Active(EnemyFSM enemyFsm):base(enemyFsm)
        {
            name = STATE.ACTIVE;
        }

        public override void Update()
        {


            if (CanAttackPlayer())
            {
                nextState = new Active(enemyFsm);
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState = new Preparation(enemyFsm);
                stage = EVENT.EXIT;
            }
            else if (player.transform.position.x > enemy.transform.position.x)
            {
                rb.velocity = new Vector2(5f, 0);
                enemy.transform.localScale = new Vector2(15, 15);
            }
            else
            {
                rb.velocity = new Vector2(-5f, 0);
                enemy.transform.localScale = new Vector2(-15, 15);
            }
        }

        public override void Enter()
        {
            if(UnityEngine.Random.Range(0,1)==1)
            {
                nextState = new Chase(enemyFsm);
                stage = EVENT.EXIT;
            }
            base.Enter();
        }
        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Preparation : State
    {

        public Preparation(EnemyFSM enemyFsm) : base(enemyFsm)
        {
            name = STATE.PREPARATION;
            timer = timeToAttack;
        }

        public override void Update()
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                nextState = new Patrol(enemyFsm);
                stage = EVENT.EXIT;
            }
        }
        public override void Enter()
        {
            enemyFsm.audio_PrerareTimeToAttack.Play(0);
            base.Enter();
        }
        public override void Exit()
        {
            enemyFsm.audio_PrerareTimeToAttack.Stop();
            base.Exit();
        }
    }

    public class Chase : State
    {
        public Chase(EnemyFSM enemyFsm) : base(enemyFsm)
        {
            name = STATE.ATTACK;
            timer = 0f;
        }

        public override void Update()
        {

            if(!CanSeePlayer())
            {
                //хз
            }
            else
            {
                if(CanAttackPlayer())
                {
                    enemyFsm.Attack();
                }
            }
            
        }

        public override void Enter()
        {
            agent.speed = speedRun;
            base.Enter();
        }
        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Kill : State
    {
        public Kill(EnemyFSM enemyFsm) : base(enemyFsm)
        {
            name = STATE.KILL;
            timer = 0f;
        }

        public override void Update()
        {

            rb.velocity = new Vector2(0, 0);
            if (timer <= 0)
            {
                enemy.GetComponent<EnemyFSM>().AttackPlayer();
                timer = 1.5f;
            }
            else timer -= Time.deltaTime;

            if (CanSeePlayer())
            {
                if (!CanAttackPlayer())
                {
                    nextState = new Active(enemyFsm);
                    stage = EVENT.EXIT;
                }

            }
            if ((player.transform.position - enemy.transform.position).magnitude < 18 && !CanSeePlayer())
            {

                int sc = 15;
                if (player.transform.position.x - enemy.transform.position.x <= 0)
                    sc = -15;

                enemy.transform.localScale = new Vector2(sc, 15);


                nextState = new UnActive(enemyFsm);
                stage = EVENT.EXIT;
            }

        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Patrol : State
    {
        bool startedPatrol = false;
        GameObject currentRoom;
        int wayPointCount;
        Transform[] WayPoints;

        public Patrol(EnemyFSM enemyFsm) : base(enemyFsm)
        {
            name = STATE.PATROL;
        }

        public override void Enter()
        {
            timer = timeToPatrol;
            agent.speed = speedWalk;
            base.Enter();
        }
        

        public override void Update()
        {
            if(!startedPatrol)
            { 
                if (enemyFsm.WhereIsAPlayer())
                { 
                    startedPatrol = true;
                    currentRoom = enemyFsm.WhereIsAPlayer();
                    wayPointCount = currentRoom.gameObject.transform.childCount;

                    for (int i = 0; i <= wayPointCount-1; i++)
                    {
                        WayPoints[i] = currentRoom.gameObject.transform.GetChild(i);
                    }
                    enemyFsm.gameObject.transform.position = WayPoints[ UnityEngine.Random.Range(0, wayPointCount - 1)].position;
                }
            }
            else
            {
                if(!CanSeePlayer())
                {
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        nextState = new Active(enemyFsm);
                        stage = EVENT.EXIT;
                    }
                    else
                    {
                        //продолжаем шагать по комнате

                    }
                }
                else
                {
                    nextState = new Chase(enemyFsm);
                    stage = EVENT.EXIT;
                }
            }

        }
        public override void Exit()
        {
            base.Exit();
        }
    }

}
