using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReferenceManager : MonoBehaviour
{
    public static PlayerController playerController;
    public static event Action OnPlayerSpawned = delegate { };
    public static void PlayerSpawned() { OnPlayerSpawned.Invoke(); }
}
