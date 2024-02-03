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
        // Limitar el ángulo a los rangos permitidos antes de aplicar la rotación
        float limitedAngle = AngleUtils.LimitAngle(angle);
        // Aquí se manejaría la lógica para rotar el dedo índice
        // Por ejemplo, aplicamos el ángulo directamente a la rotación local en X
        transform.localEulerAngles = new Vector3(limitedAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
