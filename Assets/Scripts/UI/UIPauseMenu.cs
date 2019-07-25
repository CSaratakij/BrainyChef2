using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrainyChef
{
    public class UIPauseMenu : MonoBehaviour
    {
        [SerializeField]
        RectTransform pauseMenu;

        void Awake()
        {
            SetPause(false);
        }

        void Update()
        {
            InputHandler();
        }

        void InputHandler()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                TogglePause();
            }
        }

        void SetPause(bool value)
        {
            pauseMenu.gameObject.SetActive(value);
            Time.timeScale = (value) ? 0.0f : 1.0f;
        }

        void TogglePause()
        {
            bool isPause = (Time.timeScale == 0.0f);
            isPause = !isPause;
            SetPause(isPause);
        }

        public void Resume()
        {
            SetPause(false);
        }

        public void BackToMainMenu()
        {
            SetPause(false);
            /* SceneManager.LoadScene(0); */
        }
    }
}

