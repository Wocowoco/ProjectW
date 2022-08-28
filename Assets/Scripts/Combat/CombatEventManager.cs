using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TreeEditor;
using UnityEngine;

public class CombatEventManager : MonoBehaviour
{
    //DamageEvent
    public delegate void DamageEvent(EntityType targetEntity, DefenceRow defenceRow, int damageAmount, DamageType damageType);
    public static event DamageEvent TakeDamageEvent;
    //DefenceEvent
    public delegate void DefenceEvent(EntityType targetEntity, DefenceRow defenceRow, int defenceAmount, DefenceType defenceType);
    public static event DefenceEvent AddDefenceEvent;

    //LifeNodeManagers
    private LifeNodeManager playerLifeNode;
    private LifeNodeManager enemyLifeNode;
    private List<CombatEntity> _combatants = new List<CombatEntity>();
    private int _turn = 0;
    private int _round = 0;


    void Start()
    {
        playerLifeNode = transform.GetChild(0).GetComponent<LifeNodeManager>();
        enemyLifeNode = transform.GetChild(1).GetComponent<LifeNodeManager>();

        CombatEntity player = new(5, 5, 5, 0, 1, 1, EntityType.Player);
        CombatEntity enemy = new(3, 3, 3, 1, 1, 1);
        _combatants.Add(player);
        _combatants.Add(enemy);
        _combatants.Sort();
        playerLifeNode.Initialize(player);
        enemyLifeNode.Initialize(enemy);
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
        Enemy
    }
}
