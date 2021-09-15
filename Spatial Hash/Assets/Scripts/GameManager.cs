using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameManager : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject treeGenerator;
    public GameObject road;
    public GameObject grid;
    [ReadOnly] public int numTrees;
    [ReadOnly] public int numTreesOnHash;
    [ReadOnly] public int roadNumPoints;
    public bool hashOn;

    private TreeCollider treeCollider;

    // Start is called before the first frame update
    void Start()
    {
        treeCollider = treePrefab.GetComponent<TreeCollider>();
        numTrees = treeGenerator.GetComponent<TreeGenerator>().numTrees;
    }

    // Update is called once per frame
    void Update()
    {
        numTreesOnHash = grid.GetComponent<GridGenerator>().hashTable.Length;
        roadNumPoints = road.GetComponent<RoadCreator>().points.Length;
        if(hashOn){
            treeCollider.enabled = false;
            grid.SetActive(true);
        }else{
            treeCollider.enabled = true;
            grid.SetActive(false);
        }
    }
}
