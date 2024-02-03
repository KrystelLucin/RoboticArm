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
        // Limitar el ángulo a los rangos permitidos antes de aplicar la rotación
        float limitedAngle = AngleUtils.LimitAngle(angle);
        // Aquí se manejaría la lógica para rotar el dedo pulgar
        // Por ejemplo, aplicamos el ángulo directamente a la rotación local en X
        transform.localEulerAngles = new Vector3(limitedAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
