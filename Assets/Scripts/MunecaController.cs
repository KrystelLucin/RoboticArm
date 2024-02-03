using UnityEngine;

public class MunecaController : MonoBehaviour
{
    void OnEnable()
    {
        UDPReceive.OnWristAngleReceived += RotateWrist;
    }

    void OnDisable()
    {
        UDPReceive.OnWristAngleReceived -= RotateWrist;
    }

    private void RotateWrist(float angle)
    {
        // Asumimos que el ángulo recibido es en grados y queremos aplicarlo directamente
        transform.localEulerAngles = new Vector3(angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
