using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class HomeBase : NetworkBehaviour
{
    public NetworkVariable<int> mineralAmount = new();
    public NetworkVariable<int> crystalAmount = new();
    public NetworkVariable<int> gasAmount = new();

    private void Awake() {
        ReferenceManager.homeBase = this;
    }

}
