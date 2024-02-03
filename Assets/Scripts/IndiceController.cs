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
        // Limitar el �ngulo a los rangos permitidos antes de aplicar la rotaci�n
        float limitedAngle = AngleUtils.LimitAngle(angle);
        // Aqu� se manejar�a la l�gica para rotar el dedo �ndice
        // Por ejemplo, aplicamos el �ngulo directamente a la rotaci�n local en X
        transform.localEulerAngles = new Vector3(limitedAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
