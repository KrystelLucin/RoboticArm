using UnityEngine;

public class IndiceController : MonoBehaviour
{
    void OnEnable()
    {
        UDPReceive.OnIndexAngleReceived += RotateIndex;
    }

    void OnDisable()
    {
        UDPReceive.OnIndexAngleReceived -= RotateIndex;
    }

    private void RotateIndex(float angle)
    {
        // Aqu� se manejar�a la l�gica para rotar el dedo �ndice
        // Por ejemplo, aplicamos el �ngulo directamente a la rotaci�n local en X
        transform.localEulerAngles = new Vector3(angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
