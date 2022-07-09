using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine
{
    public States currentState;
    protected Events Stage;
    protected StateMachine NextState;
    protected GameObject Enemy;
    protected NavMeshAgent Agent;
    protected Animator Anim;
    protected NavMeshPath Path;

    public StateMachine(GameObject enemy, NavMeshAgent agent, Animator anim, NavMeshPath path)
    {
        Stage = Events.ENTER;
        Enemy = enemy;
        Agent = agent;
        Anim = anim;
        Path = path;
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
}
