using System;
using System.Collections.Generic;
using System.Globalization;
using DataStruct;
using DataStruct.ScriptableObjects;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public class WheelSpinner : MonoBehaviour
{
    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private float spinFixDuration = 1.5f;
    [SerializeField] private int spinRounds = 5;
    [SerializeField] private GameObject spinBase;
    
    private Button _spinButton;
    private bool _canSpin;

    private void OnEnable()
    {
        WheelOfFortuneEvents.Instance.OnNextLevelRequested += EnableSpin;
    }

    private void OnDisable()
    {
        _spinButton.onClick.RemoveAllListeners();
        WheelOfFortuneEvents.Instance.OnNextLevelRequested -= EnableSpin;
    }

    private void Start()
    {
        SpinnerStaticData.CurrentZone = 1;
        if (_spinButton == null)
        {
            _spinButton = GetComponentInChildren<Button>();
        }
        _spinButton.onClick.AddListener(SpinWheel);
        _canSpin = true;
    }

    private void OnValidate()
    {
        if (_spinButton == null)
        {
            // Attempt to find a Button component on the same GameObject or in its children
            _spinButton = GetComponentInChildren<Button>();
        }
    }
    private void EnableSpin()
    {
        _canSpin = true;
    }

    private void SpinWheel()
    {
        if (!_canSpin) return; // Avoid multiple spins at the same time
        _canSpin = false;

        // Randomize the final rotation angle
        float targetAngle = 360 * spinRounds + Random.Range(0f, 360f);
        float snappedAngle = Mathf.Round(targetAngle / 45f) * 45f;

        // Create a sequence
        Sequence spinSequence = DOTween.Sequence();

        // Add the first rotation to the sequence (spin the wheel)
        spinSequence.Append(spinBase.transform.DORotate(new Vector3(0, 0, -targetAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic));

        // Add the second rotation to the sequence (snap to nearest segment)
        spinSequence.Append(spinBase.transform.DORotate(new Vector3(0, 0, -snappedAngle), spinFixDuration, RotateMode.Fast)
            .SetEase(Ease.OutBounce));

        // On complete, reset _isSpinning and call OnSpinComplete
        spinSequence.OnComplete(() =>
        {
            OnSpinComplete(snappedAngle);
        });
    }


    private void OnSpinComplete(float snappedAngle)
    {
        // Determine which slice the wheel landed on
        int rewardIndex = Mathf.RoundToInt((snappedAngle%360) / 45);
        // and perform actions based on the result (reward or bomb)
        
        // Call function to move to the next zone
        // MoveToNextZone();
        WheelOfFortuneEvents.Instance.OnRewardSelected?.Invoke(rewardIndex);
    }
}
