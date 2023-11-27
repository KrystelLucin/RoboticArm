using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class BrazoController : MonoBehaviour
{
    public GameObject muneca;
    public GameObject indice;
    public GameObject pulgar;

    private TcpListener listener;
    private const int PORT = 10000;
    private bool isRunning = true;

    void Start()
    {
        // Iniciar el servidor en un hilo aparte para no bloquear la UI de Unity
        listener = new TcpListener(IPAddress.Parse("127.0.0.1"), PORT);
        listener.Start();
        ListenForIncomingRequests();
    }

    private async void ListenForIncomingRequests()
    {
        try
        {
            while (isRunning)
            {
                using (var client = await listener.AcceptTcpClientAsync())
                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    var byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
                    var data = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    ProcessReceivedData(data);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Socket error: {e.Message}");
        }
    }

    private void ProcessReceivedData(string jsonData)
    {
        // Parse the JSON data
        var puntos = JsonUtility.FromJson<Puntos>(jsonData);

        // Assuming that puntos contains float values for position and/or rotation
        // Update the position and/or rotation of the robot parts
        if (puntos != null)
        {
            MoveRobotPart(muneca, puntos.muneca);
            MoveRobotPart(indice, puntos.indice);
            MoveRobotPart(pulgar, puntos.pulgar);
        }
    }

    private void MoveRobotPart(GameObject part, Punto punto)
    {
        if (part != null && punto != null)
        {
            // Aquí se mueve cada parte según las coordenadas recibidas
            // Esto puede implicar una transformación directa o una conversión basada en la cinemática del brazo
            part.transform.position = new Vector3(punto.x, punto.y, part.transform.position.z); // Ejemplo simple
            // También podrías necesitar rotar el objeto o aplicar otros cambios.
        }
    }

    void OnDisable()
    {
        // Detener el servidor cuando el objeto se deshabilite
        if (listener != null)
        {
            isRunning = false;
            listener.Stop();
        }
    }

    // Esta clase debe coincidir con la estructura JSON que estás enviando desde Python
    [Serializable]
    public class Puntos
    {
        public Punto muneca;
        public Punto indice;
        public Punto pulgar;
    }

    [Serializable]
    public class Punto
    {
        public float x;
        public float y;
    }
}
