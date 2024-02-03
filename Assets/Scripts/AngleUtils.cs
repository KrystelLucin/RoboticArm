using System;

public static class AngleUtils
{
    public static float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle < 0) angle += 360;
        return angle;
    }

    public static float LimitAngle(float angle)
    {
        float normalizedAngle = NormalizeAngle(angle);

        if (normalizedAngle > 90 && normalizedAngle < 270)
        {
            if (Math.Abs(normalizedAngle - 90) < Math.Abs(normalizedAngle - 270))
            {
                angle = (angle < 0) ? -90 : 90;
            }
            else
            {
                angle = (angle < 0) ? -270 : 270;
            }
        }

        return angle;
    }
}
