using Assets.Scripts.Combat.EnemyAI;
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

    //TurnEvent
    public delegate void TurnEvent(EntityType combatant);
    public static event TurnEvent StartTurnEvent;
    public static event TurnEvent EndTurnEvent;

    //InitializeEvent
    public delegate void InitializeEvent(CombatEntity combatant);
    public static event InitializeEvent InitializeLifeNodeEvent;

    private List<CombatEntity> _combatants = new List<CombatEntity>();

    private int _turn = 0;
    private int _round = 0;


    void Start()
    {
        this.gameObject.AddComponent<MeleeBottomAI>();
        CombatEntity player = new(5, 5, 5, 5, 5, 5, EntityType.Player);
        CombatEntity enemy = new(3, 3, 3, 1, 1, 1);
        _combatants.Add(player);
        InitializeLifeNode(player);
        _combatants.Add(enemy);
        InitializeLifeNode(enemy);
        _combatants.Sort();

        AddDefence(EntityType.Enemy, DefenceRow.Bottom, 2, DefenceType.MeleeImmune);
        EndTurnEvent += IncreaseTurnCounter;
        StartTurn(_combatants[_turn].EntityType);
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
            Debug.Log($"Dealing {damageAmount} {damageType} damage to {targetEntity} on the {defenceRow} row.");
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

    public static void EndTurn(EntityType combatant)
    {
        //Check if there are any subscribers
        if (EndTurnEvent != null)
        {
            EndTurnEvent.Invoke(combatant);
        }
    }

    public static void StartTurn(EntityType combatant)
    {
        Debug.Log($"Starting {combatant}'s turn.");
        //Check if there are any subscribers
        if (StartTurnEvent != null)
        {
            StartTurnEvent.Invoke(combatant);
        }
    }

    public static void InitializeLifeNode(CombatEntity combatant)
    {
        //Check if there are any subscribers
        if (InitializeLifeNodeEvent != null)
        {
            InitializeLifeNodeEvent.Invoke(combatant);
        }
    }

    private void IncreaseTurnCounter(EntityType combatant)
    {
        //Check if the endturn event came from the combatant whose turn it was, otherwise ignore the event.
        if (_combatants[_turn].EntityType == combatant)
        {
            Debug.Log($"Round {_round}, turn {_turn} ended. ({_combatants[_turn].EntityType})");
            _turn++;
            if (_turn == _combatants.Count)
            {
                _round++;
                //Logic for a new round
                _turn = 0;
            }
            //Start next combatants turn
            StartTurn(_combatants[_turn].EntityType);
        }
        else
        {
            Debug.LogWarning($"EndTurnEvent received from {combatant}. (Expected {_combatants[_turn].EntityType})");
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
        //Regenerating defence?
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
