using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class HomeBase : NetworkBehaviour
{
    public NetworkVariable<int> mineralAmount = new();
    public NetworkVariable<int> crystalAmount = new();
    public NetworkVariable<int> gasAmount = new();

    [SerializeField] private Transform _laserStartPoint;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _badRockPrefab;
    private GameObject _currentAsteroid;
    private LineRenderer _lineRenderer;
    private HomeBaseState _homeBaseState;
    public enum HomeBaseState
    {
        Shooting,
        RotatingRight,
        RotatingLeft
    }

    private void Awake()
    {
        ReferenceManager.homeBase = this;
    }

    private void Start()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _lineRenderer = Instantiate(_laserPrefab, _laserStartPoint.position, Quaternion.identity).GetComponent<LineRenderer>();
        UpdateLaserPositions();
    }

    private void UpdateLaserPositions()
    {
        _lineRenderer.SetPosition(0, _laserStartPoint.position);
        if (_currentAsteroid != null)
        {
            _lineRenderer.SetPosition(1, _currentAsteroid.transform.position);
        }
        else
        {
            _lineRenderer.SetPosition(1, _laserStartPoint.position);
        }
    }

    private void SpawnAsteroid()
    {
        if (!IsOwner)
        {
            throw new System.Exception("Only the owner can spawn an asteroid");
        }
        Vector3 spawnPosition = new Vector3(50f, 0f, 0f);
        // rotate spawnPosition by a random angle between 0 and 180 degrees
        spawnPosition = Quaternion.Euler(0f, Random.Range(0f, 180f), 0f) * spawnPosition;
        spawnPosition += transform.position;
        _currentAsteroid = Instantiate(_badRockPrefab, spawnPosition, Quaternion.identity);
        _currentAsteroid.GetComponent<NetworkObject>().Spawn();
    }
}
