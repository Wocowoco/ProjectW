using System.Collections;
using UnityEngine;
using static CombatEventManager;

namespace Assets.Scripts.Combat.EnemyAI
{
    public class MeleeBottomAI : EnemyAI
    {
        protected override void DoTurn()
        {
            CombatEventManager.DealDamage(EntityType.Player, DefenceRow.Bottom, 1, DamageType.Melee);
        }
    }
}