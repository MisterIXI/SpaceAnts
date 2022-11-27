using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
public class PlayerResourceManager : NetworkBehaviour
{
    public NetworkVariable<int> mineralAmount = new();
    public NetworkVariable<int> crystalAmount = new();
    public NetworkVariable<int> gasAmount = new();

    private void Start()
    {
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
