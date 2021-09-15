using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCollider : MonoBehaviour
{
    bool collidedWithRoad = false;
    GameObject[] roads;
    private void Update()
    {
        roads = GameObject.FindGameObjectsWithTag("Road");

        GameObject road = roads[0];

        RoadCreator roadCreator = road.GetComponent<RoadCreator>();

        Vector3[] roadTriangles = roadCreator.trianglePoints;

        for (int i = 0; i < roadTriangles.Length - 2; i += 2)
        {
            Vector3 p0 = roadTriangles[i + 0];
            Vector3 p1 = roadTriangles[i + 1];
            Vector3 p2 = roadTriangles[i + 2];
            Vector3 p3 = roadTriangles[i + 3];

            Vector3 actualPosition = gameObject.transform.position;

            collidedWithRoad = SquareMath.IsInside(actualPosition, p0, p1, p2, p3, 1.5f);

            if (collidedWithRoad)
                break;
        }
        
        if (collidedWithRoad)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        if (!collidedWithRoad)
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
    }
}
