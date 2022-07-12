using UnityEngine;
using UnityEngine.AI;
using Enemy.Archer;

public class StateMachine
{
    public States currentState;
    protected Events Stage;
    protected StateMachine NextState;
    protected GameObject Enemy;
    protected GameObject Player;
    protected NavMeshAgent Agent;
    protected Animator Anim;
    protected NavMeshPath Path;

    public StateMachine(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path)
    {
        Stage = Events.ENTER;
        Enemy = enemy;
        Agent = agent;
        Anim = anim;
        Path = path;
        Player = player;
    }

    public virtual void Enter()
    {
        Stage = Events.UPDATE;
    }

    public virtual void Update()
    {
        Stage = Events.UPDATE;
    }

    public virtual void Exit()
    {
        Stage = Events.EXIT;
    }

    public StateMachine Process()
    {
        if(Stage == Events.ENTER)
        {
            Enter();
        }
        if(Stage == Events.UPDATE)
        {
            Update();
        }
        if(Stage == Events.EXIT)
        {
            Exit();
            return NextState;
        }

        return this;
    }

    public bool CanSeePlayer(float distance, float angle)
    {
        Vector3 direction = Player.transform.position - Enemy.transform.position;
        float viewAngle = Vector3.Angle(direction, Enemy.transform.forward);

        if(direction.magnitude < distance && viewAngle < angle)
        {
            return true;
        }

        return false;
    }

    public void Death()
    {
        NextState = new ArcherDying(Enemy, Player, Agent, Anim, Path);
        Stage = Events.EXIT;
    }
}
