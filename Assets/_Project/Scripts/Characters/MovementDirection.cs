using UnityEngine;

namespace Project.Characters
{
    public static class MovementDirection
    {
        public static Vector3 CartesianToIso(Vector2 direction)
        {
            if (Mathf.Approximately(direction.x, 0)
                && Mathf.Approximately(direction.y, 0))
            {
                return new Vector3();
            }

            float initialRadianAngle = Mathf.Atan2(direction.y, direction.x);
            float initialDegreeAngle = Mathf.Rad2Deg * initialRadianAngle;
            float newDegreeAngle = initialDegreeAngle + 45;
            float newRadianAngle = Mathf.Deg2Rad * newDegreeAngle;
            return new Vector3(Mathf.Cos(newRadianAngle), 0, Mathf.Sin(newRadianAngle));
        }
    }
}
