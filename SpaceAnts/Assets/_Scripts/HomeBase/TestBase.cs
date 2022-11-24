using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using TMPro;
public class TestBase : NetworkBehaviour
{
    NetworkVariable<uint> score = new();
    [SerializeField] private GameObject popupTextPrefab;

    void Start()
    {
        score.OnValueChanged += OnScoreChange;
    }

    [ServerRpc]
    public void AddScoreServerRpc(int amount)
    {
        // Debug.Log("ServerRpc called");
        int newScore = (int)score.Value + amount;
        if(newScore < 0)
            newScore = 0;
        score.Value = (uint)newScore;
    }

    private void OnScoreChange(uint previousValue, uint newValue)
    {
        var popupText = Instantiate(popupTextPrefab, transform.position, Quaternion.identity);
        popupText.GetComponentInChildren<TextMeshPro>().SetText("+"+ (newValue - previousValue) + ": " + newValue);
    }

}
