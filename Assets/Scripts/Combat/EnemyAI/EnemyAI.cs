using System.Collections;
using UnityEngine;
using static CombatEventManager;

namespace Assets.Scripts.Combat.EnemyAI
{
    public abstract class EnemyAI : MonoBehaviour
    {
        protected EntityType _myEntityType = EntityType.Enemy;
        protected EntityType EntityType { get => _myEntityType;}
        protected CombatEntity _player;
        protected EnemyIntent _intent = null;

        private void OnEnable()
        {
            CombatEventManager.StartTurnEvent += StartTurn;

        }

        private void OnDisable()
        {
            CombatEventManager.StartTurnEvent -= StartTurn;
        }

        private void Start()
        {
            CalculateIntent();
        }

        private void DoTurn()
        {
            if (_intent.GetType() == typeof(EnemyAttackIntent))
            {
                EnemyAttackIntent atkI = (EnemyAttackIntent) _intent;
                CombatEventManager.DealDamage(atkI.OriginEntity, atkI.TargetEntity, atkI.DefenceRow, atkI.Damage, atkI.DamageType);
            }
            _intent = null;
          
        }
        protected abstract void CalculateIntent();

        private void StartTurn(EntityType entityType)
        {

            if (entityType == _myEntityType)
            {

                
                StartCoroutine(Wait(1));
            }
        }
        private void EndTurn()
        {
            CombatEventManager.EndTurn(_myEntityType);
        }

        private IEnumerator Wait(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            DoTurn(); //Timing bug with deleting defence and intent at the same time, needs to be spaced out with delay
            EndTurn();
            CalculateIntent();
        }

        public void SetEntity(EntityType entity)
        {
            _myEntityType = entity;
        }

    }
}