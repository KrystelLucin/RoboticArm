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
        // Limitar el �ngulo a los rangos permitidos antes de aplicar la rotaci�n
        float limitedAngle = AngleUtils.LimitAngle(angle);

        // Aplicar la rotaci�n limitada
        transform.localEulerAngles = new Vector3(limitedAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
