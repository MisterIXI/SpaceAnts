using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ParticleManager : NetworkBehaviour
{

    [HideInInspector] public float BackBoosterStrength;
    [HideInInspector] public float FrontBoosterStrength;
    [HideInInspector] public float LeftBoosterStrength;
    [HideInInspector] public float RightBoosterStrength;

    [SerializeField] private ParticleSystem BackBooster;
    [SerializeField] private ParticleSystem FrontBooster;
    [SerializeField] private ParticleSystem LeftBooster;
    [SerializeField] private ParticleSystem RightBooster;

    private float _backBoosterCurrentSpeed;
    private float _frontBoosterCurrentSpeed;
    private float _leftBoosterCurrentSpeed;
    private float _rightBoosterCurrentSpeed;

    private void Start()
    {
        // BackBoosterStrength = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        // FrontBoosterStrength = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        // LeftBoosterStrength = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        // RightBoosterStrength = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        // get the current start speed of the particle system
        _backBoosterCurrentSpeed = BackBooster.main.startSpeed.constant;
        _frontBoosterCurrentSpeed = FrontBooster.main.startSpeed.constant;
        _leftBoosterCurrentSpeed = LeftBooster.main.startSpeed.constant;
        _rightBoosterCurrentSpeed = RightBooster.main.startSpeed.constant;
    }

    [ClientRpc]
    public void UpdateVariables_ClientRPC(Vector2 vector)
    {
        Debug.Log(OwnerClientId + ": " + vector);
        // if(!IsOwner)
        //     return;
        if (vector.y < 0)
        {
            BackBoosterStrength = Mathf.Abs(vector.y);
            FrontBoosterStrength = 0;
        }
        else
        {
            FrontBoosterStrength = vector.y;
            BackBoosterStrength = 0;
        }
        if (vector.x < 0)
        {
            RightBoosterStrength = Mathf.Abs(vector.x);
            LeftBoosterStrength = 0;
        }
        else
        {
            LeftBoosterStrength = vector.x;
            RightBoosterStrength = 0;
        }
    }
    private void FixedUpdate()
    {
        // Debug.Log("ID: " + OwnerClientId + " BackBoosterStrength: " + BackBoosterStrength + " IsOwner: " + IsOwner);
        // lerp the particle system speed to the desired speed
        _backBoosterCurrentSpeed = Mathf.Lerp(_backBoosterCurrentSpeed, BackBoosterStrength, 0.7f);
        _frontBoosterCurrentSpeed = Mathf.Lerp(_frontBoosterCurrentSpeed, FrontBoosterStrength, 0.7f);
        _leftBoosterCurrentSpeed = Mathf.Lerp(_leftBoosterCurrentSpeed, LeftBoosterStrength, 0.7f);
        _rightBoosterCurrentSpeed = Mathf.Lerp(_rightBoosterCurrentSpeed, RightBoosterStrength, 0.7f);

        // set the particle system speed
        var main = BackBooster.main;
        main.startSpeed = _backBoosterCurrentSpeed;
        main = FrontBooster.main;
        main.startSpeed = _frontBoosterCurrentSpeed;
        main = LeftBooster.main;
        main.startSpeed = _leftBoosterCurrentSpeed;
        main = RightBooster.main;
        main.startSpeed = _rightBoosterCurrentSpeed;
    }
}
