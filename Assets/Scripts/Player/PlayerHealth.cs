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

    public int maxHealth;
    public PlayerInformation data;
    public Image healthBar;

    [Header("End game property")]
    public GameObject UI_EndGame;
    public Text txtStatus;


    private const string STRING_WIN = "YOU WIN";
    private const string STRING_LOST = "YOU LOST";

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
        if (currentHealth <= 0)
            Debug.Log("ID loser is : " + netId);
    }

    // Method to heal the player
    [Server]
    public void Heal(int amount)
    {
        if (!isServer) return; // Ensure this is only executed on the server

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Die()
    {
        if (isLocalPlayer)
            ActiveEndGame(STRING_LOST);
        else
            ActiveEndGame(STRING_WIN);
    }

    private void ActiveEndGame(string status)
    {
        UI_EndGame.SetActive(true);
        txtStatus.text = status;
    }

    public void TurnOnMenu()
    {
        Debug.Log("Turn on menu");
    }

}
