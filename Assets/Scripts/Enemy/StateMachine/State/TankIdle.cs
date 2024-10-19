using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static StateMachine;
public class TankIdle : IState<TankState>
{
    [SerializeField] private Transform _rotateTank;
    [SerializeField] private Transform _directionDefault;
    
    private bool _isNextState;
    private float _speedRotate = 180f;
    public override void EnterState()
    {
        _navMeshAgent.enabled = false;
        _isNextState = false;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (!RotaToTargetReturn(_rotateTank, _directionDefault.position, _speedRotate, Vector3.up, 3f)) return;
        _isNextState = true;
    }

    public override TankState GetNextState()
    {
        if(_isNextState)
            return TankState.Move;
        return currentState;
    }
    public bool RotaToTargetReturn(Transform myTrans, Vector3 targetPos, float rotaSpeed, Vector3 UpWards,
        float CheckAngle = 3)
    {
        var dir = targetPos - myTrans.position; //a vector pointing from pointA to pointB
        var rot = Quaternion.LookRotation(dir, UpWards); //calc a rotation that
        myTrans.rotation = Quaternion.RotateTowards(myTrans.rotation, rot, rotaSpeed * Time.deltaTime);
        return Quaternion.Angle(myTrans.rotation, rot) < CheckAngle;
    }

}
