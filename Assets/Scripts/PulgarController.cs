using UnityEngine;

public class PulgarController : MonoBehaviour
{
    void OnEnable()
    {
        UDPReceive.OnThumbAngleReceived += RotateThumb;
    }

    void OnDisable()
    {
        UDPReceive.OnThumbAngleReceived -= RotateThumb;
    }

    private void RotateThumb(float angle)
    {
        // Aquí se manejaría la lógica para rotar el dedo pulgar
        // Por ejemplo, aplicamos el ángulo directamente a la rotación local en X
        transform.localEulerAngles = new Vector3(angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
