using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private List<EntityType> _intentList = new();

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
            _hpText = this.transform.Find("HpText").gameObject.GetComponent<TextMeshProUGUI>();
            Transform defenceRows = this.transform.Find("DefenceRows");
            _defenceTypesTop = defenceRows.Find("DefenceRowTop").AddComponent<DefenceTypeRow>();
            _defenceTypesTop.Initialize(Entity.StartingDefenceTop);
            _defenceTypesMiddle = defenceRows.Find("DefenceRowMiddle").AddComponent<DefenceTypeRow>();
            _defenceTypesMiddle.Initialize(Entity.StartingDefenceMiddle);
            _defenceTypesBottom = defenceRows.Find("DefenceRowBottom").AddComponent<DefenceTypeRow>();
            _defenceTypesBottom.Initialize(Entity.StartingDefenceBottom);

            Health = Entity.CurrentHealth;
            _hpText.text = Health.ToString();

            //TESTING REMOVE
            _defenceTypesTop.SwapDefenceType(0, DefenceType.MagicImmune);
            _defenceTypesMiddle.SwapDefenceType(0, DefenceType.MeleeImmune);
        }
    }

    private void OnEnable()
    {
        CombatEventManager.TakeDamageEvent += TakeDamage;
        CombatEventManager.AddDefenceEvent += AddDefence;
        CombatEventManager.InitializeCombatantEvent += Initialize;
        CombatEventManager.EnemyIntentEvent += IdentifyEnemyIntent;
    }

    private void OnDisable()
    {
        CombatEventManager.TakeDamageEvent -= TakeDamage;
        CombatEventManager.AddDefenceEvent -= AddDefence;
        CombatEventManager.InitializeCombatantEvent -= Initialize;
        CombatEventManager.EnemyIntentEvent -= IdentifyEnemyIntent;
    }

    private void TakeDamage(EntityType originEntity, EntityType targetEntity, DefenceRow defenceRow, int amountOfDamage, DamageType damageType)
    {
        //Check if the entity already has an intent, if so, delete the old intent.
        if (_intentList.Contains(originEntity))
        {
            _intentList.Remove(originEntity);
            _defenceTypesTop.DestroyIntent(originEntity);
            _defenceTypesMiddle.DestroyIntent(originEntity);
            _defenceTypesBottom.DestroyIntent(originEntity);

        }
        if (Entity.EntityType == targetEntity)
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

    private void AddDefence(EntityType originEntity, EntityType targetEntity, DefenceRow defenceRow, int defenceAmount, DefenceType defenceType)
    {
        if (Entity.EntityType == targetEntity)
        {
            switch (defenceRow)
            {
                case DefenceRow.Top:
                    _defenceTypesTop.AddDefence(defenceAmount, defenceType);
                    break;
                case DefenceRow.Middle:
                    _defenceTypesMiddle.AddDefence(defenceAmount, defenceType);
                    break;
                case DefenceRow.Bottom:
                    _defenceTypesBottom.AddDefence(defenceAmount, defenceType);
                    break;
                default:
                    break;
            }
        }
    }

    private void IdentifyEnemyIntent(EnemyIntent intent)
    {
        //Check if the entity already has an intent, if so, delete the old intent.
        if (_intentList.Contains(intent.OriginEntity))
        {
            _defenceTypesTop.DestroyIntent(intent.OriginEntity);
            _defenceTypesMiddle.DestroyIntent(intent.OriginEntity);
            _defenceTypesBottom.DestroyIntent(intent.OriginEntity);
            _intentList.Remove(intent.OriginEntity);
        }
        if (Entity.EntityType == intent.TargetEntity)
        {
            _intentList.Add(intent.OriginEntity);
            if (intent.GetType() == typeof(EnemyAttackIntent))
            {
                EnemyAttackIntent attackIntent = (EnemyAttackIntent)intent;
                switch (attackIntent.DefenceRow)
                {
                    case DefenceRow.Top:
                        _defenceTypesTop.CreateDamageIntent(attackIntent.OriginEntity ,attackIntent.Damage, attackIntent.DamageType);
                        break;
                    case DefenceRow.Middle:
                        _defenceTypesMiddle.CreateDamageIntent(attackIntent.OriginEntity, attackIntent.Damage, attackIntent.DamageType);
                        break;
                    case DefenceRow.Bottom:
                        _defenceTypesBottom.CreateDamageIntent(attackIntent.OriginEntity, attackIntent.Damage, attackIntent.DamageType);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ResetStats()
    {
        Health = Entity.MaxHealth;
        _defenceTypesTop.Initialize(Entity.StartingDefenceTop);
        _defenceTypesMiddle.Initialize(Entity.StartingDefenceMiddle);
        _defenceTypesBottom.Initialize(Entity.StartingDefenceBottom);
    }

}
