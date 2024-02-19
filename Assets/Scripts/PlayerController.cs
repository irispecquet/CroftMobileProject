using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _raySize;
    [SerializeField] private Transform _rayOrigin;
    
    private Touch _touch;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rayOrigin.position, _rayOrigin.position + _raySize * transform.forward);
    }
}