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
        // Limitar el ángulo a los rangos permitidos antes de aplicar la rotación
        float limitedAngle = AngleUtils.LimitAngle(angle);

        // Aplicar la rotación limitada
        transform.localEulerAngles = new Vector3(limitedAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
