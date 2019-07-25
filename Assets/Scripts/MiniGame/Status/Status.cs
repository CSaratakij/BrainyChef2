using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrainyChef
{
    public class Status : MonoBehaviour
    {
        [SerializeField]
        float current;

        [SerializeField]
        float maximum;

        public float Current => current;
        public float Maximum => maximum;

        public void FullRestore()
        {
            current = maximum;
        }

        public void Clear()
        {
            current = 0.0f;
        }

        public void Restore(float value)
        {
            current = (current + value) > maximum ? maximum : (current + value);
        }

        public void Remove(float value)
        {
            current = (current - value) < 0.0f ? 0.0f : (current - value);
        }
    }
}

