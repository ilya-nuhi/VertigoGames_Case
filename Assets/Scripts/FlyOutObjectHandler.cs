using System;
using DataStruct;
using DataStruct.ScriptableObjects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;


public class FlyOutObjectHandler : MonoBehaviour
{
    [SerializeField] private ImagePool flyOutImagePool;
    [SerializeField] private Range<int> amountRange = new Range<int>(6, 12);
    [SerializeField] private Range<float> scaleRange = new Range<float>(0.5f, 1.2f); // Range for random scale
    
    [SerializeField] private float spreadRangeScreenHeightMultiplier = 0.05f; // Spread range for position offset
    [SerializeField] private float moveDelay = 0.5f; // Delay before moving to destination
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float spawnDuration = 0.5f;
    [SerializeField] private Ease spawnEase;
    [SerializeField] private Ease moveEase;
    
    
    private void OnEnable()
    {
        WheelOfFortuneEvents.Instance.OnFlyOutItemRequested += SpawnFlyOutObjects;
    }

    private void OnDisable()
    {
        if (WheelOfFortuneEvents.Instance != null)
        {
            WheelOfFortuneEvents.Instance.OnFlyOutItemRequested -= SpawnFlyOutObjects;
        }
    }

    public void SpawnFlyOutObjects(RewardItemSO item, Transform destinationItem)
    {
        int objectCount = Random.Range(amountRange.Min, amountRange.Max + 1);
        int completedCount = 0; // Counter for completed animations
        
        for (int i = 1; i <= objectCount; i++)
        {
            Image flyOutObject = flyOutImagePool.Get();
            if (flyOutObject != null)
            {
                flyOutObject.sprite = item.itemIcon;
                // Set initial position, rotation, and scale
                flyOutObject.transform.position = transform.position;
                flyOutObject.transform.localScale = Vector3.zero;

                // Create and configure a sequence for each object
                float targetScale = Random.Range(scaleRange.Min, scaleRange.Max);

                Sequence flyOutSequence = DOTween.Sequence();
                flyOutSequence
                    .Append(flyOutObject.transform.DOScale(targetScale, spawnDuration)
                        .SetEase(spawnEase)) // Scale up
                    .Join(flyOutObject.transform.DOMove( transform.position + GetRandomSpread(), spawnDuration)
                        .SetEase(spawnEase))
                    .AppendInterval(moveDelay) // Wait before moving
                    .Append(flyOutObject.transform.DOMove(destinationItem.position, moveDuration)
                        .SetEase(moveEase)) // Move to destination
                    .OnComplete(() =>
                    {
                        flyOutImagePool.ReturnToPool(flyOutObject);
                        completedCount++;
                        if (completedCount == objectCount)
                        {
                            // Invoke the event only when the last animation completes
                            WheelOfFortuneEvents.Instance.OnFlyOutAnimationComplete?.Invoke(item);
                            WheelOfFortuneEvents.Instance.OnNextLevelRequested?.Invoke();
                        }
                    }); // Return to pool after moving
            }
        }
    }
    
    private Vector3 GetRandomSpread()
    {
        // Random offset within the spread range
        float spreadRange = spreadRangeScreenHeightMultiplier * Camera.main.orthographicSize;
    
        return new Vector3(
            Random.Range(-spreadRange, spreadRange),
            Random.Range(-spreadRange, spreadRange),
            0
        );
    }
    
}
