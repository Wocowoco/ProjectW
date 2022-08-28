using System.Collections.Generic;
using UnityEngine;

public class CombatEventManager : MonoBehaviour
{
    //DamageEvent
    public delegate void DamageEvent(EntityType targetEntity, DefenceRow defenceRow, int damageAmount, DamageType damageType);
    public static event DamageEvent TakeDamageEvent;
    //DefenceEvent
    public delegate void DefenceEvent(EntityType targetEntity, DefenceRow defenceRow, int defenceAmount, DefenceType defenceType);
    public static event DefenceEvent AddDefenceEvent;

    private List<EntityType> _combatants = new List<EntityType>();


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }





    public static void DealDamage(EntityType targetEntity, DefenceRow defenceRow, int damageAmount, DamageType damageType = DamageType.Melee)
    {
        //Check if there are any subscribers on damageEvent before invoking it
        if (TakeDamageEvent != null)
        {
            TakeDamageEvent.Invoke(targetEntity, defenceRow, damageAmount, damageType);
        }
    }
    public static void AddDefence(EntityType targetEntity, DefenceRow defenceRow, int defenceAmount, DefenceType defenceType = DefenceType.Normal)
    {
        //Check if there are any subscribers on defenceEvent before invoking it
        if (AddDefenceEvent != null)
        {
            AddDefenceEvent.Invoke(targetEntity, defenceRow, defenceAmount, defenceType);
        }
    }

    public enum DefenceRow
    {
        Top = 1,
        Middle,
        Bottom
    }

    public enum DefenceType
    {
        None,
        Normal,
        MeleeImmune,
        RangedImmune,
        MagicImmune,
        //Regen
    }

    public enum DamageType
    {
        Melee,
        Ranged,
        Magic
    }

    public enum EntityType
    {
        Player,
        Enemy1
    }
}
