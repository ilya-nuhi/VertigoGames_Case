using System;
using System.Collections;
using System.Collections.Generic;
using DataStruct;
using DataStruct.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WheelContentSetter : MonoBehaviour
{
    [SerializeField] private SpinTypeSettingsSO[] spinTypeSettings;
    [SerializeField] private RewardItemSO cashReward, goldReward, deathReward;
    [SerializeField] private List<RewardItemSO> allOtherRewardItems;
    [SerializeField] private Image[] contentImage = new Image[8];
    [SerializeField] private TMP_Text[] contentText = new TMP_Text[8];
    
    [SerializeField] private Image spinBaseImage;
    [SerializeField] private Image spinIndicatorImage;
    [SerializeField] private TMP_Text spinnerText;
    [SerializeField] private TMP_Text spinnerInfoText;

    private List<(RewardItemSO rewardItem, int amount)> _currentRewardsWithAmounts;

    private void OnValidate()
    {
        if (contentImage.Length != 8)
        {
            System.Array.Resize(ref contentImage, 8);
        }
        if (contentText.Length != 8)
        {
            System.Array.Resize(ref contentText, 8);
        }
    }

    private void OnEnable()
    {
        WheelOfFortuneEvents.Instance.OnRewardSelected += OnRewardSelected;
        WheelOfFortuneEvents.Instance.OnNextLevelRequested += SetWheelContent;
    }

    private void OnDisable()
    {
        if (WheelOfFortuneEvents.Instance != null)
        {
            WheelOfFortuneEvents.Instance.OnRewardSelected -= OnRewardSelected;
            WheelOfFortuneEvents.Instance.OnNextLevelRequested -= SetWheelContent;
        }
    }

    private void Start()
    {
        SpinnerStaticData.CurrentSpinType = SpinType.Bronze;
        ChangeSpinType(SpinType.Bronze);
        SetWheelContent();
    }

    private void OnRewardSelected(int rewardIndex)
    {
        var reward = _currentRewardsWithAmounts[rewardIndex];
        // Logic for handling reward (e.g., display it, add to player's inventory, etc.)
        WheelOfFortuneEvents.Instance.OnItemCardSpawnRequested?.Invoke(reward.rewardItem, reward.amount);
        if (reward.rewardItem.itemType == ItemType.Death)
        {
            WheelOfFortuneEvents.Instance.DeathCardPicked?.Invoke();
        }
    }
    
    private void SetWheelContent()
    {
        List<RewardItemSO> chosenRewards = GenerateRewardsForSpin();
        _currentRewardsWithAmounts = new List<(RewardItemSO rewardItem, int amount)>();
        ShuffleList(chosenRewards);

        for (int i = 0; i < contentImage.Length; i++)
        {
            contentImage[i].sprite = chosenRewards[i].itemIcon;
            int rewardAmount = Random.Range(chosenRewards[i].amountRange.Min, chosenRewards[i].amountRange.Max);
            if (SpinnerStaticData.CurrentZone % 30 == 0) rewardAmount*=10;
            else if (SpinnerStaticData.CurrentZone % 5 == 0) rewardAmount*=2;
            
            if (rewardAmount>0)
            {
                contentText[i].text = rewardAmount >= 10000 
                    ? 'x' + Mathf.Round(rewardAmount / 1000f).ToString() + 'K' 
                    : 'x' + rewardAmount.ToString();
            }
            else
            {
                contentText[i].text = string.Empty;
            }
            _currentRewardsWithAmounts.Add((chosenRewards[i], rewardAmount));
        }
    }

    private List<RewardItemSO> GenerateRewardsForSpin()
    {
        List<RewardItemSO> spinRewards = new List<RewardItemSO>();

        // Add 1 Death Item if it's not a safe zone
        if (SpinnerStaticData.CurrentZone % 5 != 0) spinRewards.Add(deathReward);

        // Add 2 Cash Items
        spinRewards.Add(cashReward);
        spinRewards.Add(cashReward);
        
        // Add 2 Gold Items
        spinRewards.Add(goldReward);
        spinRewards.Add(goldReward);
        
        int remainingContentCount = 8-spinRewards.Count;
            
        // Fill spin contents with other items (not cash, gold, or death)
        for (int i = 0; i < remainingContentCount; i++)
        {
            spinRewards.Add(GetRandomRewardWithRarity());
        }

        return spinRewards;
    }

    private RewardItemSO GetRandomRewardWithRarity()
    {
        SpinType spinType;
        if (SpinnerStaticData.CurrentZone % 30 == 0) spinType = SpinType.Gold;
        else if (SpinnerStaticData.CurrentZone % 5 == 0) spinType = SpinType.Silver;
        else spinType = SpinType.Bronze;

        if (spinType != SpinnerStaticData.CurrentSpinType) ChangeSpinType(spinType);
        Rarity chosenRarity = GetRandomRarity(spinType);

        // Filter the rewards by the selected rarity
        List<RewardItemSO> rarityFilteredRewards = allOtherRewardItems.FindAll(item => item.rarity == chosenRarity);

        return rarityFilteredRewards[Random.Range(0, rarityFilteredRewards.Count)];
    }

    private void ChangeSpinType(SpinType spinType)
    {
        SpinTypeSettingsSO spinSettings = spinTypeSettings[(int)spinType]; ;
        spinBaseImage.sprite = spinSettings.baseSprite;
        spinIndicatorImage.sprite = spinSettings.indicatorSprite;
        spinnerText.text = spinSettings.spinnerText;
        spinnerText.color = spinSettings.textColor;
        spinnerInfoText.text = spinSettings.spinnerInfoText;
        spinnerInfoText.color = spinSettings.textColor;

        SpinnerStaticData.CurrentSpinType = spinType;
    }

    public Rarity GetRandomRarity(SpinType spinType)
    {
        Dictionary<Rarity, float> probabilities = spinType switch
        {
            SpinType.Bronze => RarityProbabilities.Bronze,
            SpinType.Silver => RarityProbabilities.Silver,
            SpinType.Gold => RarityProbabilities.Gold,
            _ => RarityProbabilities.Bronze
        };

        return GetRarityFromProbability(probabilities);
    }

    private Rarity GetRarityFromProbability(Dictionary<Rarity, float> probabilities)
    {
        float roll = Random.Range(0f, 1f);
        float cumulative = 0f;

        foreach (var rarity in probabilities)
        {
            cumulative += rarity.Value;
            if (roll <= cumulative)
                return rarity.Key;
        }

        return Rarity.Common; // Default fallback if nothing else matches
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count-1; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            if (i == randomIndex) { continue; }
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}

