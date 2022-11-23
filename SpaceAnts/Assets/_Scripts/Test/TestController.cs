using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine.InputSystem;
public class TestController : NetworkBehaviour
{
    private NetworkVariable<Vector3> _position = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);
    private void Start() {
        EventManager.SubscribeAction("Move", OnMove);
        
    }
    void Update()
    {
        if (IsOwner)
        {
            _position.Value = transform.position;
        }
        else
        {
            transform.position = _position.Value;
        }    
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            Vector2 input = context.ReadValue<Vector2>();
            transform.position += new Vector3(input.x, 0, input.y);
        }
    }
}
