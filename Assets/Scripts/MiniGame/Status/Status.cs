using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrainyChef
{
    public class Status : MonoBehaviour
    {
        internal Action<float> OnValueChanged;

        [SerializeField]
        float current;

        [SerializeField]
        float maximum;

        public bool IsEmpty => (current == 0);
        public float Current => current;
        public float Maximum => maximum;

        public void FullRestore()
        {
            current = maximum;
            OnValueChanged?.Invoke(current);
        }

        public void Clear()
        {
            current = 0.0f;
            OnValueChanged?.Invoke(current);
        }

        public void Restore(float value)
        {
            current = (current + value) > maximum ? maximum : (current + value);
            OnValueChanged?.Invoke(current);
        }

        public void Remove(float value)
        {
            current = (current - value) < 0.0f ? 0.0f : (current - value);
            OnValueChanged?.Invoke(current);
        }
    }
}

