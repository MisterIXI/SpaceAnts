using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ResourcePoint : NetworkBehaviour
{
    private const float SCALE_FACTOR = 50f;
    public enum ResourceType
    {
        Mineral,
        Crystal,
        Gas
    }

    public ResourceType resourceType;

    public NetworkVariable<int> resourceAmount = new();

    private void Awake()
    {

    }

    private void Start()
    {
        if (IsServer)
        {
            int amount = 0;
            switch (resourceType)
            {
                case ResourceType.Mineral:
                    amount = Random.Range(100, 200);
                    break;
                case ResourceType.Crystal:
                    amount = Random.Range(25, 50);
                    break;
                case ResourceType.Gas:
                    amount = Random.Range(50, 100);
                    break;
            }
            resourceAmount.Value = amount;
        }

        OnAmountChange(0, resourceAmount.Value);
        resourceAmount.OnValueChanged += OnAmountChange;
    }

    public void OnAmountChange(int oldAmount, int newAmount)
    {
        float scale = newAmount / SCALE_FACTOR + 0.3f;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    [ServerRpc]
    public void Mine_ServerRPC(int amount, ResourceType type, ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("Mining " + amount + " from " + resourceType + " with " + resourceAmount.Value + " remaining");
        int adjustedAmount = Mathf.Min(amount, resourceAmount.Value); // cap at remaining amount
        resourceAmount.Value -= adjustedAmount;
        var resourceManager = NetworkManager.Singleton.ConnectedClients[serverRpcParams.Receive.SenderClientId].PlayerObject.GetComponent<PlayerResourceManager>();
        switch (type)
        {
            case ResourceType.Mineral:
                resourceManager.mineralAmount.Value += adjustedAmount;
                break;
            case ResourceType.Crystal:
                resourceManager.crystalAmount.Value += adjustedAmount;
                break;
            case ResourceType.Gas:
                resourceManager.gasAmount.Value += adjustedAmount;
                break;
        }
        if (resourceAmount.Value <= 0)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }

}
