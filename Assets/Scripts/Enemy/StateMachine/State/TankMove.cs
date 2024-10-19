using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static StateMachine;
using Mirror;
public class TankMove : IState<TankState>
{
    [SerializeField] private Transform _targetPos;
    [SerializeField] private EnemyInformation data;

    private Vector3 _destination;
    private bool _isNextState;
    private bool _isStart;
    
   public List<GameObject> li_players = new List<GameObject>();

    public override void EnterState()
    {
        SetTargetPlayer();
        _isStart = false;
        _isNextState = false;
        _destination = GetPosAroundTarget(_targetPos, Random.Range(-180, 180), 10f, 0);
        _navMeshAgent.enabled = true;
        _navMeshAgent.speed = data.speed;
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.SetDestination(_destination);
        StartCoroutine(WaitMove());
    }

    [ClientRpc]
    public void SetAllPlayer(List<GameObject> players)
    {
        li_players = players;
    }
    private void SetTargetPlayer()
    {
        if (li_players.Count >= 1)
        {
            int randomPlayer = Random.Range(0, li_players.Count - 1);
            _targetPos = li_players[randomPlayer].transform;
        }
    }

    IEnumerator WaitMove()
    {
        yield return new WaitUntil(() => _navMeshAgent.hasPath);
        _navMeshAgent.updateRotation = true;
        _navMeshAgent.updatePosition = true;
        _isStart = true;
    }

    public override void ExitState()
    {
        _navMeshAgent.enabled = false;
    }

    public override void UpdateState()
    {
        if (_targetPos == null)
        {
            SetTargetPlayer();
            return;
        }
        if (!_isStart) return;
        float distance = Vector3.Distance(transform.position, _destination);
        if(distance < _navMeshAgent.stoppingDistance)
            _isNextState = true;
    }

    public override TankState GetNextState()
    {
        if(_isNextState)
            return TankState.Attack;
        return currentState;
    }
    
    public Vector3 GetPosAroundTarget(Transform target, float angle, float distance, float high)
    {
        float radians = ConvertAngle180(target.root.localEulerAngles.y + angle) * Mathf.Deg2Rad;
        var x = Mathf.Sin(radians);
        var y = Mathf.Cos(radians);
        return target.position + new Vector3(x * distance, high, y * distance);
    }
    
    public float ConvertAngle180(float angleInput)
    {
        return -Mathf.DeltaAngle(angleInput, 0);
    }
    
    /*
    public List<GameObject> GetAllPlayers()
    {
        List<GameObject> players = new List<GameObject>();

        // Loop through all active connections on the server
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            // Check if the connection has an associated player object
            if (conn.identity != null)
            {
                // Add the player GameObject to the list
                players.Add(conn.identity.gameObject);
            }
        }

        return players;
    }*/
}
