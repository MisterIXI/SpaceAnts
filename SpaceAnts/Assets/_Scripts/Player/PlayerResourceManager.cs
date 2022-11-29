using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
public class PlayerResourceManager : NetworkBehaviour
{
    [SerializeField] private ParticleEmitter _mineralEmitter;
    [SerializeField] private ParticleReceiver _mineralReceiver;
    [SerializeField] private ParticleEmitter _crystalEmitter;
    [SerializeField] private ParticleReceiver _crystalReceiver;
    [SerializeField] private ParticleEmitter _gasEmitter;
    [SerializeField] private ParticleReceiver _gasReceiver;
    [SerializeField] private int DigStrength = 2;
    public NetworkVariable<int> mineralAmount = new();
    public NetworkVariable<int> crystalAmount = new();
    public NetworkVariable<int> gasAmount = new();
    private HomeBase _homeBase;

    private float _lastCollectionTime;
    private const float COLLECTION_COOLDOWN = 0.2f;
    private float _lastDepositTime;
    private const float DEPOSIT_COOLDOWN = 0.15f;
    private void Start()
    {
        _homeBase = ReferenceManager.homeBase;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (IsOwner)
        {

            if (other.gameObject.CompareTag("HomeBase"))
            {
                Deposit_Start(other);
            }
        }
        if (other.CompareTag("ResourcePoint"))
        {
            Mining_Start(other);
        }
        else if (other.CompareTag("HomeBase"))
        {
            _mineralEmitter.SetTarget(other.transform);
            _crystalEmitter.SetTarget(other.transform);
            _gasEmitter.SetTarget(other.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ResourcePoint"))
        {
            Mining_Continue(other);
        }
        if (IsOwner)
        {
            if (other.gameObject.CompareTag("HomeBase"))
            {
                Deposit_Continue(other);
            }
        }
    }

    private void Mining_Start(Collider other)
    {
        _mineralReceiver.SetSource(other.transform);
        _crystalReceiver.SetSource(other.transform);
        _gasReceiver.SetSource(other.transform);
        if (IsOwner)
        {
            _lastCollectionTime = Time.time;
        }
    }

    private void Mining_Continue(Collider other)
    {
        if (IsOwner && Time.time - _lastCollectionTime > COLLECTION_COOLDOWN)
        {
            _lastCollectionTime = Time.time;
            ResourcePoint resourcePoint = other.GetComponent<ResourcePoint>();
            resourcePoint.Mine_ServerRPC(DigStrength);
        }

    }
    private void Deposit_Start(Collider other)
    {

        if (IsOwner)
        {
            _lastDepositTime = Time.time;
        }
    }
    private void Deposit_Continue(Collider other)
    {
        if (IsOwner && Time.time - _lastDepositTime > DEPOSIT_COOLDOWN)
        {
            _lastDepositTime = Time.time;
            DepositResource_ServerRPC();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void DepositResource_ServerRPC()
    {
        int mineralDelta = Mathf.Min(mineralAmount.Value, 10);
        int crystalDelta = Mathf.Min(crystalAmount.Value, 10);
        int gasDelta = Mathf.Min(gasAmount.Value, 10);
        _homeBase.mineralAmount.Value += mineralDelta;
        _homeBase.crystalAmount.Value += crystalDelta;
        _homeBase.gasAmount.Value += gasDelta;
        DepositEffect_ClientRPC(mineralDelta, crystalDelta, gasDelta);
        mineralAmount.Value -= mineralDelta;
        crystalAmount.Value -= crystalDelta;
        gasAmount.Value -= gasDelta;
    }

    [ClientRpc]
    public void DepositEffect_ClientRPC(int mineralCount, int crystalCount, int gasCount)
    {
        _mineralEmitter.EmitParticles(mineralCount);
        _crystalEmitter.EmitParticles(crystalCount);
        _gasEmitter.EmitParticles(gasCount);
    }

    [ClientRpc]
    public void GatherEffect_ClientRPC(int mineralCount, int crystalCount, int gasCount)
    {
        _mineralReceiver.EmitParticles(mineralCount);
        _crystalReceiver.EmitParticles(crystalCount);
        _gasReceiver.EmitParticles(gasCount);
    }

    // Dicey CameraControl L186++
    //     if (gameObject.GetComponent<Renderer>())
    // {
    //     MaterialPropertyBlock materialProperty = new MaterialPropertyBlock();
    //     //change the opacity of object which is colliding
    //     Renderer objRenderer = gameObject.GetComponent<Renderer>();
    //     _affectedMat.Add(gameObject);
    //     objRenderer.GetPropertyBlock(materialProperty);
    //     materialProperty.SetFloat("_Opacity", opacity);
    //     materialProperty.SetFloat("_Dither_Size", ditherSize);
    //     objRenderer.SetPropertyBlock(materialProperty);
    // }
}
