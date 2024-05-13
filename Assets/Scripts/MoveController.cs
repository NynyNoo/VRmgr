using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.Rendering.DebugUI.Table;

public class MoveController : MonoBehaviour
{
    [SerializeField] int portNumber;
    [SerializeField] TextMeshProUGUI text1;
    [SerializeField] GameObject muscleObject;
    DataConnector dataConnector;
    int counter;
    DataConverter converter;
    Quaternion attitude;
    Quaternion relativeRotation;
    bool first = true;
    string sensorName;
    float timer = 0.0f;
    int seconds;
    Quaternion startingRotation;
    Quaternion startingPhoneRotation;
    Quaternion startingPlayerRotation;
    private void Start()
    {
        counter = 0;
        dataConnector = GetComponent<DataConnector>();
        converter = new DataConverter();
        Subscribe(portNumber);
        startingRotation = muscleObject.transform.rotation;
    }

    void Update()
    {
        if (!first)
        {
            timer += Time.deltaTime;
            seconds = Convert.ToInt32(timer % 60);
            if (seconds > 5)
                ResetText();
        }
    }
    private void Subscribe(int port)
    {
        switch (port)
        {
            case 12345:
                DataConnector.Instance.DataSentToPort12345 += ProcessData;
                sensorName = "LeftTop ";
                break;
            case 12346:
                DataConnector.Instance.DataSentToPort12346 += ProcessData;
                sensorName = "LeftDown ";
                break;
            case 12347:
                DataConnector.Instance.DataSentToPort12347 += ProcessData;
                sensorName = "RightTop ";
                break;
            case 12348:
                DataConnector.Instance.DataSentToPort12348 += ProcessData;
                sensorName = "RightDown ";
                break;
            default:
                Debug.LogError($"Invalid port number: {port}");
                break;
        }
    }

    private Quaternion rightCoordToUnityCord( Quaternion q)
    {
        //ustawienie orientacji telefonu wzglêdem nogi
        return new Quaternion(-q.y, q.x, -q.z, -q.w);
    }
    void ProcessData(string data)
    {
        (_, _, attitude, _) = converter.ParseData(data);
        attitude = rightCoordToUnityCord(attitude);

        if (first)
        {
            startingPhoneRotation = attitude;
            muscleObject.transform.rotation = startingRotation;
            startingPlayerRotation = muscleObject.transform.rotation;
            first = false;
        }

        Quaternion relativeRotation = Quaternion.Inverse(startingPhoneRotation) * attitude;

        // Dostosowanie orientacji postaci gracza
        muscleObject.transform.rotation = startingPlayerRotation * relativeRotation;

        counter++;
        timer = 0;
        ChangeText();
    }
    void ChangeText()
    {
        text1.color = Color.green;
        text1.text = sensorName + counter.ToString() + " records";
    }
    void ResetText()
    {
        text1.color = Color.red;
        text1.text = sensorName + "sensor disconnected or on screensaver mode";
    }
    public void ResetRotiation()
    {
        first = true;
    }
}
