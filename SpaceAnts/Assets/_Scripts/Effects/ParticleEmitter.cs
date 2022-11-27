using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ParticleEmitter : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform _target;
    [HideInInspector] public Transform ParticleTarget;


    private void Update()
    {
        if (ParticleTarget != null)
        {
            SyncTarget();
        }
    }
    private void SyncTarget()
    {
        _target.position = ParticleTarget.position;
    }
    public void SetTarget(Transform newTarget)
    {
        ParticleTarget = newTarget;
        SyncTarget();
    }
    public void EmitParticles(int count)
    {
        _particleSystem.Emit(count);
    }
}
