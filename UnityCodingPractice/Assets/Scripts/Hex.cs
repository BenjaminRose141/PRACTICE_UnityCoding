using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds information of a Hex tile (grid position, world position, size, neighbours etc.).
/// </summary>

public class Hex
{
    public Hex(int q, int r)
    {
        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }

    // Q + R + S = 0
    // S = -(Q + R)

    public readonly int Q;  //Column
    public readonly int R;  //Row
    public readonly int S;

    static readonly float widthMultiplier = Mathf.Sqrt(3f) / 2f;

    //Returns world space position of this hex
    public Vector3 Position()
    {
        float radius = 0.5f;
        float height = radius * 2f;
        float width = widthMultiplier * height;

        float horizontal = width;
        float vertical = height * 0.75f;

        //Debug.Log("Info: rad: " + radius + " , height: " + height + " width: " + width + " hor: " + horizontal + " vert: " + vertical);

        return new Vector3(horizontal * (this.Q + this.R/2f), 0f, vertical * this.R);
    }
}