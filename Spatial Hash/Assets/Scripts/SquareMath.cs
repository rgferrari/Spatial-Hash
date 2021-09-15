using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SquareMath
{
    // Calculates if the point is inside the square made by the vertices given
    public static bool IsInside(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, float multiplier = 1)
    {
        float a1 = CalculateTriangleArea(v0, v3, point);
        float a2 = CalculateTriangleArea(v3, v2, point);
        float a3 = CalculateTriangleArea(v2, v1, point);
        float a4 = CalculateTriangleArea(v1, v0, point);

        float squareArea = CalculateTriangleArea(v0, v1, v2) + CalculateTriangleArea(v1, v2, v3);

        if ((a1 + a2 + a3 + a4) < squareArea * multiplier)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static float CalculateTriangleArea(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float l1 = Vector3.Distance(p0, p1);
        float l2 = Vector3.Distance(p1, p2);
        float l3 = Vector3.Distance(p2, p0);

        float p = (l1 + l2 + l3) / 2;

        float a = Mathf.Sqrt(p * (p - l1) * (p - l2) * (p - l3));

        return a;
    }
}
