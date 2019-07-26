using System;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrainyChef
{
    public class InputBCI : MonoBehaviour
    {
        const float SERIAL_POLLING_RATE = 0.1f;
        internal static InputBCI Instance = null;

        struct RawData
        {
            public bool IsDeviceAvailable;
            public int Attention;
            public int Meditation;
            public float Delta;
        }

        [SerializeField]
        string portName = "/dev/ttyACM0";

        [SerializeField]
        int buadRate = 57600;

        SerialPort serialPort;
        RawData rawData;

        Coroutine serialPollingCoroutine;
        WaitForSeconds serialPollingWait;

        internal bool IsDeviceAvailable => rawData.IsDeviceAvailable;
        internal int Attention => rawData.Attention;
        internal int Meditation => rawData.Meditation;
        internal float Delta =>  rawData.Delta;

        void Awake()
        {
            MakeSingleton();
            Initialize();
        }

        void Update()
        {
            PrintRawData();
        }

        void OnDestroy()
        {
            /* serialPort.Close(); */
        }

        void Initialize()
        {
            rawData.IsDeviceAvailable = false;
            rawData.Attention = 0;
            rawData.Meditation = 0;
            rawData.Delta = 0.0f;

            serialPort = new SerialPort(portName, buadRate);
            serialPort.ReadTimeout = 101;

            serialPort.Open();

            serialPollingWait = new WaitForSeconds(SERIAL_POLLING_RATE);
            serialPollingCoroutine = StartCoroutine(SerialPollingCallback(serialPollingWait));
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

        IEnumerator SerialPollingCallback(WaitForSeconds wait)
        {
            while (true)
            {
                if (!serialPort.IsOpen)
                {
                    rawData.IsDeviceAvailable = false;
                    Debug.Log("Serial port is not open..");
                    yield return null;
                }

                try
                {
                    GetRawData(serialPort, ref rawData);
                }
                catch (Exception)
                {

                }

                yield return wait;
            }
        }

        RawData GetRawData(SerialPort serialPort)
        {
            RawData data = new RawData();
            GetRawData(serialPort, ref data);
            return data;
        }

        RawData GetRawData(SerialPort serialPort, ref RawData data)
        {
            string strData = serialPort.ReadLine();
            string[] strDataArray = strData.Split(',');

            int outputA;
            int outputB;

            bool isDeviceAvailable = (strDataArray[0].Equals("0")) ? true : false;
            bool isCanParseA = int.TryParse(strDataArray[1], out outputA);
            bool isCanParseB = int.TryParse(strDataArray[2], out outputB);

            data.IsDeviceAvailable = isDeviceAvailable;
            data.Attention = (isCanParseA) ? outputA : 0;
            data.Meditation = (isCanParseB) ? outputB : 0;
            data.Delta = (data.Attention - data.Meditation);

            return data;
        }

        void PrintRawData()
        {
            ToString();
        }

        public override string ToString()
        {
            string result = string.Empty;

            result += "Device : " + ((rawData.IsDeviceAvailable) ? "connected" : "disconnected") + Environment.NewLine;
            result += "Attention : " + rawData.Attention + Environment.NewLine;
            result += "Meditation : " + rawData.Meditation + Environment.NewLine;
            result += "Delta : " + rawData.Delta + Environment.NewLine;

            return result;
        }
    }
}
