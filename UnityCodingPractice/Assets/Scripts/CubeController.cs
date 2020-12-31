using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] float speed = 90.0f;

    private Transform parent = null;
    private Vector3[] directions = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    private Quaternion qTo;
    private bool rotating;

    private void Start()
    {
        parent = new GameObject().transform;
    }

    private void Update()
    {
        if(rotating)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            FlopTo(Vector3.left);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            FlopTo(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            FlopTo(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            FlopTo(Vector3.back);
        }
    }

    private void FlopTo(Vector3 direction)
    {
        var down = WhatsDown();

        var dir = transform.InverseTransformDirection(direction);

        var pos = dir * 0.5f + down * 0.5f;

        pos = transform.TransformPoint(pos);

        var axis = Vector3.Cross(Vector3.down, direction);


        transform.parent = null;
        parent.rotation = Quaternion.identity;
        parent.position = pos;
        transform.parent = parent;

        qTo = Quaternion.AngleAxis(-90.0f, axis);

       
        StartCoroutine(Rotate());
    }

    private Vector3 WhatsDown()
    {
        for (var i = 0; i < directions.Length; i++)
        {
            if (Vector3.Dot(transform.TransformDirection(directions[i]), Vector3.down) > 0.9)
            {
                return directions[i];
            }
        }

        Debug.LogError("Direction Error");
        return directions[0];
    }

    IEnumerator Rotate()
    {
        rotating = true;

        while (parent.rotation != qTo)
        {
            parent.rotation = Quaternion.RotateTowards(parent.rotation, qTo, speed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Parent rot " + parent.rotation + " qTo: " + qTo);

        Cleanup();
        rotating = false;
    }

    //Cleans up all the unfortunate deviations resulting from rotations and parenting
    private void Cleanup()
    {
        var currentRotation = parent.eulerAngles;
        currentRotation = new Vector3(Mathf.Round(qTo.eulerAngles.x), Mathf.Round(qTo.eulerAngles.y), Mathf.Round(qTo.eulerAngles.z));
        parent.eulerAngles = currentRotation;
        transform.parent = null;
        transform.localScale = Vector3.one;
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
    }
}