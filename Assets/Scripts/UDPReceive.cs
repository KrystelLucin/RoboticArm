using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    public int port = 5052;
    public static event Action<float> OnWristAngleReceived;
    public static event Action<float> OnIndexAngleReceived;
    public static event Action<float> OnThumbAngleReceived;

    void Start()
    {
        UnityThread.Init(); // Asegúrate de que UnityThread ha sido inicializado.
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                string textData = Encoding.UTF8.GetString(data);
                // Asumiendo que textData es una cadena JSON con el formato:
                // {"wrist": valor, "index": valor, "thumb": valor}
                UnityThread.ExecuteInUpdate(() =>
                {
                    try
                    {
                        HandAngles angles = JsonUtility.FromJson<HandAngles>(textData);
                        OnWristAngleReceived?.Invoke(angles.wrist);
                        OnIndexAngleReceived?.Invoke(angles.index);
                        OnThumbAngleReceived?.Invoke(angles.thumb);
                    }
                    catch (Exception jsonEx)
                    {
                        Debug.LogError("JSON Parse error: " + jsonEx.Message);
                    }
                });
            }
            catch (Exception err)
            {
                Debug.LogError(err.ToString());
                client?.Close();
                break;
            }
        }
    }

    void OnDisable()
    {
        receiveThread?.Abort();
        client?.Close();
    }
}

[Serializable]
public class HandAngles
{
    public float wrist;
    public float index;
    public float thumb;
}
