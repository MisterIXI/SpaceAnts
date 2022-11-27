using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleReceiver : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform _target;
    [HideInInspector] public Transform ParticleSource;


    private void Update()
    {
        if (ParticleSource != null)
        {
            SyncTarget();
        }
    }

    private void SyncTarget()
    {
        _particleSystem.transform.position = ParticleSource.position;
    }
    public void SetSource(Transform newSource)
    {
        ParticleSource = newSource;
        SyncTarget();
    }
    public void EmitParticles(int count)
    {
        _particleSystem.Emit(count);
    }
}
