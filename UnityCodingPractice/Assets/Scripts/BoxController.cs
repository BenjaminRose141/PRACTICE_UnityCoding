using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class BoxController : MonoBehaviour
{
    [SerializeField] float speed = 90.0f;
    [SerializeField] bool useBuffering = false;

    private Transform parent = null;
    private Vector3[] directions = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    private Quaternion qTo;
    private bool rotating;
    private bool buffered = false;
    private Vector3 bufferedDirection = Vector3.zero;

    [Foldout("Dimensions Per Direction")]
    [SerializeField] float up;
    [Foldout("Dimensions Per Direction")]
    [SerializeField] float down;
    [Foldout("Dimensions Per Direction")]
    [SerializeField] float left;
    [Foldout("Dimensions Per Direction")]
    [SerializeField] float right;
    [Foldout("Dimensions Per Direction")]
    [SerializeField] float forward;
    [Foldout("Dimensions Per Direction")]
    [SerializeField] float back;


    private void Start()
    {
        parent = new GameObject().transform;
    }

    private void Update()
    {
        var inputDirection = DirectionFromInput();

        if (rotating)
        {
            if(inputDirection != Vector3.zero)
            {
                bufferedDirection = inputDirection;
                buffered = true;
            }

            return;
        }
        
        if(inputDirection != Vector3.zero)
        {
            FlopTo(inputDirection);
        }
        else if(buffered)
        {
            buffered = false;
            FlopTo(bufferedDirection);
        }
    }


    private Vector3 DirectionFromInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            return Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            return Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            return Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            return Vector3.back;
        }
        else return Vector3.zero;
    }

    private void FlopTo(Vector3 direction)
    {
        var down = WhatsDown();

        var dir = transform.InverseTransformDirection(direction);

        var pos = dir * DimensionFromDirection(dir) + down * DimensionFromDirection(down);

        pos = transform.TransformPoint(pos);

        var axis = Vector3.Cross(Vector3.down, direction);


        transform.parent = null;
        parent.rotation = Quaternion.identity;
        parent.position = pos;
        transform.parent = parent;

        qTo = Quaternion.AngleAxis(-90.0f, axis);

       
        StartCoroutine(Rotate());
    }

    private float DimensionFromDirection(Vector3 direction)
    {
        float dimension = 0f;

        if (direction == Vector3.up)
        {
            dimension = up;
        }
        else if(direction == Vector3.down)
        {
            dimension = down;
        }
        else if (direction == Vector3.left)
        {
            dimension = left;
        }
        else if (direction == Vector3.right)
        {
            dimension = right;
        }
        else if (direction == Vector3.forward)
        {
            dimension = forward;
        }
        else if (direction == Vector3.back)
        {
            dimension = back;
        }

        return dimension;
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
        transform.position = new Vector3(Mathf.Round(transform.position.x*100f)/100f, Mathf.Round(transform.position.y*100f)/100f, Mathf.Round(transform.position.z*100f)/100f);
    }
}