using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainyChef
{
    [RequireComponent(typeof(Slider))]
    public class UIHealthBar : MonoBehaviour
    {
        const int UPDATE_RATE = 3;

        [SerializeField]
        Status health;

        [SerializeField]
        Transform origin;

        Camera camera;
        Slider slider;

        void Awake()
        {
            Initialize();
            SubscribeEvent();
        }

        void OnEnable()
        {
            UpdateUIValue(health.Current);
        }

        void Update()
        {
            if ((Time.frameCount % UPDATE_RATE) == 0)
            {
                UpdateUIPosition();
            }
        }

        void OnDestroy()
        {
            UnsubscribeEvent();
        }

        void Initialize()
        {
            camera = Camera.main;
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
            UpdateUIValue(value);
        }

        void UpdateUIValue(float value)
        {
            slider.value = value;
            slider.maxValue = health.Maximum;
        }

        void UpdateUIPosition()
        {
            Vector2 screenPoint = camera.WorldToScreenPoint(origin.position);
            transform.position = screenPoint;
        }
    }
}

