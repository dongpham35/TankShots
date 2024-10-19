using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHealthChanged))] // SyncVar with a hook for health updates
    public int currentHealth;

    public int maxHealth;
    public EnemyInformation data;
    public Image healthBar;

    private void Start()
    {
        maxHealth = data.health;
        currentHealth = maxHealth;
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
    }

    // Command to take damage
    [Server]
    public void CmdTakeDamage(int damage)
    {
        if (!isServer) return; // Ensure this is only executed on the server

        // Prevent health from going below zero
        currentHealth = Mathf.Max(currentHealth - damage, 0);
    }

    // Method to heal the player
    [Server]
    public void CmdHeal(int amount)
    {
        if (!isServer) return; // Ensure this is only executed on the server

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    // Method called when health reaches zero
    private void Die()
    {
        // Handle player death (disable the player, trigger respawn, etc.)
        Debug.Log($"{gameObject.name} has died!");
        // Example: Disable the player object or trigger a respawn
        gameObject.SetActive(false); // For example, disable the player
    }
}
