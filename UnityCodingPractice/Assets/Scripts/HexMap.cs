using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class HexMap : MonoBehaviour
{
    [SerializeField] GameObject hexPrefab;

    private void Start()
    {
        GenerateHexMap();
    }

    [Button]
    private void GenerateHexMap()
    {
        for(int column = 0; column < 10; column++)
        {
            for(int row = 0; row < 10; row++)
            {
                Hex h = new Hex(column, row);
                Instantiate(hexPrefab, h.Position(), Quaternion.identity, this.transform);
            }
        }

        Debug.Log("Generated Hex Map");
    }
}
