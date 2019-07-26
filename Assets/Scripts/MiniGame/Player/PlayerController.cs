﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainyChef
{
    public class PlayerController : MonoBehaviour
    {
        internal Action<float> OnAttacking;
        internal Action OnAttackFinished;

        [SerializeField]
        [Range(0, 100)]
        int attackPoint;

        [SerializeField]
        Transform enemy;

        [SerializeField]
        Status playerHealth;

        [SerializeField]
        Status playerEnergy;

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
            playerHealth.OnValueChanged += OnValueChanged;
        }

        void UnsubscribeEvent()
        {
            playerHealth.OnValueChanged -= OnValueChanged;
        }

        void OnValueChanged(float value)
        {
            //if player hit by enemy -> add player an energy
        }

        public void AttackEnemy(float value)
        {
            enemyHealth.Remove(value);
            OnAttacking?.Invoke(value);
            OnAttackFinished?.Invoke();
        }

        public void AttackEnemy()
        {
            AttackEnemy(attackPoint);
        }
    }
}

