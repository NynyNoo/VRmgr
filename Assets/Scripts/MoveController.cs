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
    [SerializeField] TextMeshProUGUI lostDataText;
    [SerializeField] GameObject muscleObject;
    DataConnector dataConnector;
    int toleranceMs;
    int counter;
    DataConverter converter;
    Quaternion attitude;
    Quaternion relativeRotation;
    DateTime oldDate;
    DateTime newDate;
    int lostDataAmount=0;
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

    private Quaternion rightCoordToUnityCord2( Quaternion q)
    {
        //ustawienie orientacji telefonu wzglêdem nogi
        return new Quaternion(-q.y, q.x, -q.z, -q.w);
    }
    private Quaternion rightCoordToUnityCord4(Quaternion q)
    {
        //ustawienie orientacji telefonu wzglêdem nogi
        //return new Quaternion(q.x, -q.z, q.y, q.w);

        //Debug.Log("x="+-q.y+"y="+q.x+"z="+-q.z+"w="+ -q.w);

        //return new Quaternion(q.x, -q.z, q.y, -q.w);//tylko na prawo
        Quaternion originalRotation = new Quaternion(q.x, q.y, q.z, q.w);
        // Tworzenie quaternionu reprezentuj¹cego rotacjê wokó³ osi Y o przeciwny k¹t
        Quaternion inverseYRotation = Quaternion.Euler(0, 0, 180);
        // Mno¿enie oryginalnego quaternionu przez odwrotnoœæ rotacji w osi Y
        Quaternion adjustedRotation = originalRotation * inverseYRotation;
        return adjustedRotation;
    }
    private Quaternion rightCoordToUnityCord(Quaternion q)
    {
        Quaternion originalRotation = new Quaternion(q.x, q.y, q.z, q.w);

        // Quaternion reprezentuj¹cy rotacjê o 180 stopni wokó³ osi Y
        Quaternion yRotation180 = new Quaternion(0, 0, 1, 0);

        // Mno¿enie oryginalnego quaternionu przez rotacjê wokó³ osi Y
        return originalRotation * yRotation180;
    }
    void ProcessData(string data)
    {

        (_, _, attitude, newDate) = converter.ParseData(data);
        
        attitude = rightCoordToUnityCord(attitude);

        if (first)
        {
            startingPhoneRotation = attitude;
            muscleObject.transform.rotation = startingRotation;
            startingPlayerRotation = muscleObject.transform.rotation;
            first = false;
        }
        else
            CheckDate(oldDate, newDate);
        Quaternion relativeRotation = Quaternion.Inverse(startingPhoneRotation) * attitude;

        // Dostosowanie orientacji postaci gracza
        muscleObject.transform.rotation = startingPlayerRotation * relativeRotation;

        counter++;
        timer = 0;
        oldDate = newDate;
        ChangeText();
    }
    void CheckDate(DateTime oldDate, DateTime newDate)
    {
        var timeDifference = (newDate - oldDate).TotalMilliseconds;
        if (Math.Abs(timeDifference) > toleranceMs)
        {
            lostDataAmount++;
            lostDataText.text = lostDataAmount.ToString() +" Missing Data";
        }
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
