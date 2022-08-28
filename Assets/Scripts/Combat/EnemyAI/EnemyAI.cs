using System.Collections;
using UnityEngine;
using static CombatEventManager;

namespace Assets.Scripts.Combat.EnemyAI
{
    public abstract class EnemyAI : MonoBehaviour
    {
        protected EntityType _myEntityType = EntityType.Enemy;

        private void OnEnable()
        {
            CombatEventManager.StartTurnEvent += StartTurn; 
        }

        private void OnDisable()
        {
            CombatEventManager.StartTurnEvent -= StartTurn;
        }

        protected abstract void DoTurn();

        private void StartTurn(EntityType entityType)
        {
            if (entityType == _myEntityType)
            {
                StartCoroutine(Wait(3));
            }
        }
        private void EndTurn()
        {
            CombatEventManager.EndTurn(_myEntityType);
        }

        private IEnumerator Wait(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            DoTurn();
            EndTurn();
        }

    }
}