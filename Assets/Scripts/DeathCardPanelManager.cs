using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCardPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject deathCardPanel;

    private Button _giveUpButton;
    private Button _reviveButton;

    private void OnEnable()
    {
        WheelOfFortuneEvents.Instance.DeathCardPicked += EnablePanel;
    }

    private void OnDisable()
    {
        _giveUpButton.onClick.RemoveAllListeners();
        _reviveButton.onClick.RemoveAllListeners();
        if (WheelOfFortuneEvents.Instance.DeathCardPicked != null)
        {
            WheelOfFortuneEvents.Instance.DeathCardPicked -= EnablePanel;
        }
    }
    
    private void OnValidate()
    {
        if (_giveUpButton == null || _reviveButton == null)
        {
            List<Button> buttons = new List<Button>(GetComponentsInChildren<Button>(true));
            if (buttons.Count >= 2)
            {
                _giveUpButton = buttons[0];
                _reviveButton = buttons[1];
            }
            else
            {
                Debug.LogWarning("DeathCardPanelManager: Not enough buttons found as children. Make sure there are at least two buttons for 'Give Up' and 'Revive'.");
            }
        }
    }
    private void Start()
    {
        deathCardPanel.SetActive(false);
        if (_giveUpButton == null || _reviveButton == null)
        {
            List<Button> buttons = new List<Button>(GetComponentsInChildren<Button>(true));
            if (buttons.Count >= 2)
            {
                _giveUpButton = buttons[0];
                _reviveButton = buttons[1];
            }
            else
            {
                Debug.LogWarning("DeathCardPanelManager: Not enough buttons found as children. Make sure there are at least two buttons for 'Give Up' and 'Revive'.");
            }
        }
        _giveUpButton.onClick.AddListener(GiveUp);
        _reviveButton.onClick.AddListener(Revive);
    }
    
    private void EnablePanel()
    {
        deathCardPanel.SetActive(true);
    }

    private void Revive()
    {
        deathCardPanel.SetActive(false);
        WheelOfFortuneEvents.Instance.OnReviveSelected?.Invoke();
        WheelOfFortuneEvents.Instance.OnNextLevelRequested?.Invoke();
    }

    private void GiveUp()
    {
        GameManager.Instance.RestartGame();
    }
    
}
