using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathCreator))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoadCreator : MonoBehaviour
{
    public Vector3[] trianglePoints;

    [Range(15f, 100f)]
    public float spacing = 5f;
    public float roadWidth = 1;
    public bool autoUpdate;
    public float tiling = 1;

    [HideInInspector]
    public Vector3[] points;

    public void UpdateRoad()
    {
        Path path = GetComponent<PathCreator>().path;
        points = path.CalculateEvenlySpacedPoints(spacing);
        GetComponent<MeshFilter>().mesh = CreateRoadMesh(points);

        int textureRepeat = Mathf.RoundToInt(tiling * points.Length * spacing * .05f);
        GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector3(1, textureRepeat);
    }

    Mesh CreateRoadMesh(Vector3[] points)
    {
        Vector3[] verts = new Vector3[points.Length * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int numTris = 2 * (points.Length - 1);
        int[] tris = new int[numTris * 3];
        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 forward = Vector3.zero;
            if (i < points.Length - 1)
            {
                forward += points[(i + 1) % points.Length] - points[i];
            }
            if (i > 0)
            {
                forward += points[i] - points[(i - 1 + points.Length) % points.Length];
            }

            forward.Normalize();
            Vector3 left = new Vector3(-forward.z, forward.y, forward.x);

            verts[vertIndex] = points[i] + left * roadWidth * .5f;
            verts[vertIndex + 1] = points[i] - left * roadWidth * .5f;

            float completionPercent = i / (float)(points.Length - 1);
            float v = 1 - Mathf.Abs(2 * completionPercent - 1);
            uvs[vertIndex] = new Vector2(0, v);
            uvs[vertIndex + 1] = new Vector2(1, v);

            if (i < points.Length - 1)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 5] = (vertIndex + 3) % verts.Length;
            }

            vertIndex += 2;
            triIndex += 6;
        }

        trianglePoints = verts;

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        return mesh;
    }

        void OnDrawGizmos()
    {
        if (trianglePoints == null)
            return;

        for (int i = 0; i < trianglePoints.Length; i++)
        {
            Gizmos.DrawSphere(trianglePoints[i], 5f);

            if(i < trianglePoints.Length - 1){
                if (trianglePoints.Length > 0)
                {
                    Vector3 p0 = trianglePoints[i];
                    Vector3 p1 = trianglePoints[i + 1];

                    Gizmos.DrawLine(p0, p1);
                }
            }
        }
    }
}
