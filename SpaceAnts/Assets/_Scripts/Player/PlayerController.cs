using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Cinemachine;
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _rotationSpeed = 10.0f;
    private Rigidbody _rb;
    private Vector2 _movementInput;
    private ParticleManager _particleManager;
    void Start()
    {
        if (!IsOwner)
        {
            Destroy(GetComponentInChildren<CinemachineVirtualCamera>().gameObject);
            enabled = false;
        }

        ReferenceManager.playerController = this;
        ReferenceManager.PlayerSpawned();
        EventManager.SubscribeAction("Move", OnMove);
        _rb = GetComponent<Rigidbody>();
        _particleManager = GetComponent<ParticleManager>();
    }
    private void FixedUpdate()
    {

        if (_movementInput != Vector2.zero)
        {
            AddMovementForce();
        }
    }

    private void AddMovementForce()
    {
        // add rotation force according to _movementInput.x
        _rb.AddTorque(Vector3.up * _movementInput.x * _rotationSpeed);
        // add movement force according to _movementInput.y
        _rb.AddForce(-transform.forward * _movementInput.y * _speed);
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("HomeBase"))
        other.gameObject.GetComponent<TestBase>().AddScoreServerRpc(1);
    }
    // [ServerRpc]
    // private void UpdateBoosterVariables_ServerRPC()
    // {
    //     if (_movementInput.y < 0)
    //     {
    //         _particleManager.BackBoosterStrength.Value = Mathf.Abs(_movementInput.y);
    //         _particleManager.FrontBoosterStrength.Value = 0;
    //     }
    //     else
    //     {
    //         _particleManager.FrontBoosterStrength.Value = _movementInput.y;
    //         _particleManager.BackBoosterStrength.Value = 0;
    //     }
    //     if (_movementInput.x < 0)
    //     {
    //         _particleManager.RightBoosterStrength.Value = Mathf.Abs(_movementInput.x);
    //         _particleManager.LeftBoosterStrength.Value = 0;
    //     }
    //     else
    //     {
    //         _particleManager.LeftBoosterStrength.Value = _movementInput.x;
    //         _particleManager.RightBoosterStrength.Value = 0;
    //     }
    // }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movementInput = context.ReadValue<Vector2>();
            _particleManager.UpdateVariables(_movementInput);
        }
        if (context.canceled)
        {
            _movementInput = Vector2.zero;
            _particleManager.UpdateVariables(_movementInput);
        }
    }

}
