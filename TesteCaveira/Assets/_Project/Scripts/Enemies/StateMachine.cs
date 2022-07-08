using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public States currentState;
    protected Events _stage;
    protected StateMachine _nextState;

    public StateMachine()
    {
        _stage = Events.ENTER;
    }

    public virtual void Enter()
    {
        _stage = Events.UPDATE;
    }

    public virtual void Update()
    {
        _stage = Events.UPDATE;
    }

    public virtual void Exit()
    {
        _stage = Events.EXIT;
    }

    public StateMachine Process()
    {
        if(_stage == Events.ENTER)
        {
            Enter();
        }
        if(_stage == Events.UPDATE)
        {
            Update();
        }
        if(_stage == Events.EXIT)
        {
            Exit();
            return _nextState;
        }

        return this;
    }
}

public class Idle : StateMachine
{
    public Idle()
    {
        currentState = States.IDLE;
        Debug.Log("IDLE");
    }

    public override void Enter()
    {
        Debug.Log("ENTER IDLE");
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("UPDATE IDLE");
        _nextState = new Moving();
        _stage = Events.EXIT;
    }

    public override void Exit()
    {
        Debug.Log("EXIT IDLE");
        base.Exit();
    }
}

public class Moving : StateMachine
{
    public Moving()
    {
        currentState = States.MOVING;
        Debug.Log("Moving");
    }

    public override void Enter()
    {
        Debug.Log("ENTER Moving");
        base.Enter();
    }

    public override void Update()
    {
        Debug.Log("UPDATE Moving");
        base.Enter();
    }

    public override void Exit()
    {
        Debug.Log("EXIT Moving");
        base.Enter();
    }
}