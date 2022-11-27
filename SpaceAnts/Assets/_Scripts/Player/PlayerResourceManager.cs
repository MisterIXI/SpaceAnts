using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
public class PlayerResourceManager : NetworkBehaviour
{
    [SerializeField] private ParticleEmitter _crystalEmitter;
    [SerializeField] private ParticleReceiver _crystalReceiver;
    [SerializeField] private int DigStrength = 5;
    public NetworkVariable<int> mineralAmount = new();
    public NetworkVariable<int> crystalAmount = new();
    public NetworkVariable<int> gasAmount = new();
    private HomeBase _homeBase;

    private float _lastCollectionTime;
    private const float COLLECTION_COOLDOWN = 0.5f;
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
                Debug.Log("HomeBase triggered");
                DepositResource_ServerRPC();
            }
        }
        if (other.CompareTag("ResourcePoint"))
        {
            Mining_Start(other);
        }
        else if (other.CompareTag("HomeBase"))
        {
            _crystalEmitter.SetTarget(other.transform);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ResourcePoint"))
        {
            Mining_Continue(other);
        }
    }
    private void Mining_Start(Collider other)
    {
        _crystalReceiver.SetSource(other.transform);
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

    [ServerRpc]
    public void DepositResource_ServerRPC()
    {
        _homeBase.mineralAmount.Value += mineralAmount.Value;
        _homeBase.crystalAmount.Value += crystalAmount.Value;
        _homeBase.gasAmount.Value += gasAmount.Value;
        DepositEffect_ClientRPC(mineralAmount.Value, crystalAmount.Value, gasAmount.Value);
        mineralAmount.Value = 0;
        crystalAmount.Value = 0;
        gasAmount.Value = 0;
    }

    [ClientRpc]
    public void DepositEffect_ClientRPC(int mineralCount, int crystalCount, int gasCount)
    {
        // TODO: add particle effect
        _crystalEmitter.EmitParticles(crystalCount);
    }

    [ClientRpc]
    public void GatherEffect_ClientRPC(int mineralCount, int crystalCount, int gasCount)
    {
        _crystalReceiver.EmitParticles(crystalCount);
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
