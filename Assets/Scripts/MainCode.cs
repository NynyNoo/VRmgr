using System;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;

public class MainCode : MonoBehaviour
{
    private UdpServer[] udpServers = new UdpServer[4];
    private int[] ports = { 12345, 12346, 12347, 12348 };

    async void Start()
    {
        await InitializeServers();
    }

    void OnDestroy()
    {
        Close();
    }

    private async Task InitializeServers()
    {
        Task[] tasks = new Task[ports.Length];

        for (int i = 0; i < ports.Length; i++)
        {
            udpServers[i] = new UdpServer(ports[i]);
            tasks[i] = udpServers[i].StartListening();
        }

        await Task.WhenAll(tasks);
    }

    public void Close()
    {
        foreach (var server in udpServers)
        {
            if (server != null)
                server.Close();
        }
    }
}
