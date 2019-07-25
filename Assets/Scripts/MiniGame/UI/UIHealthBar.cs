using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainyChef
{
    [RequireComponent(typeof(Slider))]
    public class UIHealthBar : MonoBehaviour
    {
        [SerializeField]
        Status health;

        Slider slider;

        void Awake()
        {
            Initialize();
            SubscribeEvent();
        }

        void OnEnable()
        {
            UpdateUI(health.Current);
        }

        void OnDestroy()
        {
            UnsubscribeEvent();
        }

        void Initialize()
        {
            slider = GetComponent<Slider>();
        }

        void SubscribeEvent()
        {
            health.OnValueChanged += OnValueChanged;
        }

        void UnsubscribeEvent()
        {
            health.OnValueChanged -= OnValueChanged;
        }

        void OnValueChanged(float value)
        {
            UpdateUI(value);
        }

        void UpdateUI(float value)
        {
            slider.value = value;
            slider.maxValue = health.Maximum;
        }
    }
}

