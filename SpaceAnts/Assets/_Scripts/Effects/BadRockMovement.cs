using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class BadRockMovement : NetworkBehaviour
{
    public float Health;
    private HomeBase _homeBase;
    private Vector3 startPosition;
    private float finishTime;
    private float elapsedTime;
    public override void OnNetworkSpawn()
    {
        _homeBase = ReferenceManager.homeBase;
        finishTime = 30f;
        startPosition = transform.position;
        Health = 30;
        _homeBase.OnAsteroidSpawn_ServerOnly(gameObject);
    }

    public void TakeDamage(float damage)
    {
        if (IsServer)
        {
            Health -= damage;
            if (Health <= 0)
            {
                _homeBase.OnAsteroidDespawn_ServerOnly();
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    [ClientRpc]
    public void DamageEffect_ClientRPC()
    {
        // TODO: Add damage effect
    }
    // Update is called once per frame
    void Update()
    {
        if (IsSpawned)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, _homeBase.transform.position, elapsedTime / finishTime);
        }
    }
}
