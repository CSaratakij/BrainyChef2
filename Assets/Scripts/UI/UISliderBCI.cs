﻿using System;
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
        internal Action<float> OnTimeoutValue;

        [SerializeField]
        float minBCI = 1;

        [SerializeField]
        float maxBCI = 200;

        [SerializeField]
        InputBCIType inputBCIType;

        [SerializeField]
        float increaseRate = 1;

        [SerializeField]
        Slider slider;

        [SerializeField]
        bool noDecreasement = false;

        [SerializeField]
        bool noTimer = false;

        [SerializeField]
        Text lblTimeout;

        Timer timer;

        void Awake()
        {
            Initialize();
            SubscribeEvent();
        }

        void Update()
        {
            TickHandler();
        }

        void OnEnable()
        {
            if (noTimer)
            {
                lblTimeout.text = string.Empty;
                return;
            }

            timer.Reset();
            timer.CountDown();

            UpdateUI(timer.Current);
        }

        void OnDisable()
        {
            slider.value = 20;
            timer.Reset();
        }

        void Initialize()
        {
            timer = GetComponent<Timer>();
        }

        void TickHandler()
        {
            float value = 0.0f;

            if (InputBCI.Instance.IsDeviceAvailable)
            {
                //TODO (bridge InputBCI here...)
                float rawValue = (inputBCIType == InputBCIType.Attention) ? InputBCI.Instance.Attention : InputBCI.Instance.Meditation;
                float processedValue = 0.0f;

                if (rawValue >= maxBCI)
                {
                    processedValue = 1.0f;
                }
                else if (rawValue < minBCI)
                {
                    processedValue = -1.0f * 0.5f;
                }

                value = (processedValue * increaseRate) * Time.deltaTime;
            }
            else
            {
                value = (Input.GetAxisRaw("Vertical") * increaseRate) * Time.deltaTime;
            }

            slider.value += value;
        }

        void SubscribeEvent()
        {
            timer.OnStopped += OnStopped;
            timer.OnTick += OnTick;
            OnValueMax += Self_OnValueMax;
            slider.onValueChanged.AddListener((value) => OnValueChanged(value));
        }

        void UnsubscribeEvent()
        {
            timer.OnStopped -= OnStopped;
            timer.OnTick -= OnTick;
            OnValueMax -= Self_OnValueMax;
        }

        void Self_OnValueMax(float value)
        {
            timer.Pause(true);
        }

        void OnTick(float value)
        {
            UpdateUI(value);
        }

        void OnStopped()
        {
            OnTimeoutValue?.Invoke(slider.value);
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

        void UpdateUI(float value)
        {
            lblTimeout.text = string.Format("({0:0})", timer.Current);
        }
    }
}

