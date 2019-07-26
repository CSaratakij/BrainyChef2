using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrainyChef
{
    public class EnemyController : MonoBehaviour
    {
        internal Action OnAttacking;
        internal Action OnAttackFinished;

        [SerializeField]
        [Range(0, 100)]
        int attackPoint;

        [SerializeField]
        Transform player;

        [SerializeField]
        float incrasePlayerEnergyRate = 1;

        [SerializeField]
        Status playerHealth;

        [SerializeField]
        Status playerEnergy;

        public int AttackPoint => attackPoint;

        void Awake()
        {
            SubscribeEvent();
        }

        void OnDestroy()
        {
            UnsubscribeEvent();
        }

        void SubscribeEvent()
        {
            playerHealth.OnValueChanged += OnValueChanged;
        }

        void UnsubscribeEvent()
        {
            playerHealth.OnValueChanged -= OnValueChanged;
        }

        void OnValueChanged(float value)
        {
            //if player hit by player -> blinking (change material to red)
        }

        public void AttackPlayer()
        {
            playerHealth.Remove(attackPoint);
            playerEnergy.Restore(incrasePlayerEnergyRate);

            OnAttacking?.Invoke();
            OnAttackFinished?.Invoke();
        }
    }
}

