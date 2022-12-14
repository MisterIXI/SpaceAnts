using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class EventManager : MonoBehaviour
{
    public static void SubscribeAction(string actionName, Action<InputAction.CallbackContext> action)
    {
        InputAction inputAction = ReferenceManager.playerInput.actions.FindAction(actionName);
        inputAction.started += action;
        inputAction.performed += action;
        inputAction.canceled += action;
    }

    private void test()
    {
        SubscribeAction("test", test2);
    }
    private void test1()
    {

    }

    private void test2(InputAction.CallbackContext context)
    {

    }
}
