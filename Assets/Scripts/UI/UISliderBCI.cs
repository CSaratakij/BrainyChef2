using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainyChef
{
    public class UISliderBCI : MonoBehaviour
    {
        internal Action<float> OnValueMax;
        internal Action<float> OnValueMin;

        [SerializeField]
        InputBCIType inputBCIType;

        [SerializeField]
        float increaseRate = 1;

        [SerializeField]
        Slider slider;

        [SerializeField]
        bool noDecreasement = false;

        void Awake()
        {
            SubscribeEvent();
        }

        void OnDisable()
        {
            slider.value = 20;
        }

        void Update()
        {
            TickHandler();
        }

        void TickHandler()
        {
            if (InputBCI.Instance.IsDeviceAvailable)
            {
                //TODO (bridge InputBCI here...)
            }
            else
            {
                slider.value += (Input.GetAxisRaw("Vertical") * increaseRate) * Time.deltaTime;
            }
        }

        void SubscribeEvent()
        {
            slider.onValueChanged.AddListener((value) => OnValueChanged(value));
        }

        void OnValueChanged(float value)
        {
            if (value >= slider.maxValue)
            {
                Debug.Log("Max");
                OnValueMax?.Invoke(value);
            }
            else if (value <= slider.minValue)
            {
                Debug.Log("Min");
                OnValueMin?.Invoke(value);
            }
        }
    }
}

