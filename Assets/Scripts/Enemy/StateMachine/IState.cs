using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public abstract class IState<State> : NetworkBehaviour where State : Enum
{
    [SerializeField]
    protected NavMeshAgent _navMeshAgent;
    
    public State currentState{get; private set;}

    public void Initialize(State newState)
    {
        currentState = newState;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract State GetNextState();

}
