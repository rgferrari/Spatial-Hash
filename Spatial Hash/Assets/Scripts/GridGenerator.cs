using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject treeGenerator;
    public GameObject road;
    public int cellSide = 100;
    [ReadOnly]
    public GameObject[] hashTable; // Guardas as árvores com os indexes guardados pelo pivots

    private Vector3 startingPoint;
    private List<Vector3> vertices;
    private Vector3[] pivots;
    private GameObject[] trees;
    private Vector3[] trianglePoints;

    int xSize = 1000;
    int zSize = 1000;
    int numCells;

    // Start is called before the first frame update
    void Start()
    {
        numCells = (xSize / cellSide) * (zSize / cellSide);

        vertices = new List<Vector3>();

        trees = treeGenerator.GetComponent<TreeGenerator>().trees; // Pega uma lista de todos os gameObjects de árvores  

        startingPoint = gameObject.transform.position;

        for (int z = 0; z <= zSize / cellSide; z++)
        {
            for (int x = 0; x <= xSize / cellSide; x++)
            {
                Vector3 vertex = new Vector3(startingPoint.x + cellSide * x, 0, startingPoint.z + cellSide * z);
                vertices.Add(vertex);
            }
        }
        AddPivots();
    }

    // Update is called once per frame
    void Update()
    {
        trianglePoints = road.GetComponent<RoadCreator>().trianglePoints;
        CellCollider();
    }

    void AddPivots()
    {
        hashTable = new GameObject[trees.Length];

        int hashTableIndex = 0;
        pivots = new Vector3[numCells];

        int vert = 0;
        int pivotIndex = 0;
        for (int z = 0; z < zSize / cellSide; z++)
        {
            for (int x = 0; x < xSize / cellSide; x++)
            {
                // For painting the objects
                Vector3 draw = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                Color color = Color.black;

                color = new Color(draw.x, draw.y, draw.z);

                int count = 0;
                for (int i = 0; i < trees.Length; i++)
                {
                    Vector3 p0 = vertices[vert + 0];
                    Vector3 p1 = vertices[vert + 1];
                    Vector3 p2 = vertices[vert + (xSize / cellSide) + 1];
                    Vector3 p3 = vertices[vert + (xSize / cellSide) + 2];

                    Vector3 actualPosition = trees[i].transform.position;

                    bool isInsideCell = true;

                    //bool isInsideCell = SquareMath.IsInside(actualPosition, p0, p1, p2, p3);
                    for( int j = 0 ; j < 3; j += 2){
                        if( actualPosition[j] < p0[j] || actualPosition[j] > p3[j]){
                            isInsideCell = false;
                            break;
                        }
                    }

                    if (isInsideCell)
                    {
                        trees[i].GetComponent<Renderer>().material.SetColor("_Color", color);
                        hashTable[hashTableIndex] = trees[i];
                        hashTableIndex++;
                        count++;
                    }
                }

                pivots[pivotIndex].x = count;
                pivots[pivotIndex].y = hashTableIndex - count;
                pivots[pivotIndex].z = hashTableIndex;
                //Debug.Log("pivot " + pivotIndex + ": " + count);

                vert++;
                pivotIndex++;
            }
            vert++;
        }
    }

    void CellCollider()
    {
        int vert = 0;
        int pivotIndex = 0;
        for (int z = 0; z < zSize / cellSide; z++)
        {
            for (int x = 0; x < xSize / cellSide; x++)
            {
                for (int i = 0; i < trianglePoints.Length; i++)
                {
                    Vector3 p0 = vertices[vert + 0];
                    Vector3 p1 = vertices[vert + 1];
                    Vector3 p2 = vertices[vert + xSize / cellSide + 1];
                    Vector3 p3 = vertices[vert + xSize / cellSide + 2];
                    
                    Vector3 actualPosition = trianglePoints[i];

                    bool collidedWithRoad = SquareMath.IsInside(actualPosition, p0, p1, p2, p3);

                    if (collidedWithRoad)
                    {
                        float initial_index = pivots[pivotIndex].y;
                        float final_index = pivots[pivotIndex].z;

                        for (int j = (int)initial_index; j < final_index; j++){
                            Vector3 treePosition = hashTable[j].transform.position;
                            
                            for(int k = 0; k < trianglePoints.Length - 2; k+=2)
                            {
                                Vector3 v0 = trianglePoints[k + 0];
                                Vector3 v1 = trianglePoints[k + 1];
                                Vector3 v2 = trianglePoints[k + 2];
                                Vector3 v3 = trianglePoints[k + 3];

                                bool treeCollided = SquareMath.IsInside(treePosition, v0, v1, v2, v3, 1.5f);

                                if(treeCollided){
                                    hashTable[j].GetComponent<Renderer>().enabled = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                    else
                    {
                        float initial_index = pivots[pivotIndex].y;
                        float final_index = pivots[pivotIndex].z;

                        for (int j = (int)initial_index; j < final_index; j++)
                        {
                            hashTable[j].GetComponent<Renderer>().enabled = true;
                        }
                    }
                }
                vert++;
                pivotIndex++;
            }
            vert++;
        }
    }

    void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Count; i++)
        {
            Gizmos.DrawSphere(vertices[i], 5f);
        }

        int vert = 0;
        for (int i = 0; i < zSize / cellSide; i++)
        {
            for (int j = 0; j < xSize / cellSide; j++)
            {
                if (vertices.Count > 0)
                {
                    Vector3 p0 = vertices[vert + 0];
                    Vector3 p1 = vertices[vert + 1];
                    Vector3 p2 = vertices[vert + xSize / cellSide + 1];
                    Vector3 p3 = vertices[vert + xSize / cellSide + 2];

                    Gizmos.DrawLine(p0, p2);
                    Gizmos.DrawLine(p2, p3);
                    Gizmos.DrawLine(p3, p1);
                    Gizmos.DrawLine(p1, p0);
                }
                vert++;
            }
            vert++;
        }
    }
}
