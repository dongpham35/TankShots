using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Health", menuName = "ScriptableObjects/player/Information")]
public class PlayerInformation : ScriptableObject
{
    public int health;
    public int minDamage;
    public int maxDamage;
    public int fireRate;
}
