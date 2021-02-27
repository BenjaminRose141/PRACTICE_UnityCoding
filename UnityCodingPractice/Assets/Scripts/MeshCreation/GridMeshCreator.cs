using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridMeshCreator : MonoBehaviour
{
    [SerializeField] int xSize, ySize;

    private Vector3[] vertices;
    private Mesh mesh;


    private void Awake()
    {
        StartCoroutine(Generate());    
    }

    private IEnumerator Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        WaitForSeconds wait = new WaitForSeconds(0.1f);

        //Generate verts
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        for(int i = 0, y = 0; y <= ySize; y++)
        {
            for(int x = 0; x <= xSize; x++,i++)
            {
                vertices[i] = new Vector3(x, y);

                yield return wait;
            }
        }

        mesh.vertices = vertices;

        //Generate tris
        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for(int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
                mesh.triangles = triangles;
                yield return wait;
            }
        }
    }

    //Try Out OnDrawGizmosSelected
    private void OnDrawGizmos()
    {
        if(vertices == null)
        {
            return;
        }

        //Gizmos.color = Color.black;
        Color color = new Color(0, 0, 0);

        for(int i = 0; i < vertices.Length; i++)
        {
            color = new Color((float)i/vertices.Length, color.g, color.b);
            Gizmos.color = color;
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
