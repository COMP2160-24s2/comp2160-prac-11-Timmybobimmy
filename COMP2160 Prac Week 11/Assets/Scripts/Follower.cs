using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float gizmoRadius;

    void Update()
    {
        transform.position = target.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
