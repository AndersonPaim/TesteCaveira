using UnityEngine;
using UnityEngine.AI;
using Enemy;
using Enemy.Archer;
using Enemy.Melee;
using Managers;

public class StateMachine
{
    public States CurrentState;
    public Events Stage;
    public StateMachine StateMachineNextState;
    protected GameObject Enemy;
    protected GameObject Player;
    protected NavMeshAgent Agent;
    protected SkinnedMeshRenderer Mesh;
    protected Animator Anim;
    protected NavMeshPath Path;
    protected EnemyBalancer Balancer;
    protected GameManager Manager;

    public StateMachine(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
    {
        Stage = Events.ENTER;
        Enemy = enemy;
        Agent = agent;
        Anim = anim;
        Path = path;
        Player = player;
        Balancer = balancer;
        Manager = manager;
        Mesh = mesh;
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
            return StateMachineNextState;
        }

        return this;
    }
}
