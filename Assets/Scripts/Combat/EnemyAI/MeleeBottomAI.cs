using System.Collections;
using UnityEngine;
using static CombatEventManager;

namespace Assets.Scripts.Combat.EnemyAI
{
    public class MeleeBottomAI : EnemyAI
    {
        protected override void DoTurn()
        {
            int randNumber = Random.Range(1, 101);
            /*
            if (_PlayerLifeNode.)
            if (_PlayerLifeNode.)
            {

            }*/

            if (randNumber <= 75)
            {
                CombatEventManager.DealDamage(EntityType.Player, DefenceRow.Bottom, 1, DamageType.Melee);
            }
            else if (randNumber > 75 && randNumber <= 99)
            {
                CombatEventManager.DealDamage(EntityType.Player, DefenceRow.Middle, 1, DamageType.Melee);
            }
            else if (randNumber == 100)
            {
                CombatEventManager.DealDamage(EntityType.Player, DefenceRow.Top, 1, DamageType.Melee);
            }
        }
    }
}