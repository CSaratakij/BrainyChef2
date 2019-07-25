using System;
using UnityEngine;

namespace BrainyChef
{
    public class Timer : MonoBehaviour
    {
        public event Action OnStopped;
        public event Action<float> OnTick;

        [SerializeField]
        float currentTime;

        [SerializeField]
        float maxTime;

        [SerializeField]
        bool isPause;

        bool isStart;

        public bool IsStart => isStart;
        public bool IsPause => isPause;

        public float Current => currentTime;
        public float Max => maxTime;

        void OnDestroy()
        {
            OnStopped = null;
            OnTick = null;
        }

        void Update()
        {
            TickHandler();
        }

        void TickHandler()
        {
            if (!isStart)
                return;

            if (isPause)
                return;

            currentTime -= (1.0f * Time.deltaTime);
            OnTick?.Invoke(currentTime);

            if (currentTime <= 0.0f)
                Stop();
        }

        public void CountDown()
        {
            if (isStart)
                return;

            Reset();
            isStart = true;
        }

        public void Stop()
        {
            if (!isStart)
                return;

            isStart = false;
            OnStopped?.Invoke();
        }

        public void Pause(bool value)
        {
            isPause = value;
        }

        public void Reset()
        {
            isStart = false;
            isPause = false;
            currentTime = maxTime;
        }

        public void TogglePause()
        {
            isPause = !isPause;
        }
    }
}

