using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PopupText : MonoBehaviour
{

    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
        Destroy(gameObject, 2f); // destroy after 2 seconds
    }

    private void Update()
    {
        transform.forward = _camera.transform.forward;
    }

}
