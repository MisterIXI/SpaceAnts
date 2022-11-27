using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
public class IngameGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText;

    private PlayerResourceManager _playerResourceManager;
    private HomeBase _homeBase;

    private void Start()
    {
        ReferenceManager.OnPlayerSpawned += OnSpawn;

    }

    private void OnSpawn()
    {
        _playerResourceManager = ReferenceManager.playerController.GetComponent<PlayerResourceManager>();
        _homeBase = ReferenceManager.homeBase;
        _playerResourceManager.mineralAmount.OnValueChanged += UpdateUI;
        _playerResourceManager.crystalAmount.OnValueChanged += UpdateUI;
        _playerResourceManager.gasAmount.OnValueChanged += UpdateUI;
        _homeBase.mineralAmount.OnValueChanged += UpdateUI;
        _homeBase.crystalAmount.OnValueChanged += UpdateUI;
        _homeBase.gasAmount.OnValueChanged += UpdateUI;
        UpdateUI(0, 0);
    }
    private void UpdateUI(int oldValue, int newValue)
    {
        statusText.text = $"Minerals: {_playerResourceManager.mineralAmount.Value} \n" +
                          $"Crystals: {_playerResourceManager.crystalAmount.Value} \n" +
                          $"Gas: {_playerResourceManager.gasAmount.Value} \n" +
                          $"HomeBase Minerals: {_homeBase.mineralAmount.Value} \n" +
                          $"HomeBase Crystals: {_homeBase.crystalAmount.Value} \n" +
                          $"HomeBase Gas: {_homeBase.gasAmount.Value} \n";
    }
}
