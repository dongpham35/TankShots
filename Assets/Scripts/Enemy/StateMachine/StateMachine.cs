using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class StateMachine : NetworkBehaviour
{
    [SerializeField] private TankIdle _tankIdle;
    [SerializeField] private TankMove _tankMove;
    [SerializeField] private TankAttack _tankAttack;
    [SerializeField] private EnemyInformation _data;
    
    private Dictionary<TankState, IState<TankState>> _statemachine = new Dictionary<TankState, IState<TankState>>();
    public enum TankState
    {
        Idle,
        Move,
        Attack
    }
    
    private IState<TankState> _currentTankState ;
    
    private bool isTransition = false;
    
    private new void OnValidate()
    {
        _tankAttack = GetComponent<TankAttack>();
        _tankMove = GetComponent<TankMove>();
        _tankIdle = GetComponent<TankIdle>();
    }
    private void Awake()
    {
        Initialization();
    }


    private void OnEnable()
    {
        _currentTankState = _statemachine[TankState.Idle];
        
    }
    void Initialization()
    {
        _tankAttack.Initialize(TankState.Attack);
        _tankIdle.Initialize(TankState.Idle);
        _tankMove.Initialize(TankState.Move);
        
        _statemachine.Add(TankState.Attack, _tankAttack);
        _statemachine.Add(TankState.Move, _tankMove);
        _statemachine.Add(TankState.Idle, _tankIdle);
        
    }
    
    private void Update()
    {
        TankState nextState = _currentTankState.GetNextState();
        if (_currentTankState.currentState.Equals(nextState) && !isTransition)
        {
            _currentTankState.UpdateState();
        }
        else
        {
            Transition(nextState);
        }
    }

    private void Transition(TankState nextState)
    {
        isTransition = true;
        _currentTankState.ExitState();
        _currentTankState = _statemachine[nextState];
        _currentTankState.EnterState();
        isTransition = false;
    }
    
}
