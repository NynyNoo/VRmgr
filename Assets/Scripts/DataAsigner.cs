using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataAsigner : MonoBehaviour
{
    [SerializeField] int portNumber;
    [SerializeField] TextMeshProUGUI text1;
    [SerializeField] TextMeshProUGUI text2;
    DataConnector dataConnector;
    DataConverter converter;
    Vector3 accelerometer;
    Vector3 gyroscope;
    Quaternion attitude;
    DateTime timestamp;
    GameObject gameObject;
    Rigidbody rb;
    Vector3 acce = Vector3.zero;
    Vector3 counter = new(0, 0, 0);
    Vector3 counter2 = new(0, 0, 0);
    Vector3 FirstData = new(0, 0, 0);
    CharacterController characterController;
    bool first = true;
    public float minChangeThreshold = 0.05f; // Minimalna wartoœæ zmiany, poni¿ej której nie bêdzie dodawana si³a
    public float forceMultiplier = 300f; // Wspó³czynnik mno¿¹cy si³ê
    private void Start()
    {
        //SetupRB();
        characterController = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
        dataConnector = GetComponent<DataConnector>();
        converter = new DataConverter();
        Subscribe(portNumber);
        gameObject = GetComponent<GameObject>();
        transform.SetPositionAndRotation(transform.position, transform.rotation);
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

    void ProcessData(string data)
    {
        (_, gyroscope, attitude, _) = converter.ParseData(data);
        accelerometer.y = 0;
        accelerometer.z = 0;

        if (first)
        {
            FirstData = accelerometer;
            first = false;
        }
        Vector3 accFixed = accelerometer - FirstData;
        Move(accFixed);
        text1.text = (accFixed.ToString());
        text2.text = (rb.velocity.ToString());
    }
    private void Move(Vector3 acc)
    {
        Vector3 acceleration = acc;

        Vector3 moveDirection = new(acceleration.x * 5 * Time.deltaTime, 0, -acceleration.z * 5 * Time.deltaTime);
        Vector3 transformedDirection = transform.TransformDirection(moveDirection);

        characterController.Move(transformedDirection);
    }
}
