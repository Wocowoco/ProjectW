using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            Debug.Log($"Destroying {this.gameObject.transform.GetChild(_currentDefence).gameObject} as removeDefence");
            Destroy(this.gameObject.transform.GetChild(_currentDefence).gameObject);
        }
    }

    public void SwapDefenceType(int position, DefenceType defenceType)
    {
        if (_defenceRow[position] != DefenceType.None)
        {
            SpriteRenderer spriteRender = this.gameObject.transform.GetChild(position).GetComponent<SpriteRenderer>();
            switch (defenceType)
            {
                case DefenceType.MeleeImmune:
                    _defenceRow[position] = DefenceType.MeleeImmune;
                    spriteRender.sprite = CombatEventManager.DefenceObjects.MeleeImmune.GetComponent<SpriteRenderer>().sprite;
                    break;
                case DefenceType.RangedImmune:
                    _defenceRow[position] = DefenceType.RangedImmune;
                    spriteRender.sprite = CombatEventManager.DefenceObjects.RangedImmune.GetComponent<SpriteRenderer>().sprite;
                    break;
                case DefenceType.MagicImmune:
                    _defenceRow[position] = DefenceType.MagicImmune;
                    spriteRender.sprite = CombatEventManager.DefenceObjects.MagicImmune.GetComponent<SpriteRenderer>().sprite;
                    break;
                default:
                    _defenceRow[position] = DefenceType.Normal;
                    spriteRender.sprite = CombatEventManager.DefenceObjects.NormalDefence.GetComponent<SpriteRenderer>().sprite;
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Trying to swap defence slot that is currently not in use.");
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
        Debug.Log($"Attack Intent received on {this.gameObject}");
        DamageIntent damageIntent = Instantiate(CombatEventManager.DefenceObjects.IntentObject,this.gameObject.transform).GetComponent<DamageIntent>();
        damageIntent.Initialize(damageAmount);
        _intents.Add(originEntity);
    }

    public void DestroyIntent(EntityType originEntity)
    {
        if (_intents.Contains(originEntity))
        {
            //Get the intent in the intent list
            int index = _intents.IndexOf(originEntity);
            Debug.Log($"Destroying {this.gameObject.transform.GetChild(_currentDefence + index).gameObject} as intentDestroy");
            Destroy(this.gameObject.transform.GetChild(_currentDefence + index).gameObject);
            _intents.RemoveAt(index);

        }
    }

}