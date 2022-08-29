using System.Collections;
using UnityEngine;
using static CombatEventManager;

namespace Assets.Scripts.Combat.EnemyAI
{
    public abstract class EnemyAI : MonoBehaviour
    {

        protected EntityType _myEntityType = EntityType.Enemy;
        protected LifeNodeManager _PlayerLifeNode;

        private void OnEnable()
        {
            _PlayerLifeNode = transform.Find("PlayerLifeNode").GetComponent<LifeNodeManager>();
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
                StartCoroutine(Wait(0));
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