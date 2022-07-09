using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

public class ArcherAI : MonoBehaviour
{
    [SerializeField] private float _viewDistance;
    [SerializeField] private float _viewAngle;

    private NavMeshAgent _agent;
    private Animator _anim;
    private StateMachine _currentState;
    private NavMeshPath _path;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _path = new NavMeshPath();
        _currentState = new ArcherSpawn(gameObject, _agent, _anim, _path);
    }

    private void Update()
    {
        _currentState = _currentState.Process();
    }
}

public class ArcherSpawn : StateMachine
{
    public ArcherSpawn(GameObject enemy, NavMeshAgent agent, Animator anim, NavMeshPath path)
                : base(enemy, agent, anim, path)
    {
        currentState = States.SPAWNING;
    }

    public override void Enter()
    {
        Anim.SetTrigger("Spawn");
        SpawnDelay();
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private async UniTask SpawnDelay()
    {
        await UniTask.Delay(1000);
        NextState = new ArcherMoving(Enemy, Agent, Anim, Path);
        Stage = Events.EXIT;
    }
}

public class ArcherIdle : StateMachine
{
    public ArcherIdle(GameObject enemy, NavMeshAgent agent, Animator anim, NavMeshPath path)
                : base(enemy, agent, anim, path)
    {
        currentState = States.IDLE;
    }

    public override void Enter()
    {
        Anim.SetTrigger("Idle");
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        SearchPlayer();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void SearchPlayer()
    {
        Vector3 position = new Vector3(Enemy.transform.position.x, 1, Enemy.transform.position.z);

        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;

        int layerMask = 1 << 6;

        Vector3 forwardRay = Enemy.transform.TransformDirection(Vector3.forward);
        Vector3 rightRay = Quaternion.AngleAxis(30, Enemy.transform.up) * Enemy.transform.forward;
        Vector3 leftRay = Quaternion.AngleAxis(-30, Enemy.transform.up) * Enemy.transform.forward;
        Debug.DrawRay(position, forwardRay * 10, Color.green);
        Debug.DrawRay(position, rightRay * 10, Color.green);
        Debug.DrawRay(position, leftRay * 10, Color.green);

        bool ray1 = Physics.Raycast(position, forwardRay, out hit1, 10, layerMask);
        bool ray2 = Physics.Raycast(position, rightRay, out hit2, 10, layerMask);
        bool ray3 = Physics.Raycast(position, leftRay, out hit3, 10, layerMask);

        if(ray1)
        {
            FindPlayer(hit1.transform.gameObject.transform);
        }
        else if(ray2)
        {
            FindPlayer(hit2.transform.gameObject.transform);
        }
        else if(ray3)
        {
            FindPlayer(hit3.transform.gameObject.transform);
        }


    }

    private void FindPlayer(Transform player)
    {
        NextState = new ArcherAttack(Enemy, Agent, Anim, Path, player);
        Stage = Events.EXIT;
    }
}

public class ArcherAttack : StateMachine
{
    private int _targetLayer = 1 << 6;
    private Vector3 _rayPosition;
    private Transform _player;

    public ArcherAttack(GameObject enemy, NavMeshAgent agent, Animator anim, NavMeshPath path, Transform player)
                : base(enemy, agent, anim, path)
    {
        currentState = States.ATTACK;
        _player = player;
    }

    public override void Enter()
    {
        base.Enter();
        RaycastHit hit;
        Vector3 forwardRay = Enemy.transform.TransformDirection(Vector3.forward);
        _rayPosition = new Vector3(Enemy.transform.position.x, 1, Enemy.transform.position.z);
        bool ray = Physics.Raycast(_rayPosition, forwardRay, out hit, 10, _targetLayer);
    }

    public override void Update()
    {
        base.Update();
        SearchPlayer();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void SearchPlayer()
    {
        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;

        Vector3 forwardRay = Enemy.transform.TransformDirection(Vector3.forward);
        Vector3 rightRay = Quaternion.AngleAxis(30, Enemy.transform.up) * Enemy.transform.forward;
        Vector3 leftRay = Quaternion.AngleAxis(-30, Enemy.transform.up) * Enemy.transform.forward;
        Debug.DrawRay(_rayPosition, forwardRay * 10, Color.green);
        Debug.DrawRay(_rayPosition, rightRay * 10, Color.green);
        Debug.DrawRay(_rayPosition, leftRay * 10, Color.green);

        bool ray1 = Physics.Raycast(_rayPosition, forwardRay, out hit1, 10, _targetLayer);
        bool ray2 = Physics.Raycast(_rayPosition, rightRay, out hit2, 10, _targetLayer);
        bool ray3 = Physics.Raycast(_rayPosition, leftRay, out hit3, 10, _targetLayer);

        if(!ray1 && !ray2 && !ray3)
        {
            LostPlayer();
        }
        else
        {
            Enemy.transform.LookAt(_player);
        }
    }

    private void LostPlayer()
    {
        NextState = new ArcherIdle(Enemy, Agent, Anim, Path);
        Stage = Events.EXIT;
    }
}

public class ArcherMoving : StateMachine
{
    private Transform _waypoint;

    public ArcherMoving(GameObject enemy, NavMeshAgent agent, Animator anim, NavMeshPath path) : base(enemy, agent, anim, path)
    {
        currentState = States.MOVING;
    }

    public override void Enter()
    {
        _waypoint = GetAvailableWaypoint();
        Anim.SetTrigger("Run");
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        Move();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void Move()
    {
        Agent.SetDestination(_waypoint.position);

        if(Agent.remainingDistance < Agent.stoppingDistance && Agent.remainingDistance != 0)
        {
            NextState = new ArcherIdle(Enemy, Agent, Anim, Path);
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

