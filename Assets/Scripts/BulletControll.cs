using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletControll : NetworkBehaviour
{
    private const string NAME_TERRAIN = "Terrain";
    private const string NAME_ENEMY = "Enemy";
    private const string NAME_PLAYER = "Player";

    private const int MIN_DAMAGE = 1;
    private const int MAX_DAMAGE = 5;

    public GameObject onwer;
    

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case NAME_TERRAIN:
            {
                Destroy(gameObject);
                break;
            }
            case NAME_ENEMY:
            {
                int randDamage = Random.Range(MIN_DAMAGE, MAX_DAMAGE);
                EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {

                    // Optionally, you can log the player's NetworkIdentity
                    NetworkIdentity networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();
                    if (networkIdentity != null)
                    {
                        enemyHealth.CmdTakeDamage(randDamage); // Call CmdTakeDamage to apply damage
                    }
                }

                if (enemyHealth.currentHealth <= 0)
                {
                    PlayerController player = onwer.GetComponent<PlayerController>();
                    player.count++;
                }
                // Destroy the bullet on impact
                Destroy(gameObject);
                break;
            }
            case NAME_PLAYER:
            {
                int randDamage = Random.Range(MIN_DAMAGE, MAX_DAMAGE);
                PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {

                    // Optionally, you can log the player's NetworkIdentity
                    NetworkIdentity networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();
                    if (networkIdentity != null)
                    {
                        playerHealth.CmdTakeDamage(randDamage); // Call CmdTakeDamage to apply damage
                    }
                }

                // Destroy the bullet on impact
                Destroy(gameObject);
                break;
            }
                
        }
    }
}
