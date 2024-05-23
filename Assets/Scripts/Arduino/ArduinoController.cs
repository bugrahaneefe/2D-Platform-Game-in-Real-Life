using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class ArduinoController : MonoBehaviour
{
    SerialPort sp = new SerialPort("/dev/cu.usbserial-10", 9600);
    Thread serialThread;
    bool keepReading = true;
    ConcurrentQueue<string> dataQueue = new ConcurrentQueue<string>();
    public string LatestData { get; private set; }

    void Start()
    {
        if (!sp.IsOpen) {
            sp.Open();
        }
        serialThread = new Thread(ReadSerialPort);
        serialThread.Start();
    }

    void Update()
    {
        int processCount = 0;
        while (processCount++ < 10 && dataQueue.TryDequeue(out string data)) {
            LatestData = data;  
        }
    }

    void ReadSerialPort() {
        while (keepReading) {
            try {
                if (sp.IsOpen) {
                    string data = sp.ReadLine();
                    if (!string.IsNullOrEmpty(data)) {
                        if (dataQueue.Count < 70) {
                            dataQueue.Enqueue(data);
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
        keepReading = false;
        if (serialThread != null && serialThread.IsAlive) {
            serialThread.Join(500);
        }
        if (sp != null && sp.IsOpen) {
            sp.Close();
        }
    }
}
