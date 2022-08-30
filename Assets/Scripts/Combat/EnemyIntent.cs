using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static CombatEventManager;

public abstract class EnemyIntent
{
    public readonly EntityType TargetEntity;
    public readonly EntityType OriginEntity;
    public EnemyIntent(EntityType origin, EntityType target)
    {
        TargetEntity = target;
        OriginEntity = origin;
    }

}

public class EnemyAttackIntent : EnemyIntent
{
    public readonly int Damage;
    public readonly DamageType DamageType;
    public DefenceRow DefenceRow;

    public EnemyAttackIntent(EntityType origin, EntityType target, DefenceRow row, int damage, DamageType damageType) : base(origin, target)
    {
        Damage = damage;
        DamageType = damageType;
        DefenceRow = row;
    }
}
