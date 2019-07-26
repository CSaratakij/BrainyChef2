using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainyChef
{
    public class UIDebugBCI : MonoBehaviour
    {
        const int UPDATE_RATE = 2;
        static UIDebugBCI Instance = null;

        [SerializeField]
        Text txtDeviceInfo;

        void Awake()
        {
            MakeSingleton();
        }

        void Start()
        {
            /* txtDeviceInfo.gameObject.SetActive(false); */
        }

        void Update()
        {
            if ((Time.frameCount % UPDATE_RATE) == 0)
            {
                UpdateUI();
            }

            InputHandler();
        }

        void MakeSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void InputHandler()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                bool isShow = txtDeviceInfo.gameObject.activeSelf;
                isShow = !isShow;
                txtDeviceInfo.gameObject.SetActive(isShow);
            }
        }

        void UpdateUI()
        {
            txtDeviceInfo.text = InputBCI.Instance.ToString();
        }
    }
}

