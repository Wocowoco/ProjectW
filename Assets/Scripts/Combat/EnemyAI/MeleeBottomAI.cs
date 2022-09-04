using System.Collections;
using UnityEditor;
using UnityEngine;
using static CombatEventManager;

namespace Assets.Scripts.Combat.EnemyAI
{
    public class MeleeBottomAI : EnemyAI
    {
        protected override void CalculateIntent()
        {
            int randNumber = Random.Range(1, 101);
            int damage = 1; 
            if (randNumber > 75)
            {
                damage = 2;
            }

            if (randNumber % 3 == 1)
            {
                _intent = new EnemyAttackIntent(this.EntityType, EntityType.Player, DefenceRow.Top, damage, DamageType.Melee);
            }
            else if (randNumber % 3 == 2)
            {
                _intent = new EnemyAttackIntent(this.EntityType, EntityType.Player, DefenceRow.Middle, damage, DamageType.Melee);
            }
            else
            {
                _intent = new EnemyAttackIntent(this.EntityType, EntityType.Player, DefenceRow.Bottom, damage, DamageType.Melee);
            }

            CombatEventManager.EmitEnemyIntent(_intent);
        }
    }
}