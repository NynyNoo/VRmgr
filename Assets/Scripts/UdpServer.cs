using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UdpServer
{
    public event Action<string> DataSent;
    private UdpClient udpListener;
    private int port;
    
    public UdpServer(int port)
    {
        this.port = port;
    }

    public async Task StartListening()
    {
        udpListener = new UdpClient(port);
        Debug.Log($"Server started on port {port}, waiting for connection...");

        try
        {
            while (true)
            {
                UdpReceiveResult result = await udpListener.ReceiveAsync();
                string receivedData = Encoding.UTF8.GetString(result.Buffer);
                DataConnector.Instance.SendData(port, receivedData);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error receiving UDP data on port {port}: {ex.Message}");
        }
    }

    public void Close()
    {
        if (udpListener != null)
            udpListener.Close();
    }
}
