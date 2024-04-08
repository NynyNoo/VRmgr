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
    [SerializeField] float x,y,z,w;
    [SerializeField] TextMeshProUGUI text1;
    [SerializeField] TextMeshProUGUI text2;
    [SerializeField] GameObject muscleObject;
    DataConnector dataConnector;
    DataConverter converter;
    Vector3 accelerometer;
    Vector3 gyroscope;
    Vector3 additional;
    Vector3 additional2;
    Quaternion attitude;
    Quaternion firstQua;
    Quaternion d;
    Quaternion relativeRotation;
    DateTime timestamp;
    Quaternion rot = new Quaternion(1, 1, 1, 1);
    Quaternion startPos = new Quaternion(0, 0, 0, 0);
    Quaternion rotation90X;
    Quaternion rotatedAttitude;
    Rigidbody rb;
    Vector3 acce = Vector3.zero;
    Vector3 counter = new (0,0,0);
    Vector3 counter2 = new (0,0,0);
    Vector3 FirstData = new (0,0,0);
    CharacterController characterController;
    bool first = true;
    public float minChangeThreshold = 0.05f; // Minimalna wartoœæ zmiany, poni¿ej której nie bêdzie dodawana si³a
    public float forceMultiplier = 300f; // Wspó³czynnik mno¿¹cy si³ê
    private void Update()
    {
    }
    private void Start()
    {
        //SetupRB();
        rb=GetComponent<Rigidbody>();
        dataConnector = GetComponent<DataConnector>();
        converter = new DataConverter();
        Subscribe(portNumber);
    }


    private void Subscribe(int port)
    {
        switch (port)
        {
            case 12345:
                DataConnector.Instance.DataSentToPort12345 += ProcessData;
                break;
            case 12346:
                DataConnector.Instance.DataSentToPort12345 += ProcessData;
                break;
            case 12347:
                DataConnector.Instance.DataSentToPort12345 += ProcessData;
                break;
            case 12348:
                DataConnector.Instance.DataSentToPort12345 += ProcessData;
                break;
            default:
                Debug.LogError($"Invalid port number: {port}");
                break;
        }
    }
    void setStart(Quaternion data)
    {
        startPos = data;
    }
    void ProcessData14(string data)
    {
        (_, _, attitude, _) = converter.ParseData(data);
        if (first)
        {
            relativeRotation = Quaternion.Inverse(attitude) * muscleObject.transform.rotation;
            first = false;
        }
        muscleObject.transform.rotation = attitude * Quaternion.Inverse(relativeRotation);
        text1.text = attitude.ToString();
        // Ustawienie nowej orientacji obiektu na podstawie ró¿nicy
        //muscleObject.transform.rotation = attitude*Quaternion.Inverse(relativeRotation);
    }
    void ProcessData8(string data)
    {
        (_, _, attitude, _) = converter.ParseData(data);

        if (first)
        {
            relativeRotation = Quaternion.Inverse(attitude) * muscleObject.transform.rotation;
            first = false;
        }

        muscleObject.transform.rotation = attitude * relativeRotation;
        text1.text = attitude.ToString();
    }
    private Quaternion rightCoordToUnityCord( Quaternion q)
    {
        return new Quaternion(-q.x, q.z, -q.y, -q.w);
    }
    void ProcessData(string data)
    {
        (_, _, attitude, _) = converter.ParseData(data);
        attitude = rightCoordToUnityCord(attitude);
        if (first)
        {
            relativeRotation = Quaternion.Inverse(attitude) * muscleObject.transform.rotation;
            first = false;
        }

        muscleObject.transform.rotation = attitude * relativeRotation;
        text1.text = attitude.ToString();
    }
    void SetupRB()
    {
        
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        //rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.mass = 1f;
        //rb.maxAngularVelocity = 20f;
    }
    void safe()
    {
        var accFixed = accelerometer;
        accFixed.z -= 1.02f;
        accFixed.x += 0.01f;
        counter += accFixed;
        text1.text = (counter.ToString());

        counter += accFixed;
        text2.text = (accFixed.ToString());
        //text1.text = (accFixed.ToString());
        rb.AddForce(accFixed, ForceMode.Acceleration);
    }
    void SaveFirstData(Vector3 data)
    {
        FirstData = data;
        first = false;
    }
}
