using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class ReferenceManager : MonoBehaviour
{
    public static PlayerInput playerInput;
    public static PlayerController playerController;
    public static event Action OnPlayerSpawned = delegate { };
    public static void PlayerSpawned() { OnPlayerSpawned.Invoke(); }

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }
}
