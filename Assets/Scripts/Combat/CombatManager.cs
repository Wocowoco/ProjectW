using Assets.Scripts.Combat.EnemyAI;
using System.Collections.Generic;
using Unity.Rendering.HybridV2;
using Unity.VisualScripting;
using UnityEngine;
using static CombatEventManager;

public class CombatManager : MonoBehaviour
{
    public GameObject EnemyLifeNode;
    public GameObject EnemyLifeNodeCollection;

    private int _turn = 0;
    private int _round = 0;
    private List<CombatEntity> _combatants = new List<CombatEntity>();
    private Dictionary<EntityType, bool> _enemyEntitiesInCombat = new();

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    void Start()
    {
        //Initialize Dictionary
        _enemyEntitiesInCombat.Add(EntityType.Enemy, false);
        _enemyEntitiesInCombat.Add(EntityType.Enemy2, false);
        _enemyEntitiesInCombat.Add(EntityType.Enemy3, false);

        
        CombatEntity player = new(5, 5, 5, 2, 2, 1, 3, EntityType.Player);
        CombatEntity enemy = new(3, 3, 3, 0, 1, 1, 1, EntityType.Enemy);
        CombatEntity enemy2 = new(7, 7, 2, 2, 1, 3, 2, EntityType.Enemy);
        CombatEntity enemy3 = new(4, 4, 2, 2, 5, 4, 3, EntityType.Enemy);
        CombatEntity enemy4 = new(4, 4, 2, 2, 5, 4, 3, EntityType.Enemy);
        AddCombatant(player);
        AddCombatant(enemy);
        AddCombatant(enemy2);
        AddCombatant(enemy3);
        AddCombatant(enemy4);

        EndTurnEvent += IncreaseTurnCounter;
        StartTurn(_combatants[_turn].EntityType);
    }

    private void IncreaseTurnCounter(EntityType combatant)
    {
        //Check if the endturn event came from the combatant whose turn it was, otherwise ignore the event.
        if (_combatants[_turn].EntityType == combatant)
        {
            //Debug.Log($"Round {_round}, turn {_turn} ended. ({_combatants[_turn].EntityType})");
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

    private void AddCombatant(CombatEntity combatEntity)
    {
        if (combatEntity.EntityType == EntityType.Enemy)
        {
            //Try to put enemy in slot1
            if (_enemyEntitiesInCombat[EntityType.Enemy] == false)
            {
                _combatants.Add(combatEntity);
                GameObject lifeNode = Instantiate(EnemyLifeNode, EnemyLifeNodeCollection.transform);
                lifeNode.GetComponent<LifeNodeManager>().SetEntity(EntityType.Enemy);
                this.gameObject.AddComponent<MeleeBottomAI>().SetEntity(EntityType.Enemy);
                CombatEventManager.InitializeLifeNode(combatEntity);
                _enemyEntitiesInCombat[EntityType.Enemy] = true;
            }
            else if (_enemyEntitiesInCombat[EntityType.Enemy2] == false)
            {
                combatEntity.EntityType = EntityType.Enemy2;
                _combatants.Add(combatEntity);
                GameObject lifeNode = Instantiate(EnemyLifeNode, EnemyLifeNodeCollection.transform);
                lifeNode.GetComponent<LifeNodeManager>().SetEntity(EntityType.Enemy2);
                this.gameObject.AddComponent<MeleeBottomAI>().SetEntity(EntityType.Enemy2);
                CombatEventManager.InitializeLifeNode(combatEntity);
                _enemyEntitiesInCombat[EntityType.Enemy2] = true;
            }
            else if(_enemyEntitiesInCombat[EntityType.Enemy3] == false)
            {
                combatEntity.EntityType = EntityType.Enemy3;
                _combatants.Add(combatEntity);
                GameObject lifeNode = Instantiate(EnemyLifeNode, EnemyLifeNodeCollection.transform);
                lifeNode.GetComponent<LifeNodeManager>().SetEntity(EntityType.Enemy3);
                this.gameObject.AddComponent<MeleeBottomAI>().SetEntity(EntityType.Enemy3);
                CombatEventManager.InitializeLifeNode(combatEntity);
                _enemyEntitiesInCombat[EntityType.Enemy3] = true;
            }
            else
            {
                Debug.LogError("TOO MANY ENEMIES");
            }
        } //Add player
        else if (combatEntity.EntityType == EntityType.Player)
        {
            _combatants.Add(combatEntity);
            CombatEventManager.InitializeLifeNode(combatEntity);
        }

        _combatants.Sort();
    }

}
