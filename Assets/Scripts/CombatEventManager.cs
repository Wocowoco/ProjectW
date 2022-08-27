using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LifeNodeManager;

public class CombatEventManager : MonoBehaviour
{
    //DamageEvent
    public delegate void DamageEvent(DefenceRow defenceRow, int damageAmount, DamageType damageType);
    public static event DamageEvent DealDamageEvent;
    //DefenceEvent
    public delegate void DefenceEvent(DefenceRow defenceRow, int defenceAmount, DefenceType defenceType);
    public static event DefenceEvent AddDefenceEvent;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void DealDamage(DefenceRow defenceRow, int damageAmount, DamageType damageType = DamageType.Melee)
    {
        //Check if there are any subscribers on damageEvent before invoking it
        if (DealDamageEvent != null)
        {
            DealDamageEvent.Invoke(defenceRow, damageAmount, damageType);
        }
    }
    public static void AddDefence(DefenceRow defenceRow, int defenceAmount, DefenceType defenceType = DefenceType.Normal)
    {
        //Check if there are any subscribers on damageEvent before invoking it
        if (DealDamageEvent != null)
        {
            AddDefenceEvent.Invoke(defenceRow, defenceAmount, defenceType);
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
}
