using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainyChef
{
    [RequireComponent(typeof(Slider))]
    public class UIHealthBar : MonoBehaviour
    {
        Slider slider;

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            slider = GetComponent<Slider>();
        }
    }
}

