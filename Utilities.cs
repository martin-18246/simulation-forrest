using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public static class Utilities
{
    public static Vector3 GenerateRandomLocationInRangeSquare(Vector3 center, double radius)
    {
        return new Vector3
        (
            Random.Range((float)(center.x - radius), (float)(center.x + radius)),
            0f,
            Random.Range((float)(center.z - radius), (float)(center.z + radius))
        );
    }

    public static Vector3 GenerateRandomLocationInCircle(Vector3 center, float radius)
    {
        float distFoodCenter = Random.Range(5, radius);
        float angleFood = Random.Range(0f, 360f);

        float xPos = Mathf.Sin(Mathf.Deg2Rad * angleFood) * distFoodCenter;
        float zPos = Mathf.Cos(Mathf.Deg2Rad * angleFood) * distFoodCenter;
        return new Vector3(center.x + xPos, 0, center.z + zPos);
    }

    public static Vector3 Flatten(this Vector3 vectorToFlatten) => new Vector3(vectorToFlatten.x, 0, vectorToFlatten.z);

    public static float GetAngleFromVectorFloat(Vector3 direction)
    {
        Vector3 directionNormalized = direction.normalized;
        float n = Mathf.Atan2(directionNormalized.y, directionNormalized.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public static float NormalDistribution(float meanFloat, float standardDeviation)
    {
        double meanDouble = (double)meanFloat;
        System.Random random = new System.Random();
        double u1 = random.NextDouble();
        double u2 = random.NextDouble();
        double z = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2);
        float result = (float)(meanDouble + standardDeviation * z);
        if (result < 0.3 * meanFloat) return 0.3f * (float)meanFloat;
        if (result > 3 * meanFloat) return 3f * (float)meanFloat;

        return result;
    }
}


