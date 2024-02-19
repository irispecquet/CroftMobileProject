using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private float _detectionRadius;
    [SerializeField] private float _initScale = 0.1f;
    [SerializeField] private float _scaleDuration;

    private void Start()
    {
        transform.DOScale(Vector3.one * _initScale, _scaleDuration);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}