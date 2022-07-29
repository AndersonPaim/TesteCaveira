using UnityEngine;
using UnityEngine.AI;
using Enemy;
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

    protected StateMachine(GameObject enemy, GameObject player, NavMeshAgent agent, SkinnedMeshRenderer mesh, Animator anim, NavMeshPath path, EnemyBalancer balancer, GameManager manager)
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

    protected virtual void Enter()
    {
        Stage = Events.UPDATE;
    }

    protected virtual void Update()
    {
        Stage = Events.UPDATE;
    }

    protected virtual void Exit()
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
