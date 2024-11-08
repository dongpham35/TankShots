using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEditor;
using UnityEngine;

public class ObjectPool : NetworkBehaviour
{

    public static ObjectPool Instance { get; private set; }
    public static Dictionary<GameObject, GameObject> bulletPools = new Dictionary<GameObject, GameObject>();
    public GameObject bulletPrefab;

    public int maxBullets = 100;
    public override void OnStartServer()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        InitBullet();
        
    }
    [Server]
    private void InitBullet()
    {

        InitBulletInClient();
    }

    [TargetRpc]
    private void InitBulletInClient()
    {

    }

    public GameObject GetBullet(GameObject onwer)
    {
        foreach (var bullet in bulletPools.Values)
        {
            if (bullet.activeInHierarchy)  continue;
            bulletPools[bullet] = onwer;
            return bullet;
        }

        return null;
    }
    
}
