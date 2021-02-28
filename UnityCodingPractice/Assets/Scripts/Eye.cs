using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    [SerializeField] Transform lookTarget;
    [SerializeField] float lookSpeed = 0.01f;
    [SerializeField] float radius = 5f;
    [SerializeField] float lightDistance = 3f;

    private bool moving = false;
    private Vector3 point = Vector3.zero;
    private Vector3 startPoint = Vector3.zero;

    private void Awake()
    {
        transform.LookAt(lookTarget);
        CheckView();
        startPoint = lookTarget.position;
    }

    private void Update()
    {
        transform.LookAt(lookTarget);

        

        if (!moving)
        {
            //Find a point
            bool validPoint = false;
            int n = 0;
            while(!validPoint)
            {
                Vector3 offset = (Vector3)(radius * Random.insideUnitCircle.normalized);
                point = lookTarget.position + new Vector3(offset.x, 0f, offset.y);
                validPoint = TestPoint(point);

                if (!validPoint)
                {
                    point = lookTarget.position - offset;
                    validPoint = TestPoint(point);
                }

                n++;

                if(n >= 20)
                {
                    n = 0;
                    point = startPoint;
                    validPoint = true;
                }
            }
            
            moving = true;

            Debug.Log("current: " + lookTarget.position + " target: " + point);
        }
        else
        {
            lookTarget.position = Vector3.Lerp(lookTarget.position, point, lookSpeed * Time.deltaTime);

            Debug.Log("MOVING");
            if((lookTarget.position - point).magnitude <= 0.1f)
            {
                CheckView();
                moving = false;                
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(lookTarget.position, 0.6f);
        Debug.DrawRay(transform.position, lookTarget.position - transform.position, Color.green);
    }

    private void CheckView()
    {
        Debug.Log("CHECKING");
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, lookTarget.position - transform.position, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("hit ground");
                lookTarget.position = hit.point;
            }
            else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.LogWarning("ATTACK");
            }
        }
        else
        {
            Debug.Log("Didnt hit");
            point = startPoint;
        }
    }

    private bool TestPoint(Vector3 point)
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, lookTarget.position - transform.position, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("Hit Ground");
                return true;
            }
        }

        return false;
    }
}