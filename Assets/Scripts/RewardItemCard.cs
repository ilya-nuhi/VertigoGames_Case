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

public class RewardItemCard : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Image cardFrameImage;
    [SerializeField] private Image bgFlashImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private GameObject itemCardContainer;

    [SerializeField] private float scaleUpDuration = 1;
    [SerializeField] private float scaleDownDuration = 1;
    [SerializeField] private float waitDuration = 1;
    [SerializeField] private Ease scaleUpEase = Ease.OutBack;
    [SerializeField] private Ease scaleDownEase = Ease.InBack;
    

    public RewardItemSO rewardItemSO;
    public int rewardAmount;
    
    private Dictionary<Rarity, Color> rarityColors = new Dictionary<Rarity, Color>
    {
        { Rarity.Common, new Color(0.75f,0.75f,0.75f) },
        { Rarity.Uncommon, new Color(0,0.67f,0) },
        { Rarity.Rare, new Color(0, 0.6f, 1) },
        { Rarity.Epic, new Color(0.7f, 0f, 1) },
        { Rarity.Legendary, new Color(1,0.74f,0) }
    };
    private void Awake()
    {
        itemCardContainer.SetActive(false);
    }

    private void OnEnable()
    {
        WheelOfFortuneEvents.Instance.OnItemCardSpawnRequested += OnSpawnRequested;
        WheelOfFortuneEvents.Instance.OnReviveSelected += ScaleDownTween;
    }

    private void OnDisable()
    {
        if (WheelOfFortuneEvents.Instance != null)
        {
            WheelOfFortuneEvents.Instance.OnItemCardSpawnRequested -= OnSpawnRequested;
            WheelOfFortuneEvents.Instance.OnReviveSelected -= ScaleDownTween;
        }
    }

    private void OnSpawnRequested(RewardItemSO rewardItem, int amount)
    {
        Initialize(rewardItem, amount);
        ScaleUpTween(rewardItem);
    }

    private void Initialize(RewardItemSO rewardItemSO, int rewardAmount)
    {
        this.rewardItemSO = rewardItemSO;
        this.rewardAmount = rewardAmount;
        SetItemColor(rewardItemSO.itemType, rewardItemSO.rarity);
        itemIcon.sprite = rewardItemSO.itemIcon;
        itemNameText.text = rewardItemSO.itemType == ItemType.Death ? string.Empty : rewardItemSO.itemName;
        amountText.text = rewardAmount>0 ? rewardAmount.ToString() : string.Empty;
    }
    
    private void ScaleUpTween(RewardItemSO rewardItem)
    {
        itemCardContainer.transform.position = transform.position;
        itemCardContainer.transform.localScale = Vector3.zero;
        itemCardContainer.SetActive(true);

        itemCardContainer.transform.DOScale(Vector3.one, scaleUpDuration)
            .SetEase(scaleUpEase)
            .OnComplete(() =>
            {
                if (rewardItem.itemType == ItemType.Death) {return; }
                WheelOfFortuneEvents.Instance.OnScaleUpItemCardComplete?.Invoke(rewardItemSO);
                StartCoroutine(WaitAndScaleDown());
            });
    }

    private IEnumerator WaitAndScaleDown()
    {
        yield return new WaitForSeconds(waitDuration);
        ScaleDownTween();
    }

    private void ScaleDownTween()
    {
        itemCardContainer.transform.DOScale(Vector3.zero, scaleDownDuration)
            .SetEase(scaleDownEase)
            .OnComplete(() =>
            {
                itemCardContainer.SetActive(false); // Hide the card if needed after scaling down
            });
    }
    
    private void SetItemColor(ItemType itemType,Rarity rarity)
    {
        Color color = rarityColors.TryGetValue(rarity, out Color foundColor) ? foundColor : new Color(0.75f,0.75f,0.75f);
        if(itemType == ItemType.Death) color = Color.red;
        cardFrameImage.color = color;
        bgFlashImage.color = color;
        itemNameText.color = color;
    }
    
}

