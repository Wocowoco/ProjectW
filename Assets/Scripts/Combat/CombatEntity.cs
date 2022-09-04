using System;
using UnityEngine;
using static CombatEventManager;

public class CombatEntity : IComparable<CombatEntity>
{
    [field: SerializeField]
    public EntityType EntityType { get; set; } = EntityType.Enemy;


    //Stats 
    [field: SerializeField]
    public int MaxHealth { get; private set; }

    public int CurrentHealth { get; set; }

    [field: SerializeField]
    public int StartingSpeed { get; private set; }

    [field: SerializeField]
    public int MaxEnergy { get; private set; }


    //Defence rows
    [field: SerializeField]
    public int StartingDefenceTop { get; private set; } = 0;

    [field: SerializeField]
    public int StartingDefenceMiddle { get; private set; } = 0;

    [field: SerializeField]
    public int StartingDefenceBottom { get; private set; } = 0;


    public CombatEntity(int maxHp, int currentHp, int speed, int maxEnergy, int defTop = 0, int defMid = 0, int defBot = 0, EntityType entityType = EntityType.Enemy)
    {
        MaxHealth = maxHp;
        CurrentHealth = currentHp;
        StartingSpeed = speed;
        MaxEnergy = maxEnergy;
        StartingDefenceTop = defTop;
        StartingDefenceMiddle = defMid;
        StartingDefenceBottom = defBot;
        EntityType = entityType;
    }

    public int CompareTo(CombatEntity other)
    {
        if (this.StartingSpeed >= other.StartingSpeed)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}