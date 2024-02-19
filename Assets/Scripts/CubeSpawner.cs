using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private CubeController _cubePrefab;
    [SerializeField] private LayerMask _sphereLayer;
    [SerializeField] private LayerMask _cubeLayer;
    [SerializeField] private Material _colorCube;
    [SerializeField] private float _timerBtwCubes;
    private List<CubeController> _cubeControllers = new List<CubeController>();

    private float _timer;

    private State _state;

    private void Start()
    {
        _state = State.Spawn;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (_state == State.Spawn)
            {
                Physics.Raycast(ray, out hit, 1000, _sphereLayer);

                if (_cubeControllers.Count > 0)
                {
                    if (Vector3.Distance(_cubeControllers[^1].transform.position, hit.point) > 0.2f)
                    {
                        CreateCube(hit);
                    }
                }
                else
                {
                    CreateCube(hit);
                }
            }
            else if (_state == State.Color)
            {
                Physics.Raycast(ray, out hit, 1000, _cubeLayer);
                if (hit.collider != null && hit.collider.TryGetComponent(out MeshRenderer mesh))
                {
                    mesh.sharedMaterial = _colorCube;
                }
            }
        }
    }

    private void CreateCube(RaycastHit hit)
    {

        CubeController newCube = Instantiate(_cubePrefab, hit.point, Quaternion.LookRotation(hit.point - transform.position).normalized);
        _cubeControllers.Add(newCube);
    }

    public void SwitchStateToColor()
    {
        _state = State.Color;
    }

    public void SwitchStateToSpawn()
    {
        _state = State.Spawn;
    }
}

[Serializable]
public enum State
{
    Color,
    Spawn
}