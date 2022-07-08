using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

public class ArcherAI : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private NavMeshAgent _agent;
    private Animator _anim;
    private StateMachine _currentState;
    private NavMeshPath _path;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _path = new NavMeshPath();
        _currentState = new ArcherSpawn(gameObject, _player, _agent, _anim, _path);
    }

    private void Update()
    {
        _currentState = _currentState.Process();
    }
}

public class ArcherSpawn : StateMachine
{
    public ArcherSpawn(GameObject enemy, Transform player, NavMeshAgent agent, Animator anim, NavMeshPath path)
                : base(enemy, player, agent, anim, path)
    {
        currentState = States.SPAWNING;
        Debug.Log("SPAWN");
    }

    public override void Enter()
    {
        Debug.Log("ENTER SPAWN");
        Anim.SetTrigger("Spawn");
        SpawnDelay();
        base.Enter();
    }

    public override void Exit()
    {
        Debug.Log("EXIT SPAWN");
        base.Exit();
    }

    private async UniTask SpawnDelay()
    {
        await UniTask.Delay(1200);
        NextState = new ArcherMoving(Enemy, Player, Agent, Anim, Path);
        Stage = Events.EXIT;
    }
}

public class ArcherIdle : StateMachine
{
    public ArcherIdle(GameObject enemy, Transform player, NavMeshAgent agent, Animator anim, NavMeshPath path)
                : base(enemy, player, agent, anim, path)
    {
        currentState = States.IDLE;
        Debug.Log("IDLE");
    }

    public override void Enter()
    {
        Debug.Log("ENTER IDLE");
        Anim.SetTrigger("Idle");
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("UPDATE IDLE");
    }

    public override void Exit()
    {
        Debug.Log("EXIT IDLE");
        base.Exit();
    }
}

public class ArcherMoving : StateMachine
{
    private Transform _waypoint;

    public ArcherMoving(GameObject enemy, Transform player, NavMeshAgent agent, Animator anim, NavMeshPath path) : base(enemy, player, agent, anim, path)
    {
        currentState = States.MOVING;
        Debug.Log("Moving");
    }

    public override void Enter()
    {
        Debug.Log("ENTER Moving");
        _waypoint = GetAvailableWaypoint();
        Anim.SetTrigger("Run");
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("UPDATE Moving");
        Move();
    }

    public override void Exit()
    {
        Debug.Log("EXIT Moving");
        base.Exit();
    }

    private void Move()
    {
        Agent.SetDestination(_waypoint.position);

        if(Agent.remainingDistance < Agent.stoppingDistance && Agent.remainingDistance != 0)
        {
            NextState = new ArcherIdle(Enemy, Player, Agent, Anim, Path);
            Stage = Events.EXIT;
        }
    }

    private Transform GetAvailableWaypoint()
    {
        Transform closestWaypoint = null;
        List<Transform> archerWaypoints = WaypointController.sInstance.ArcherWaypoint;
        float currentWaypointDistance;
        float closestWaypointDistance = 0;

        for (int i = 0; i < archerWaypoints.Count; i++)
        {
            currentWaypointDistance = Vector3.Distance(archerWaypoints[i].transform.position, Enemy.transform.position);

            bool isPathAvailable = Agent.CalculatePath(archerWaypoints[i].transform.position, Path);

            if (closestWaypointDistance == 0)
            {
                if (isPathAvailable)
                {
                    closestWaypoint = archerWaypoints[i].transform;
                    closestWaypointDistance = currentWaypointDistance;
                }
            }
            else if (currentWaypointDistance < closestWaypointDistance)
            {
                if (isPathAvailable)
                {
                    closestWaypoint = archerWaypoints[i].transform;
                    closestWaypointDistance = currentWaypointDistance;
                }
            }
        }

        return closestWaypoint;
    }
}

