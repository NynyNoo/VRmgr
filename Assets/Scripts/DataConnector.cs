using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DataConnector : MonoBehaviour
{
    private static DataConnector instance;
    public event Action<string> DataSentToPort12345;
    public event Action<string> DataSentToPort12346;
    public event Action<string> DataSentToPort12347;
    public event Action<string> DataSentToPort12348;
    private void OnDestroy()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public static DataConnector Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject server = GameObject.Find("Server");
                instance = server.AddComponent<DataConnector>();
            }
            return instance;
        }
    }
    public void SendData(int port, string data)
    { 
        switch (port)
        {
            case 12345:
                {
                    DataSentToPort12345(data);
                    break;
                };
            case 12346:
                {
                    DataSentToPort12346(data);
                    break;
                }
            case 12347:
                {
                    DataSentToPort12347(data);
                    break;
                }
            case 12348:
                {
                    DataSentToPort12348(data);
                    break;
                }
            default:
                Debug.LogError($"Invalid port number: {port}");
                break;
        }
    }
}
