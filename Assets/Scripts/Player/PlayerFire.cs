using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.MultipleMatch;
using UnityEngine;

public class PlayerFire : NetworkBehaviour
{
    public PlayerInformation playerInfo;
    public Transform bulletSpawn;
    public GameObject bulletPrefab;

    private float fireCooldown;
    private float timerFire;

    private void Start()
    {
        fireCooldown = 1 / playerInfo.fireRate;
        timerFire = fireCooldown;
    }
    private void Update()
    {
        if(!isLocalPlayer) return;
        if(timerFire > 0) timerFire -= Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && timerFire <= 0)
        {
            CmdFire();
        }
    }

    [Command]
    private void CmdFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawn.forward * 30;
        }

        // Spawn the bullet across the network
        NetworkServer.Spawn(bullet);
        bullet.GetComponent<BulletControll>().onwer = this.gameObject;
        // Optionally, destroy the bullet after a certain time
        Destroy(bullet, 2f); // Destroy the bullet after 5 seconds
    }
}
