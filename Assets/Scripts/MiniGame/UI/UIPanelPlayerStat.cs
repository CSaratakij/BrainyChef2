using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainyChef
{
    public class UIPanelPlayerStat : MonoBehaviour
    {
        const string FORMAT = "{0}/{1}";

        [Header("Setting")]
        [SerializeField]
        string entityName;

        [SerializeField]
        Status health;

        [SerializeField]
        Status energy;

        [Header("UI")]
        [SerializeField]
        Text txtEntityName;

        [SerializeField]
        Text txtHealth;

        [SerializeField]
        Text txtEnergy;

        [SerializeField]
        Slider sliderHealth;

        [SerializeField]
        Slider sliderEnergy;

        void Awake()
        {
            SubscribeEvent();
        }

        void OnEnable()
        {
            Health_OnValueChanged(health.Current);
            Energy_OnValueChanged(energy.Current);
        }

        void OnDestroy()
        {
            UnsubscribeEvent();
        }

        void SubscribeEvent()
        {
            health.OnValueChanged += Health_OnValueChanged;
            energy.OnValueChanged += Energy_OnValueChanged;
        }

        void UnsubscribeEvent()
        {
            health.OnValueChanged -= Health_OnValueChanged;
            energy.OnValueChanged -= Energy_OnValueChanged;
        }

        void Health_OnValueChanged(float value)
        {
            sliderHealth.maxValue = health.Maximum;
            sliderHealth.value = value;
            txtHealth.text = string.Format(FORMAT, health.Current, health.Maximum);
        }

        void Energy_OnValueChanged(float value)
        {
            sliderEnergy.maxValue = energy.Maximum;
            sliderEnergy.value = value;
            txtEnergy.text = string.Format(FORMAT, energy.Current, energy.Maximum);
        }
    }
}

