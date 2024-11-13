using System;
using System.Collections;
using System.Collections.Generic;
using DataStruct;
using DataStruct.ScriptableObjects;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RewardListManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardContainer;
    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private float iconScaler = 1.2f;
    [SerializeField] private float iconScaleDuration = 0.2f;
    [SerializeField] private float amountUpdateDuration = 1.5f;
    [SerializeField] private Ease scaleUpEase = Ease.OutBounce;
    [SerializeField] private Ease scaleDownEase = Ease.OutBounce;
    [SerializeField] private Ease amountUpdateEase = Ease.InQuart;
    [SerializeField] private GameObject exitPanel;
    
    
    
    private Dictionary<string, int> _rewardNameAmountDict;
    private Dictionary<string, (Image rewardImage, TMP_Text rewardAmountText)> _rewardDisplayDict; // Dictionary to store instantiated prefabs' tmp text component
    private Button _exitButton;
    
    private void OnEnable()
    {
        WheelOfFortuneEvents.Instance.OnItemCardSpawnRequested += AddItemToRewardsList;
        WheelOfFortuneEvents.Instance.OnScaleUpItemCardComplete += CallFlyOutOnRewardItem;
        WheelOfFortuneEvents.Instance.OnFlyOutAnimationComplete += UpdateListAnimation;
    }

    private void OnDisable()
    {
        if (WheelOfFortuneEvents.Instance != null)
        {
            WheelOfFortuneEvents.Instance.OnItemCardSpawnRequested -= AddItemToRewardsList;
            WheelOfFortuneEvents.Instance.OnScaleUpItemCardComplete -= CallFlyOutOnRewardItem;
            WheelOfFortuneEvents.Instance.OnFlyOutAnimationComplete -= UpdateListAnimation;
        }
    }

    private void Start()
    {
        _rewardNameAmountDict = new Dictionary<string, int>();
        _rewardDisplayDict = new Dictionary<string, (Image, TMP_Text)>();
        SpinnerStaticData.CurrentZone = 1;
        if (_exitButton == null)
        {
            _exitButton = GetComponentInChildren<Button>();
        }
        _exitButton.onClick.AddListener(GetRewardsAndExit);
    }

    private void OnValidate()
    {
        if (_exitButton == null)
        {
            _exitButton = GetComponentInChildren<Button>();
        }
    }

    private void AddItemToRewardsList(RewardItemSO item, int amount)
    {
        if (item.itemType == ItemType.Death) { return; }
        if (_rewardNameAmountDict.ContainsKey(item.itemName))
        {
            _rewardNameAmountDict[item.itemName] += amount;
        }
        else
        {
            _rewardNameAmountDict.Add(item.itemName, amount);
            
            GameObject newRewardPrefab = Instantiate(rewardPrefab, rewardContainer.transform);
            TMP_Text amountText = newRewardPrefab.GetComponentInChildren<TMP_Text>();
            Image rewardImage = newRewardPrefab.GetComponentInChildren<Image>();
            rewardImage.sprite = item.itemIcon;
            amountText.text = "0";
            // Store the amountText reference in the dictionary for future updates
            _rewardDisplayDict.Add(item.itemName, (rewardImage, amountText));
        }
    }
    
    private void CallFlyOutOnRewardItem(RewardItemSO item)
    {
        Transform rewardImageTransform = GetRewardImageTransform(item);
        WheelOfFortuneEvents.Instance.OnFlyOutItemRequested?.Invoke(item, rewardImageTransform);
    }
    
    private Transform GetRewardImageTransform(RewardItemSO item)
    {
        if (_rewardDisplayDict.TryGetValue(item.itemName, out (Image rewardImage, TMP_Text rewardAmountText) rewardDisplay))
        {
            return rewardDisplay.rewardImage.transform;
        }
        else
        {
            Debug.LogError("Couldn't find the searched item image!");
            return null;
        }
    }
    
    private void UpdateListAnimation(RewardItemSO item)
    {
        if (_rewardDisplayDict.TryGetValue(item.itemName, out (Image rewardImage, TMP_Text rewardAmountText) rewardDisplay))
        {
            float originScale = rewardDisplay.rewardImage.rectTransform.localScale.x;
            Sequence iconTweenSequence = DOTween.Sequence();
            iconTweenSequence
                .Append(rewardDisplay.rewardImage.transform.DOScale(iconScaler, iconScaleDuration))
                .SetEase(scaleUpEase)
                .Append(rewardDisplay.rewardImage.transform.DOScale(originScale, iconScaleDuration))
                .SetEase(scaleDownEase);
            // Update the amount text
            int currentAmount = int.Parse(rewardDisplay.rewardAmountText.text); // Get the current amount as an integer
            iconTweenSequence.Insert(0, DOTween.To(() => currentAmount, x => {
                currentAmount = x;
                rewardDisplay.rewardAmountText.text = currentAmount.ToString();
            }, _rewardNameAmountDict[item.itemName], amountUpdateDuration).SetEase(amountUpdateEase));
        }
        else
        {
            Debug.LogError("Couldn't find the searched item image!");
        }
    }
    
    private void GetRewardsAndExit()
    {
        exitPanel.SetActive(true);
        _exitButton.GetComponentInChildren<TMP_Text>().text = "RESTART";
        _exitButton.onClick.RemoveAllListeners();
        _exitButton.onClick.AddListener(RestartGame);
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
        // Tween only the x-position to the center of the screen
        transform.DOMoveX(worldCenter.x, 1).SetEase(Ease.OutQuart);
        
    }

    private void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
}

