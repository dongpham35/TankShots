using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.MultipleMatch;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    public Text txtCurrenthealth;

    [SyncVar(hook = nameof(OnHealthChanged))] // SyncVar with a hook for health updates
    public int currentHealth;

    [SyncVar] public uint idloser = 0;

    public int maxHealth;
    public PlayerInformation data;
    public Image healthBar;

    private void Start()
    {
        maxHealth = data.health;
        currentHealth = maxHealth;
        txtCurrenthealth.text = $"{currentHealth}/{maxHealth}";
    }
    // Called when health changes
    private void OnHealthChanged(int oldHealth, int newHealth)
    {
        if (newHealth <= 0)
        {
            Die(); // Call die method if health is zero or below
        }

        // Optionally update UI or handle visual effects here
        healthBar.fillAmount = (float)newHealth / maxHealth;
        txtCurrenthealth.text = $"{currentHealth}/{maxHealth}";
    }

    // Command to take damage
    [Server]
    public void TakeDamage(int damage)
    {
        if (!isServer) return; // Ensure this is only executed on the server

        // Prevent health from going below zero
        currentHealth = Mathf.Max(currentHealth - damage, 0);
    }

    // Method to heal the player
    [Server]
    public void Heal(int amount)
    {
        if (!isServer) return; // Ensure this is only executed on the server

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    // Method called when health reaches zero
    private void Die()
    {
        if (!isLocalPlayer) return;
        CmdSendIdloser(netId);

    }

    [Command]
    private void CmdSendIdloser(uint id)
    {
        RpcSendIdloser(idloser);
    }

    [ClientRpc]
    private void RpcSendIdloser(uint id)
    {
        idloser = id;
        if (netId == idloser)
        {
            Debug.Log("Loser");
        }
        else
        {
            Debug.Log("Winner");
        }
    }

}
