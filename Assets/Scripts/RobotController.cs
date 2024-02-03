using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public GameObject muneca; // Este es el GameObject de la mu�eca

    private TcpListener listener;
    private const int PORT = 10000;
    private bool isRunning = true;
    private float angleToRotate = 0f; // �ngulo recibido para la rotaci�n
    private bool shouldRotate = false; // Controla si debe ocurrir una rotaci�n

    void Start()
    {
        // Iniciar el servidor en un hilo aparte para no bloquear la UI de Unity
        listener = new TcpListener(IPAddress.Parse("127.0.0.1"), PORT);
        listener.Start();
        ListenForIncomingRequests();
    }

    void Update()
    {
        // Verificar si hay una nueva rotaci�n para aplicar
        if (shouldRotate)
        {
            // Aplica la rotaci�n en el eje X
            muneca.transform.localEulerAngles = new Vector3(angleToRotate, muneca.transform.localEulerAngles.y, muneca.transform.localEulerAngles.z);
            shouldRotate = false; // Restablece la bandera
        }
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
        float newAngle = JsonUtility.FromJson<float>(jsonData);

        // Asegurarse de que los datos son v�lidos
        if (!float.IsNaN(newAngle))
        {
            // Establece el �ngulo para la rotaci�n y activa la bandera
            angleToRotate = newAngle;
            shouldRotate = true;
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
}
