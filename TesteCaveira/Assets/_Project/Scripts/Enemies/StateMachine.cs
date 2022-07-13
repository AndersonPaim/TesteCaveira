using UnityEngine;
using UnityEngine.AI;
using Enemy;
using Enemy.Archer;
using Enemy.Melee;

public class StateMachine
{
    public States CurrentState;
    protected States LastState;
    protected Events Stage;
    protected StateMachine NextState;
    protected GameObject Enemy;
    protected GameObject Player;
    protected NavMeshAgent Agent;
    protected Animator Anim;
    protected NavMeshPath Path;
    protected EnemyBalancer Balancer;

    public StateMachine(GameObject enemy, GameObject player, NavMeshAgent agent, Animator anim, NavMeshPath path, EnemyBalancer balancer)
    {
        Stage = Events.ENTER;
        Enemy = enemy;
        Agent = agent;
        Anim = anim;
        Path = path;
        Player = player;
        Balancer = balancer;
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
        LastState = CurrentState;
        Debug.Log("LAST STATE: " + LastState);
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

    public void MeleeDamage()
    {
        NextState = new MeleeDamage(Enemy, Player, Agent, Anim, Path, Balancer);
        Stage = Events.EXIT;
    }

    public void MeleeDeath()
    {
        NextState = new MeleeDying(Enemy, Player, Agent, Anim, Path, Balancer);
        Stage = Events.EXIT;
    }

    public void ArcherDamage()
    {
        NextState = new ArcherDamage(Enemy, Player, Agent, Anim, Path, Balancer);
        Stage = Events.EXIT;
    }

    public void ArcherDeath()
    {
        NextState = new ArcherDying(Enemy, Player, Agent, Anim, Path, Balancer);
        Stage = Events.EXIT;
    }
}
