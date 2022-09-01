using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnergyNodeManager : MonoBehaviour
{
    private int _maxEnergy;
    public int MaxEnergy { get => _maxEnergy; }

    private int _currentEnergy = 0;

    GameObject _background;
    GameObject _foreground;

    // Start is called before the first frame update
    void Start()
    {
        _background = this.transform.Find("Background").gameObject;
        _foreground = this.transform.Find("Foreground").gameObject;
    }

    private void OnEnable()
    {
        CombatEventManager.InitializeCombatantEvent += Initialize;
        CombatEventManager.StartTurnEvent += RefreshEnergy;
        CombatEventManager.SpendEnergyEvent += SpendEnergy;
        
    }

    private void OnDisable()
    {
        CombatEventManager.InitializeCombatantEvent -= Initialize;
        CombatEventManager.StartTurnEvent -= RefreshEnergy;
        CombatEventManager.SpendEnergyEvent -= SpendEnergy;
    }

    void Initialize(CombatEntity entity)
    {
        //Check for player event init, otherwise ignore
        if (entity.EntityType != CombatEventManager.EntityType.Player)
        {
            return;
        }

        _maxEnergy = entity.MaxEnergy;
        _currentEnergy = MaxEnergy;

        //Draw EnergyNodes
        for (int i = 0; i < _maxEnergy; i++)
        {
            Instantiate(CombatEventManager.EnergyObjects.EnergyBackground, _background.transform);
            Instantiate(CombatEventManager.EnergyObjects.EnergyForeground, _foreground.transform);
        }
    }

    private void RefreshEnergy(CombatEventManager.EntityType combatant)
    {
        if (combatant != CombatEventManager.EntityType.Player)
        {
            return;
        }

        //Refill energy visually
        int missingEnergy = MaxEnergy - _currentEnergy;
        for (int i = 0; i < missingEnergy; i++)
        {
            Instantiate(CombatEventManager.EnergyObjects.EnergyForeground, _foreground.transform);
        }

        _currentEnergy = MaxEnergy;
    }

    private void SpendEnergy(int energyAmount)
    {

        if (_currentEnergy - energyAmount < 0)
        {
            Debug.LogError("Energy below zero");
            return;
        }

        //Remove energy visually
        for (int i = 0; i < energyAmount; i++)
        {
            Debug.Log(_foreground.transform.GetChild(i).gameObject);
            Destroy(_foreground.transform.GetChild(i).gameObject);
        }

        //Update leftover energy
        _currentEnergy -= energyAmount;

        //If out of energy, end the player turn
        if (_currentEnergy == 0)
        {
            CombatEventManager.EndTurn(CombatEventManager.EntityType.Player);
        }
        else
        {
            CombatEventManager.RemainingEnergy(_currentEnergy);
        }
    }
}
