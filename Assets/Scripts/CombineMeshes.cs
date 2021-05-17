using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineMeshes : MonoBehaviour
{
    void Start()
    {
        Execute();
    }
    void Execute()
    {
        MeshFilter[] meshFilterArray = GetComponentsInChildren<MeshFilter>();
        List<MeshFilter> meshFilters = new List<MeshFilter>();
        
        for(int i = 1; i < meshFilterArray.Length; i++)
            meshFilters.Add(meshFilterArray[i]);


        CombineInstance[] combine = new CombineInstance[meshFilters.Count];

        int j = 0;
        while (j < meshFilters.Count)
        {
            combine[j].mesh = meshFilters[j].sharedMesh;
            combine[j].transform = meshFilters[j].transform.localToWorldMatrix;
            meshFilters[j].gameObject.SetActive(false);
            j++;
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);

        Material cellMat = Resources.Load<Material>("CellMaterial");
    }
}
