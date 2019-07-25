using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainyChef
{
    public class UITimer : MonoBehaviour
    {
        const string FORMAT = "({0:00} : {1:00})";

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
            int minute = (int)(value / 60);
            float seconds = value % 60;
            txtTimer.text = string.Format(FORMAT, minute, seconds);
        }
    }
}

