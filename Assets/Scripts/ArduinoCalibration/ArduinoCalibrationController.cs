using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class ArduinoCalibrationController : MonoBehaviour
{
    [SerializeField]
    private ArduinoController arduinoController;
    [SerializeField]
    private ArduinoController_2 arduinoController_2;
    private float xInput_1 = 0;
    private float yInput_1 = 0;
    private float zInput_1 = 0;
    private float xInput_2 = 0;
    private float yInput_2 = 0;
    private float zInput_2 = 0;
    [SerializeField]
    private TextMeshProUGUI player_1_isCalibrateText;
    private bool player_1_isCalibrated = false;
    [SerializeField] private RectTransform targetDot;
    [SerializeField] private RectTransform inputDot_1;
    [SerializeField]
    private TextMeshProUGUI player_2_isCalibrateText;
    private bool player_2_isCalibrated = false;
    [SerializeField] private RectTransform inputDot_2;
    [SerializeField]
    private TextMeshProUGUI countdownText;  
    private bool countdownStarted = false; 

    void Start()
    {
        arduinoController = GetComponent<ArduinoController>();
        arduinoController_2 = GetComponent<ArduinoController_2>();
        StartCoroutine(CalibratingTextAnimation(player_1_isCalibrateText));
        StartCoroutine(CalibratingTextAnimation(player_2_isCalibrateText));
    }

    void Update()
    {
        ProcessSerialData(arduinoController.LatestData); 
        ProcessSerialData_2(arduinoController_2.LatestData_2); 
        ProcessInputsPlayerOne();
        ProcessInputsPlayerTwo();

        if (player_1_isCalibrated && player_2_isCalibrated && !countdownStarted)
        {
            countdownStarted = true;
            StartCoroutine(CountdownAndLoadNextScene());
        }
    }

    IEnumerator CountdownAndLoadNextScene()
    {
        countdownText.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            float startTime = Time.time;
            while (Time.time - startTime < 1.5f)
            {
                if (!player_1_isCalibrated || !player_2_isCalibrated)
                {
                    countdownText.gameObject.SetActive(false);
                    countdownText.text = "";
                    countdownStarted = false;
                    yield break; 
                }
                yield return null;  
            }
        }
        SceneManager.LoadScene(1); 
    }


    private void ProcessInputsPlayerOne()
    {
        Vector2 inputPosition = new Vector2(-xInput_1, yInput_1) * 100;
        inputDot_1.anchoredPosition = inputPosition;

        if(Vector2.Distance(inputDot_1.anchoredPosition, Vector2.zero) < 15.0f)
        {
            
            player_1_isCalibrateText.text = "Player 1 is calibrated";
            player_1_isCalibrated = true;
            StopCoroutine(CalibratingTextAnimation(player_1_isCalibrateText));
        } else {
            player_1_isCalibrated = false;
        }
    }

    private void ProcessInputsPlayerTwo()
    {
        Vector2 inputPosition = new Vector2(xInput_2, yInput_2) * 100;
        inputDot_2.anchoredPosition = inputPosition;

        if(Vector2.Distance(inputDot_2.anchoredPosition, Vector2.zero) < 15.0f)
        {
            player_2_isCalibrateText.text = "Player 2 is calibrated";
            player_2_isCalibrated = true;
            StopCoroutine(CalibratingTextAnimation(player_2_isCalibrateText));
        } else {
            player_2_isCalibrated = false;
        }
    }

    IEnumerator CalibratingTextAnimation(TextMeshProUGUI textMesh)
    {
        while (true)
        {
            textMesh.text = "Calibrating.";
            yield return new WaitForSeconds(0.5f);
            textMesh.text = "Calibrating..";
            yield return new WaitForSeconds(0.5f);
            textMesh.text = "Calibrating...";
            yield return new WaitForSeconds(0.5f);
        }
    }
   
    private void ProcessSerialData(string data)
{
    Debug.Log("Data received: " + data);
    string[] parts = data.Split(',');

    // Additional variable to store the sensor value
    float sensorValue = 0.0f;

    foreach (var part in parts)
    {
        string trimmedPart = part.Trim();
        int colonIndex = trimmedPart.IndexOf(':');
        if (colonIndex > 0)  // Ensure there is a colon in the string
        {
            string key = trimmedPart.Substring(0, colonIndex).Trim();
            string value = trimmedPart.Substring(colonIndex + 1).Trim();

            if (float.TryParse(value, out float parsedValue))
            {
                switch (key)
                {
                    case "x1":
                        xInput_1 = parsedValue;
                        break;
                    case "y1":
                        yInput_1 = parsedValue;
                        break;
                    case "z1":
                        zInput_1 = parsedValue;
                        break;
                    case "x2":
                        //xInput_2 = parsedValue;
                        break;
                    case "y2":
                        //yInput_2 = parsedValue;
                        break;
                    case "z2":
                        //zInput_2 = parsedValue;
                        break;
                }
            }
            else
            {
                Debug.LogError("Failed to parse value for " + key);
            }
        }
        else  // Handle the sensor value which does not have a key
        {
            if (float.TryParse(trimmedPart, out sensorValue))
            {
                // Handle sensor value if needed
                Debug.Log("Sensor value: " + sensorValue);
                if (sensorValue < 60)
                {
                }
            }
            else
            {
                Debug.LogError("Failed to parse sensor value.");
            }
        }
    }
}

private void ProcessSerialData_2(string data)
{
    Debug.Log("Data received: " + data);
    string[] parts = data.Split(',');

    // Additional variable to store the sensor value
    float sensorValue = 0.0f;

    foreach (var part in parts)
    {
        string trimmedPart = part.Trim();
        int colonIndex = trimmedPart.IndexOf(':');
        if (colonIndex > 0)  // Ensure there is a colon in the string
        {
            string key = trimmedPart.Substring(0, colonIndex).Trim();
            string value = trimmedPart.Substring(colonIndex + 1).Trim();

            if (float.TryParse(value, out float parsedValue))
            {
                switch (key)
                {
                    case "x1":
                        xInput_2 = -parsedValue;
                        break;
                    case "y1":
                        yInput_2 = parsedValue;
                        break;
                    case "z1":
                        zInput_2 = parsedValue;
                        break;
                }
            }
            else
            {
                Debug.LogError("Failed to parse value for " + key);
            }
        }
        else  
        {
            if (float.TryParse(trimmedPart, out sensorValue))
            {
                Debug.Log("Sensor value222: " + sensorValue);
                if (sensorValue < 200)
                {
                }
            }
            else
            {
                Debug.LogError("Failed to parse sensor value.");
            }
        }
    }
}

}