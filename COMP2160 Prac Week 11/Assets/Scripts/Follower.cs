using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform target2;
    [SerializeField] private float gizmoRadius;
    [SerializeField] private float proportionBetween;
    private Vector3 midpoint;

    void Start()
    {
        if(target2 == null)
        {
            Debug.Log("No Second Target");
        }
    }

    void Update()
    {
        if(target2 == null)
        {
            transform.position = target.position;
        }
        else
        {
            midpoint = new Vector3(target.position.x + (target2.position.x - target.position.x)/proportionBetween, 
            0, target.position.z + (target2.position.z - target.position.z)/proportionBetween);
            transform.position = midpoint;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
