using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridCreator : MonoBehaviour
{
    [SerializeField] int xSize, ySize;

    private Vector3[] vertices;

    private void Awake()
    {
        StartCoroutine(Generate());    
    }

    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        for(int i = 0, y = 0; y <= ySize; y++)
        {
            for(int x = 0; x <= xSize; x++,i++)
            {
                vertices[i] = new Vector3(x, y);

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
