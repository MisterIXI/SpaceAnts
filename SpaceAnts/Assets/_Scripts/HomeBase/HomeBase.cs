using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class HomeBase : NetworkBehaviour
{
    public NetworkVariable<int> mineralAmount = new();
    public NetworkVariable<int> crystalAmount = new();
    public NetworkVariable<int> gasAmount = new();
    public NetworkVariable<HomeBaseState> homeBaseState = new();
    [SerializeField] private Transform _laserStartPoint;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _badRockPrefab;
    private GameObject _currentAsteroid;
    private LineRenderer _lineRenderer;
    private float _damagePerSecond = 5f;
    private float _rotationTargetTime;
    private float _rotationStartTime;
    private float _rotationStartEuler;
    private float _rotationTargetEuler;
    private const float ROTATION_PER_SECOND = 50f;
    public enum HomeBaseState
    {
        Idle,
        Shooting,
        Rotating
    }

    private void Awake()
    {
        ReferenceManager.homeBase = this;
    }

    public override void OnNetworkSpawn()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _lineRenderer = Instantiate(_laserPrefab, _laserStartPoint.position, Quaternion.identity).GetComponent<LineRenderer>();
        UpdateLaserPositions();
        homeBaseState.OnValueChanged += OnStateChange;
        if (IsOwner)
        {
            SpawnAsteroid();
            homeBaseState.Value = HomeBaseState.Rotating;
        }
    }

    private void FixedUpdate()
    {
        if (IsSpawned)
        {
            if (IsOwner)
            {
                if (homeBaseState.Value == HomeBaseState.Shooting)
                {
                    if (_currentAsteroid != null)
                    {
                        _currentAsteroid.GetComponent<BadRockMovement>().TakeDamage(_damagePerSecond * Time.fixedDeltaTime);
                    }
                }
                else if (homeBaseState.Value == HomeBaseState.Rotating)
                {
                    RotateToAsteroid();
                }
            }
            UpdateLaserPositions();
        }
    }
    private void OnStateChange(HomeBaseState oldState, HomeBaseState newState)
    {
        if (IsServer)
        {
            Debug.Log($"HomeBaseState changed from {oldState} to {newState}");
            if (newState == HomeBaseState.Rotating)
            {
                _rotationStartEuler = transform.eulerAngles.y;
                _rotationTargetEuler = Quaternion.LookRotation(_currentAsteroid.transform.position - transform.position).eulerAngles.y;
                _rotationStartTime = Time.time;
                float angle = Mathf.Abs(_rotationTargetEuler - _rotationStartEuler);
                if (angle > 180)
                {
                    angle = 360 - angle;
                }
                _rotationTargetTime = Time.time + angle / ROTATION_PER_SECOND;
                Debug.Log($"_rotationStartEuler: {_rotationStartEuler}, _rotationTargetEuler: {_rotationTargetEuler}, _rotationStartTime: {_rotationStartTime}, _rotationTargetTime: {_rotationTargetTime}");
            }
        }
    }

    private void RotateToAsteroid()
    {
        float rotationT = Mathf.InverseLerp(_rotationStartTime, _rotationTargetTime, Time.time);
        if (Time.time >= _rotationTargetTime)
        {
            rotationT = 1f;
            homeBaseState.Value = HomeBaseState.Shooting;
        }
        float newEulerY = Mathf.LerpAngle(_rotationStartEuler, _rotationTargetEuler, rotationT);
        transform.rotation = Quaternion.Euler(0f, newEulerY, 0f);
    }
    public void OnAsteroidSpawn_ServerOnly(GameObject asteroid)
    {
        _currentAsteroid = asteroid;
        homeBaseState.Value = HomeBaseState.Rotating;
    }

    public void OnAsteroidDespawn_ServerOnly()
    {
        _currentAsteroid = null;
        homeBaseState.Value = HomeBaseState.Idle;
        SpawnAsteroid();
    }
    private void UpdateLaserPositions()
    {
        _lineRenderer.SetPosition(0, _laserStartPoint.position);
        if (_currentAsteroid != null && homeBaseState.Value == HomeBaseState.Shooting)
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
        Vector3 spawnPosition = new Vector3(0f, 0f, 150f);
        // rotate spawnPosition by a random angle between 0 and 180 degrees
        spawnPosition = Quaternion.Euler(0f, Random.Range(-90f, 90f), 0f) * spawnPosition;
        spawnPosition += transform.position;
        _currentAsteroid = Instantiate(_badRockPrefab, spawnPosition, Quaternion.identity);
        _currentAsteroid.GetComponent<NetworkObject>().Spawn();
        Debug.Log("Spawned asteroid at " + spawnPosition);
    }

}
