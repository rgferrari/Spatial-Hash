using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
	public GameObject tree;
	public float radius = 1;
	public Vector2 regionSize = new Vector2(10,10);

	private int rejectionSamples = 30; // Número de tentativas para cada árvore

	[HideInInspector]
	public List<Vector2> points;

	[HideInInspector]
	public GameObject[] trees;

	[HideInInspector]
	public int numTrees;

	void Awake()
	{
		points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
		InstantiateTrees();
		gameObject.transform.position -= new Vector3(regionSize.x / 2, 0, regionSize.y / 2);
	}

	private void InstantiateTrees()
    {
		trees = new GameObject[points.Count];
		if (points != null)
		{
			int i = 0;
			foreach (Vector2 point in points)
			{
				GameObject obj = tree;
				//GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
				//obj.transform.localScale = new Vector3(10, 10, 10);
				trees[i] = Instantiate(obj, new Vector3(point.x, obj.transform.position.y, point.y), Quaternion.identity, gameObject.transform);
				i++;
			}
		}
		numTrees = trees.Length;
	}
}