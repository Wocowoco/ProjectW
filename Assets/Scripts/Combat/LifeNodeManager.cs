using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CombatEventManager;

public class LifeNodeManager : MonoBehaviour
{
    //[field: SerializeField]
    public CombatEntity Entity;

    private DefenceTypeRow _defenceTypesTop;
    private DefenceTypeRow _defenceTypesMiddle;
    private DefenceTypeRow _defenceTypesBottom;

    private static Color _meleeImmuneColor = Color.red;
    private static Color _normalColor = Color.white;
    private static Color _magicImmuneColor = Color.blue;
    private static Color _rangedImmuneColor = Color.green;

    private GameObject _defenceRowTopObject;
    private GameObject _defenceRowMiddleObject;
    private GameObject _defenceRowBottomObject;

    private TextMeshProUGUI _hpText;

    public int Health
    {
        get { return Entity.CurrentHealth; }
        set
        {
            if (value >= 0 && value <= Entity.MaxHealth)
            {
                Entity.CurrentHealth = value;
            }
            else
            {
                Entity.CurrentHealth = 0;
            }
            _hpText.text = Health.ToString();
        }
    }

    public void Initialize(CombatEntity entity)
    {
        //Init
        Entity = entity;
        _hpText = this.transform.Find("HpCanvas").Find("HpText").gameObject.GetComponent<TextMeshProUGUI>();
        _defenceRowTopObject = this.transform.Find("DefenceRowTop").gameObject;
        _defenceRowMiddleObject = this.transform.Find("DefenceRowMiddle").gameObject;
        _defenceRowBottomObject = this.transform.Find("DefenceRowBottom").gameObject;
        _defenceTypesTop = new DefenceTypeRow(_defenceRowTopObject, Entity.StartingDefenceTop);
        _defenceTypesMiddle = new DefenceTypeRow(_defenceRowMiddleObject, Entity.StartingDefenceMiddle);
        _defenceTypesBottom = new DefenceTypeRow(_defenceRowBottomObject, Entity.StartingDefenceBottom);
        Health = Entity.CurrentHealth;
        _hpText.text = Health.ToString();
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        CombatEventManager.TakeDamageEvent += TakeDamage;
        CombatEventManager.AddDefenceEvent += AddDefence;
    }

    private void OnDisable()
    {
        CombatEventManager.TakeDamageEvent -= TakeDamage;
        CombatEventManager.AddDefenceEvent -= AddDefence;
    }

    private void TakeDamage(EntityType entityType, DefenceRow defenceRow, int amountOfDamage, DamageType damageType)
    {
        if (Entity.EntityType == entityType)
        {
            switch (defenceRow)
            {
                case DefenceRow.Top:
                    Health -= _defenceTypesTop.CalculateDamageReceived(amountOfDamage, damageType);
                    break;
                case DefenceRow.Middle:
                    Health -= _defenceTypesMiddle.CalculateDamageReceived(amountOfDamage, damageType);
                    break;
                case DefenceRow.Bottom:
                    Health -= _defenceTypesBottom.CalculateDamageReceived(amountOfDamage, damageType);
                    break;
                default:
                    break;
            }
        }
    }

    private void AddDefence(EntityType entityType, DefenceRow defenceRow, int defenceAmount, DefenceType defenceType)
    {
        if (Entity.EntityType == entityType)
        {
            switch (defenceRow)
            {
                case DefenceRow.Top:
                    _defenceTypesTop.AddDefence(defenceType, defenceAmount);
                    break;
                case DefenceRow.Middle:
                    _defenceTypesMiddle.AddDefence(defenceType, defenceAmount);
                    break;
                case DefenceRow.Bottom:
                    _defenceTypesBottom.AddDefence(defenceType, defenceAmount);
                    break;
                default:
                    break;
            }
        }
    }

    public void ResetStats()
    {
        Health = Entity.MaxHealth;
        _defenceTypesTop = new DefenceTypeRow(_defenceRowTopObject, Entity.StartingDefenceTop);
        _defenceTypesMiddle = new DefenceTypeRow(_defenceRowMiddleObject, Entity.StartingDefenceMiddle);
        _defenceTypesBottom = new DefenceTypeRow(_defenceRowBottomObject, Entity.StartingDefenceBottom);

    }


    public class DefenceTypeRow
    {
        private readonly int maxDefence = 10;
        private List<DefenceType> defenceRow = new List<DefenceType>();
        private int currentDefence = 0;
        private readonly GameObject gameObject;

        public DefenceTypeRow(GameObject obj, int amount)
        {
            gameObject = obj;
            //Fill all defence slots
            for (int i = 0; i < maxDefence; i++)
            {
                defenceRow.Add(DefenceType.None);
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            //visually add defence
            for (int i = 0; i < amount; i++)
            {
                AddDefence(DefenceType.Normal);
            }
        }

        public void AddDefence(DefenceType defenceType, int defenceAmount = 1)
        {
            for (int i = 0; i < defenceAmount; i++)
            {
                //Only add defence is it is not maxed yet
                if (currentDefence < maxDefence)
                {
                    defenceRow[currentDefence] = defenceType;
                    //Set color to defencetype
                    switch (defenceType)
                    {
                        case DefenceType.Normal:
                            gameObject.transform.GetChild(currentDefence).GetComponent<SpriteRenderer>().color = _normalColor;
                            break;
                        case DefenceType.MeleeImmune:
                            gameObject.transform.GetChild(currentDefence).GetComponent<SpriteRenderer>().color = _meleeImmuneColor;
                            break;
                        case DefenceType.RangedImmune:
                            gameObject.transform.GetChild(currentDefence).GetComponent<SpriteRenderer>().color = _rangedImmuneColor;
                            break;
                        case DefenceType.MagicImmune:
                            gameObject.transform.GetChild(currentDefence).GetComponent<SpriteRenderer>().color = _magicImmuneColor;
                            break;
                        default:
                            break;
                    }
                    gameObject.transform.GetChild(currentDefence).gameObject.SetActive(true);

                    currentDefence++;
                }
            }
        }

        void RemoveDefence()
        {
            if (currentDefence > 0)
            {
                currentDefence--;

                defenceRow[currentDefence] = DefenceType.None;
                gameObject.transform.GetChild(currentDefence).gameObject.SetActive(false);
            }
        }

        public void SwapDefenceType(int position, DefenceType defenceType)
        {
            if (defenceRow[position] != DefenceType.None)
            {
                switch (defenceType)
                {
                    case DefenceType.Normal:
                        defenceRow[position] = DefenceType.Normal;
                        gameObject.transform.GetChild(position).GetComponent<SpriteRenderer>().color = _normalColor;
                        break;
                    case DefenceType.MeleeImmune:
                        defenceRow[position] = DefenceType.MeleeImmune;
                        gameObject.transform.GetChild(position).GetComponent<SpriteRenderer>().color = _meleeImmuneColor;
                        break;
                    case DefenceType.RangedImmune:
                        defenceRow[position] = DefenceType.RangedImmune;
                        gameObject.transform.GetChild(position).GetComponent<SpriteRenderer>().color = _rangedImmuneColor;
                        break;
                    case DefenceType.MagicImmune:
                        defenceRow[position] = DefenceType.MagicImmune;
                        gameObject.transform.GetChild(currentDefence).GetComponent<SpriteRenderer>().color = _magicImmuneColor;
                        break;
                    default:
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
                if (currentDefence > 0)
                {
                    var defenceType = defenceRow[currentDefence-1];

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

    }



}
