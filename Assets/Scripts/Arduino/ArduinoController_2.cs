using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Collections.Concurrent;

public class ArduinoController_2 : MonoBehaviour
{
    SerialPort sp_2 = new SerialPort("/dev/cu.usbserial-110", 9600);
    Thread serialThread_2;
    bool keepReading_2 = true; 
    ConcurrentQueue<string> dataQueue_2 = new ConcurrentQueue<string>();
    public string LatestData_2 { get; private set; }

    void Start()
    {
        if (!sp_2.IsOpen) {
            sp_2.Open();
        }
        serialThread_2 = new Thread(ReadSerialPort_2);
        serialThread_2.Start();
    }

    void Update()
    {
        int processCount = 0;
        while (processCount++ < 10 && dataQueue_2.TryDequeue(out string data)) {
            LatestData_2 = data;  
        }
    }

    void ReadSerialPort_2() {
        while (keepReading_2) {
            try {
                if (sp_2.IsOpen) {
                    string data = sp_2.ReadLine();
                    if (!string.IsNullOrEmpty(data)) {
                        if (dataQueue_2.Count < 70) {
                            dataQueue_2.Enqueue(data);
                        } else {
                        }
                    }
                }
            } catch (System.Exception ex) {
                Debug.LogError("Serial port read error: " + ex.Message);
            }
        }
    }

    void OnDisable() {
        StopSerialThread();
    }

    void OnApplicationQuit() {
        StopSerialThread();
    }

    void StopSerialThread() {
        keepReading_2 = false;
        if (serialThread_2 != null && serialThread_2.IsAlive) {
            serialThread_2.Join(500);
        }
        if (sp_2 != null && sp_2.IsOpen) {
            sp_2.Close();
        }
    }
}
