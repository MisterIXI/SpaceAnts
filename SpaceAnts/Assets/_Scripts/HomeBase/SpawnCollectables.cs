using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnCollectables : NetworkBehaviour
{

    [SerializeField] private GameObject _gasPrefab;
    [SerializeField] private GameObject _crystalPrefab;
    [SerializeField] private GameObject _mineralPrefab;

    [SerializeField] private int _gasCount;
    private void Start()
    {
        if(IsOwner)
        {

        }
    }

}
