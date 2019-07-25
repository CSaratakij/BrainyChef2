using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrainyChef
{
    public class GameController : MonoBehaviour
    {
        internal static GameController Instance = null;

        internal Action OnGameStart;
        internal Action OnGameOver;

        internal bool IsStart { get; set; }

        void Awake()
        {
            MakeSingleton();
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

        internal void GameStart()
        {
            if (IsStart)
            {
                return;
            }

            IsStart = true;
            OnGameStart?.Invoke();
        }

        internal void GameOver()
        {
            if (!IsStart)
            {
                return;
            }

            IsStart = false;
            OnGameOver?.Invoke();
        }
    }
}

