using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Start()
    {
        ReferenceManager.OnPlayerSpawned += DelayedStart;
        ReferenceManager.playerController = this;
        ReferenceManager.PlayerSpawned();
    }

    private void DelayedStart()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
