using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CombatEventManager;

public class LifeNodeManager : MonoBehaviour
{
    public int StartingHealth;
    public int StartingDefenceTop;
    public int StartingDefenceMiddle;
    public int StartingDefenceBottom;

    private int _health;

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
        get { return _health; }
        set
        {
            if (value >= 0)
            {
                _health = value;
            }
            else
            {
                _health = 0;
            }

            _hpText.text = Health.ToString();
        }
    }

    void Start()
    {
        //Init
        _hpText = this.transform.Find("HpCanvas").Find("HpText").gameObject.GetComponent<TextMeshProUGUI>();
        _defenceRowTopObject = this.transform.Find("DefenceRowTop").gameObject;
        _defenceRowMiddleObject = this.transform.Find("DefenceRowMiddle").gameObject;
        _defenceRowBottomObject = this.transform.Find("DefenceRowBottom").gameObject;
        _defenceTypesTop = new DefenceTypeRow(_defenceRowTopObject, StartingDefenceTop);
        _defenceTypesMiddle = new DefenceTypeRow(_defenceRowMiddleObject, StartingDefenceMiddle);
        _defenceTypesBottom = new DefenceTypeRow(_defenceRowBottomObject, StartingDefenceBottom);
        Health = StartingHealth;
        _hpText.text = Health.ToString();

        _defenceTypesTop.AddDefence(DefenceType.MagicImmune);
        _defenceTypesMiddle.AddDefence(DefenceType.RangedImmune);
        _defenceTypesBottom.SwapDefenceType(2, DefenceType.MeleeImmune);

    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        CombatEventManager.DealDamageEvent += TakeDamage;
    }

    private void OnDisable()
    {
        CombatEventManager.DealDamageEvent -= TakeDamage;
    }

    public void TakeDamage(DefenceRow defenceRow, int amountOfDamage, DamageType damageType)
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

    public void ResetStats()
    {
        Health = StartingHealth;
        _defenceTypesTop = new DefenceTypeRow(_defenceRowTopObject, StartingDefenceTop);
        _defenceTypesMiddle = new DefenceTypeRow(_defenceRowMiddleObject, StartingDefenceMiddle);
        _defenceTypesBottom = new DefenceTypeRow(_defenceRowBottomObject, StartingDefenceBottom);
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

        public void AddDefence(DefenceType defenceType)
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
