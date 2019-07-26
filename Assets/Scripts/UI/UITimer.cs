using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainyChef
{
    public class UITimer : MonoBehaviour
    {
        const string TWO_DIGIT_FORMAT = "({0:00}:{1:00})";

        [SerializeField]
        string format = "{0}";

        [SerializeField]
        string onZeroText = "";

        [SerializeField]
        bool useTwoDigitNumber = false;

        [SerializeField]
        Text txtTimer;

        [SerializeField]
        Timer timer;

        void Awake()
        {
            SubscribeEvent();
        }

        void OnEnable()
        {
            UpdateUI(timer.Current);
        }

        void OnDestroy()
        {
            UnsubscribeEvent();
        }

        void SubscribeEvent()
        {
            timer.OnTick += OnTick;
        }

        void UnsubscribeEvent()
        {
            timer.OnTick -= OnTick;
        }

        void OnTick(float value)
        {
            UpdateUI(value);
        }

        void UpdateUI(float value)
        {
            if (useTwoDigitNumber)
            {
                int minute = (int)(value / 60);
                float seconds = value % 60;
                txtTimer.text = string.Format(TWO_DIGIT_FORMAT, minute, seconds);
            }
            else
            {
                if (onZeroText != "" && value < 1.0f)
                {
                    txtTimer.text = onZeroText;
                }
                else
                {
                    txtTimer.text = string.Format(format, value);
                }
            }
        }
    }
}

