using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrainyChef
{
    public class MiniGameController : MonoBehaviour
    {
        internal Action OnGameStart;
        internal Action OnGameOver;

        internal bool IsStart { get; set; }

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

