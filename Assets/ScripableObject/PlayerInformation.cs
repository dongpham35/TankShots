using System;
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
    public int level;
    public int experirene;
    public List<LevelInfor> levelInfor = new List<LevelInfor>();
    [Serializable]
    public struct LevelInfor
    {
        public int levelID;
        public int experience;
        public int damage;
        public int health;
    }
}
