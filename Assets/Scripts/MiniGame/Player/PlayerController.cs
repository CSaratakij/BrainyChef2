using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainyChef
{
    public class PlayerController : MonoBehaviour
    {
        internal Action OnAttacking;
        internal Action OnAttackFinished;

        [SerializeField]
        [Range(0, 100)]
        int attackPoint;

        [SerializeField]
        Transform enemy;

        [SerializeField]
        Status enemyHealth;

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

        }

        void UnsubscribeEvent()
        {

        }

        public void AttackEnemy()
        {
            enemyHealth.Remove(attackPoint);
            OnAttacking?.Invoke();
            OnAttackFinished?.Invoke();
        }
    }
}

