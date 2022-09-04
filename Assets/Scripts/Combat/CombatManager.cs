using Assets.Scripts.Combat.EnemyAI;
using System.Collections.Generic;
using UnityEngine;
using static CombatEventManager;

public class CombatManager : MonoBehaviour
{
    public GameObject EnemyLifeNode;
    public GameObject EnemyLifeNodeCollection;

    private int _turn = 0;
    private int _round = 0;
    private List<CombatEntity> _combatants = new List<CombatEntity>();
    private List<CombatEntity> _waitingCombatants = new List<CombatEntity>();
    private Dictionary<EntityType, GameObject> _enemyEntitiesInCombat = new();
    private int _enemiesToBeAdded = 0;

    void OnEnable()
    {
        CombatEventManager.DefeatedCombatantEvent += RemoveCombatant;
    }

    void OnDisable()
    {
        CombatEventManager.DefeatedCombatantEvent -= RemoveCombatant;
    }

    void Start()
    {
        _enemyEntitiesInCombat.Add(EntityType.Enemy, null);
        _enemyEntitiesInCombat.Add(EntityType.Enemy2, null);
        _enemyEntitiesInCombat.Add(EntityType.Enemy3, null);

        CombatEntity player = new(999, 999, 4, 2, 0, 0, 0, EntityType.Player);
        CombatEntity enemy = new(3, 3, 3, 0, 0, 0, 0, EntityType.Enemy);
        CombatEntity enemy2 = new(2, 2, 2, 2, 2, 2, 2, EntityType.Enemy);
        CombatEntity enemy3 = new(4, 4, 2, 3, 3, 3, 3, EntityType.Enemy);
        CombatEntity enemy4 = new(5, 5, 2, 4, 4, 0, 0, EntityType.Enemy);
        CombatEntity enemy5 = new(7, 7, 2, 5, 5, 0, 0, EntityType.Enemy);
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
                //Start next round
                EndRound();
            }
            //Start next combatants turn
            CombatEventManager.StartTurn(_combatants[_turn].EntityType);
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
            if (_enemyEntitiesInCombat[EntityType.Enemy] == null)
            {
                _combatants.Add(combatEntity);
                GameObject lifeNode = Instantiate(EnemyLifeNode, EnemyLifeNodeCollection.transform);
                lifeNode.GetComponent<LifeNodeManager>().SetEntity(EntityType.Enemy);
                lifeNode.AddComponent<MeleeBottomAI>().SetEntity(EntityType.Enemy);
                CombatEventManager.InitializeLifeNode(combatEntity);
                _enemyEntitiesInCombat[EntityType.Enemy] = lifeNode;
            }
            else if (_enemyEntitiesInCombat[EntityType.Enemy2] == null)
            {
                combatEntity.EntityType = EntityType.Enemy2;
                _combatants.Add(combatEntity);
                GameObject lifeNode = Instantiate(EnemyLifeNode, EnemyLifeNodeCollection.transform);
                lifeNode.GetComponent<LifeNodeManager>().SetEntity(EntityType.Enemy2);
                lifeNode.AddComponent<MeleeBottomAI>().SetEntity(EntityType.Enemy2);
                CombatEventManager.InitializeLifeNode(combatEntity);
                _enemyEntitiesInCombat[EntityType.Enemy2] = lifeNode;
            }
            else if(_enemyEntitiesInCombat[EntityType.Enemy3] == null)
            {
                combatEntity.EntityType = EntityType.Enemy3;
                _combatants.Add(combatEntity);
                GameObject lifeNode = Instantiate(EnemyLifeNode, EnemyLifeNodeCollection.transform);
                lifeNode.GetComponent<LifeNodeManager>().SetEntity(EntityType.Enemy3);
                lifeNode.AddComponent<MeleeBottomAI>().SetEntity(EntityType.Enemy3);
                CombatEventManager.InitializeLifeNode(combatEntity);
                _enemyEntitiesInCombat[EntityType.Enemy3] = lifeNode;
            }
            else
            {
                _waitingCombatants.Add(combatEntity);
            }
        } //Add player
        else if (combatEntity.EntityType == EntityType.Player)
        {
            _combatants.Add(combatEntity);
            CombatEventManager.InitializeLifeNode(combatEntity);
        }

        _combatants.Sort();
    }

    private void RemoveCombatant(CombatEntity combatEntity)
    {
        if (combatEntity.EntityType != EntityType.Player)
        {
            //Remove combatant's LifeNode and intents
            Destroy(_enemyEntitiesInCombat[combatEntity.EntityType]);
            CombatEventManager.DeleteIntent(combatEntity.EntityType);
            //Remove combatant from dictonary and combatants
            _enemyEntitiesInCombat[combatEntity.EntityType] = null;
            _combatants.Remove(combatEntity);

            //Check if a new enemy needs to added to combat
            if (_waitingCombatants.Count != 0)
            {
                _enemiesToBeAdded++;
            }
            else if (_combatants.Count == 1) //Check if this was the last enemy
            {
                //Combat is won
                Debug.Log("Player is VICTORIOUS");
            }
        }
        else
        {
            //PLAYER IS DEFEATED
        }
    }

    private void EndRound()
    {
        //Start next round, on turn 0.
        _round++;
        _turn = 0;

        //Check if any new enemies need to be added to combat.
        if (_enemiesToBeAdded != 0)
        {
            for (int i = 0; i < _enemiesToBeAdded; i++)
            {

                AddCombatant(_waitingCombatants[0]);
                _waitingCombatants.RemoveAt(0);
            }
            _enemiesToBeAdded = 0;
        }

        CombatEventManager.EndRound();
    }

}
