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
        //accFixed.x = (float)Math.Round(accFixed.x, 2);
       /* if (Mathf.Abs(accFixed.x) > 0.03f || Mathf.Abs(accFixed.y) > 0.03f || Mathf.Abs(accFixed.z) > 0.03f)
        {
            characterController.Move(accFixed);
            //rb.AddForce(accFixed*200, ForceMode.Acceleration);

        }*/
        Move(accFixed);
        //LookAround(gyroscope);
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
    private void LookAround(Vector3 gyros)
    {
        Quaternion attitude = new Quaternion(0,0,0,0);

        transform.rotation = attitude;
        transform.Rotate(0f, 0f, 180f, Space.Self);
        transform.Rotate(90f, 180f, 0f, Space.World);

        transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation, 0.1f);
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
