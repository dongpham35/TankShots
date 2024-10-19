using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Tanks;
using UnityEngine;
using static StateMachine;
public class TankAttack : IState<TankState>
{
    [SerializeField] private Transform _targetPlayer;
    [SerializeField] private Transform _rotateTank;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private EnemyInformation data;
    private float _rotateSpeed = 180f;
    private bool _isNextState;
    private float _timerNextState;
    private bool _isFire;

    public List<GameObject> li_players = new List<GameObject>();
    
    
    public override void EnterState()
    {
        SetTargetPlayer();
        _timerNextState = 1f;
        _isFire = false;

        
        _navMeshAgent.enabled = false;
        _isNextState = false;
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
            _targetPlayer = li_players[randomPlayer].transform;
        }
    }
    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        if (_targetPlayer == null)
        {
            SetTargetPlayer();
            return;
        }
        if (!RotaToTargetReturn(_rotateTank, _targetPlayer.position, _rotateSpeed, Vector3.up, 3)) return;
        //Finish rotate
        //Attack
        if (_isFire)
        {
            _timerNextState -= Time.deltaTime;
            if (_timerNextState < 0) _isNextState = true;
            return;
        }
        CmdAttack();
    }
    
    
    [Server]
    private void CmdAttack()
    {
        GameObject bullet = Instantiate(bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = _bulletSpawnPoint.forward * 30;
        }
        // Spawn the bullet across the network
        NetworkServer.Spawn(bullet);

        // Optionally, destroy the bullet after a certain time
        Destroy(bullet, 5f); // Destroy the bullet after 5 seconds
        _isFire = true;
    }
    
    public override TankState GetNextState()
    {
        if(_isNextState)
            return TankState.Idle;
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
