using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CombatEventManager;

public class LifeNodeManager : MonoBehaviour
{
    //[field: SerializeField]
    public CombatEntity Entity;

    [SerializeField]
    private EntityType EntityType;

    private DefenceTypeRow _defenceTypesTop;
    private DefenceTypeRow _defenceTypesMiddle;
    private DefenceTypeRow _defenceTypesBottom;

    private static Color _meleeImmuneColor = Color.red;
    private static Color _normalColor = new Color32(104, 108, 130, 255);
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

    public void Initialize(CombatEntity combatant)
    {
        if (combatant.EntityType == EntityType)
        {
            Entity = combatant;
            _hpText = this.transform.Find("HpCanvas").Find("HpText").gameObject.GetComponent<TextMeshProUGUI>();
            Transform defenceRows = this.transform.Find("DefenceRows");
            _defenceRowTopObject = defenceRows.Find("DefenceRowTop").gameObject;
            _defenceRowMiddleObject = defenceRows.Find("DefenceRowMiddle").gameObject;
            _defenceRowBottomObject = defenceRows.Find("DefenceRowBottom").gameObject;
            _defenceTypesTop = new DefenceTypeRow(_defenceRowTopObject, Entity.StartingDefenceTop);
            _defenceTypesMiddle = new DefenceTypeRow(_defenceRowMiddleObject, Entity.StartingDefenceMiddle);
            _defenceTypesBottom = new DefenceTypeRow(_defenceRowBottomObject, Entity.StartingDefenceBottom);
            Health = Entity.CurrentHealth;
            _hpText.text = Health.ToString();

            //TESTING REMOVE
            _defenceTypesTop.SwapDefenceType(0, DefenceType.MagicImmune);
            _defenceTypesMiddle.SwapDefenceType(0, DefenceType.MeleeImmune);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        CombatEventManager.TakeDamageEvent += TakeDamage;
        CombatEventManager.AddDefenceEvent += AddDefence;
        CombatEventManager.InitializeLifeNodeEvent += Initialize;
    }

    private void OnDisable()
    {
        CombatEventManager.TakeDamageEvent -= TakeDamage;
        CombatEventManager.AddDefenceEvent -= AddDefence;
        CombatEventManager.InitializeLifeNodeEvent -= Initialize;
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
                GameObject defenceObject = null;

                //Only add defence is it is not maxed yet
                if (currentDefence < maxDefence)
                {
                    defenceRow[currentDefence] = defenceType;
                    //Set color to defencetype
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
                Destroy(gameObject.transform.GetChild(currentDefence).gameObject);
            }
        }

        public void SwapDefenceType(int position, DefenceType defenceType)
        {
            if (defenceRow[position] != DefenceType.None)
            {
                SpriteRenderer spriteRender = gameObject.transform.GetChild(position).GetComponent<SpriteRenderer>();
                switch (defenceType)
                {
                    case DefenceType.MeleeImmune:
                        defenceRow[position] = DefenceType.MeleeImmune;
                        spriteRender.sprite = CombatEventManager.DefenceObjects.MeleeImmune.GetComponent<SpriteRenderer>().sprite;
                        break;
                    case DefenceType.RangedImmune:
                        defenceRow[position] = DefenceType.RangedImmune;
                        spriteRender.sprite = CombatEventManager.DefenceObjects.RangedImmune.GetComponent<SpriteRenderer>().sprite;
                        break;
                    case DefenceType.MagicImmune:
                        defenceRow[position] = DefenceType.MagicImmune;
                        spriteRender.sprite = CombatEventManager.DefenceObjects.MagicImmune.GetComponent<SpriteRenderer>().sprite;
                        break;
                    default:
                        defenceRow[position] = DefenceType.Normal;
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
