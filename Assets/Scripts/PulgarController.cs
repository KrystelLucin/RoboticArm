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
        // Aqu� se manejar�a la l�gica para rotar el dedo pulgar
        // Por ejemplo, aplicamos el �ngulo directamente a la rotaci�n local en X
        transform.localEulerAngles = new Vector3(angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
