using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CombatEventManager;

public class DefenceTypeRow : MonoBehaviour
{
    private int _maxDefence = 10;
    private List<DefenceType> _defenceRow = new List<DefenceType>();
    private List<EntityType> _intents = new List<EntityType>();
    private int _currentDefence = 0;

    public void Initialize(int amount)
    {
        //Fill all defence slots
        for (int i = 0; i < _maxDefence; i++)
        {
            _defenceRow.Add(DefenceType.None);
        }
        //visually add defence
        for (int i = 0; i < amount; i++)
        {
            AddDefence(DefenceType.Normal);
        }
    }

    public void AddDefence(DefenceType defenceType)
    {
        AddDefence(1, defenceType);
    }

    public void AddDefence(int defenceAmount, DefenceType defenceType)
    {
        for (int i = 0; i < defenceAmount; i++)
        {
            GameObject defenceObject = null;

            //Only add defence is it is not maxed yet
            if (_currentDefence < _maxDefence)
            {
                _defenceRow[_currentDefence] = defenceType;
                switch (defenceType)
                {
                    case DefenceType.MeleeImmune:
                        defenceObject = CombatEventManager.DefenceObjects.MeleeImmune;
                        break;
                    case DefenceType.RangedImmune:
                        defenceObject = CombatEventManager.DefenceObjects.RangedImmune;
                        break;
                    case DefenceType.MagicImmune:
                        defenceObject = CombatEventManager.DefenceObjects.MagicImmune;
                        break;
                    default:
                        defenceObject = CombatEventManager.DefenceObjects.NormalDefence;
                        break;
                }


                Instantiate(defenceObject, this.gameObject.transform);
                //Check for intents
                if (_intents.Count != 0)
                {
                    this.transform.GetChild(_currentDefence).SetAsLastSibling();
                }

                _currentDefence++;
            }
        }
    }

    void RemoveDefence()
    {
        if (_currentDefence > 0)
        {
            _currentDefence--;

            _defenceRow[_currentDefence] = DefenceType.None;
            Destroy(this.gameObject.transform.GetChild(_currentDefence).gameObject);
        }
    }

    public void SwapDefenceType(int position, DefenceType defenceType)
    {
        if (_defenceRow[position] != DefenceType.None)
        {
            RawImage rawImage = this.gameObject.transform.GetChild(position).GetComponent<RawImage>();
            switch (defenceType)
            {
                case DefenceType.MeleeImmune:
                    _defenceRow[position] = DefenceType.MeleeImmune;
                    rawImage.texture = CombatEventManager.DefenceObjects.MeleeImmune.GetComponent<RawImage>().texture;
                    break;
                case DefenceType.RangedImmune:
                    _defenceRow[position] = DefenceType.RangedImmune;
                    rawImage.texture = CombatEventManager.DefenceObjects.RangedImmune.GetComponent<RawImage>().texture;
                    break;
                case DefenceType.MagicImmune:
                    _defenceRow[position] = DefenceType.MagicImmune;
                    rawImage.texture = CombatEventManager.DefenceObjects.MagicImmune.GetComponent<RawImage>().texture;
                    break;
                default:
                    _defenceRow[position] = DefenceType.Normal;
                    rawImage.texture = CombatEventManager.DefenceObjects.NormalDefence.GetComponent<RawImage>().texture;
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Trying to swap defence slot that is currently not in use.");
        }
    }

    public void DeleteIntent(EntityType fromEntity)
    {
        //Check if there is currently an intent by the provided entity, if so, delete it.
        if (_intents.Contains(fromEntity))
        {
            DestroyIntent(fromEntity);
        }
    }

    public int CalculateDamageReceived(int amountOfDamage, DamageType damageType)
    {
        int damageReceived = 0;

        //Check damage for each point of damage received
        for (int i = 0; i < amountOfDamage; i++)
        {
            //If defence is present, receive damage there first
            if (_currentDefence > 0)
            {
                var defenceType = _defenceRow[_currentDefence - 1];

                //Check melee
                switch (defenceType)
                {
                    case DefenceType.MeleeImmune:
                        if (damageType != DamageType.Melee)
                        {
                            RemoveDefence();
                        }
                        break;
                    case DefenceType.RangedImmune:
                        if (damageType != DamageType.Ranged)
                        {
                            RemoveDefence();
                        }
                        break;
                    case DefenceType.MagicImmune:
                        if (damageType != DamageType.Magic)
                        {
                            RemoveDefence();
                        }
                        break;
                    default:
                        RemoveDefence();
                        break;
                }
            }
            else
            {
                damageReceived++;
            }
        }

        return damageReceived;
    }

    public void CreateDamageIntent(EntityType originEntity, int damageAmount, DamageType damageType)
    {
        DamageIntent damageIntent = Instantiate(CombatEventManager.DefenceObjects.IntentObject,this.gameObject.transform).GetComponent<DamageIntent>();
        damageIntent.Initialize(originEntity, damageAmount, damageType);
        _intents.Add(originEntity);
    }

    public void DestroyIntent(EntityType originEntity)
    {
        if (_intents.Contains(originEntity))
        {
            //Get the intent in the intent list
            int index = _intents.IndexOf(originEntity);
            Destroy(this.gameObject.transform.GetChild(_currentDefence + index).gameObject);
            _intents.RemoveAt(index);

        }
    }

}