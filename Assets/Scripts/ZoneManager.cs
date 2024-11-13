using System;
using DataStruct;
using DataStruct.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class ZoneManager : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRectBackground;
    [SerializeField] private ScrollRect scrollRectText;
    [SerializeField] private GameObject zoneTextPrefab; // Assign your ZoneObject prefab here
    [SerializeField] private GameObject zoneBackgroundPrefab; // Assign your ZoneObject prefab here
    [SerializeField] private int totalZones = 60; // Total levels or zones
    [SerializeField] private float slideDuration = 0.5f; // Duration for sliding animation
    [SerializeField] private SpecialZonePrefabsSO specialZonePrefabs;
    [SerializeField] private TMP_Text _nextSuperZoneText;
    [SerializeField] private TMP_Text _nextSafeZoneText;
    [SerializeField] private Color superZoneTextColor;
    [SerializeField] private Color safeZoneTextColor;
    
    
    
    
    
    private float _preferredWidth;
    private int currentZone = 1;

    private void OnEnable()
    {
       WheelOfFortuneEvents.Instance.OnNextLevelRequested += NextZone;
    }

    private void OnDisable()
    {
       if (WheelOfFortuneEvents.Instance != null)
       {
           WheelOfFortuneEvents.Instance.OnNextLevelRequested -= NextZone;
       }
    }

    void Start()
    {
       InitializeZoneBar();
       GetPreferredWidthOfZone();
       UpdateSpecialZoneIndicators();
    }

    private void GetPreferredWidthOfZone()
    {
        double currentAspectRatio = (float)Screen.width / Screen.height;
        double scaleFactor = currentAspectRatio / SpinnerStaticData.referenceAspectRatio;
        _preferredWidth =  (float)(SpinnerStaticData.zoneReferenceWidth * scaleFactor) ;
    }

    private void InitializeZoneBar()
    {
       // Create Zone Levels
       for (int i = 1; i <= totalZones + 10; i++)
       {
           TMP_Text newZoneText = Instantiate(zoneTextPrefab, scrollRectText.content).GetComponentInChildren<TMP_Text>();
           Image newZoneBgImage = Instantiate(zoneBackgroundPrefab, scrollRectBackground.content).GetComponentInChildren<Image>();
           if (i <=5 || i>totalZones+5) // filling 5 empty objects to center the first level
           {
               newZoneText.enabled = false;
               newZoneBgImage.enabled = false;
               continue;
           }
           int zoneNumber = i - 5;
           newZoneText.text = zoneNumber.ToString();
           if (zoneNumber % 30 == 0)
           {
               newZoneBgImage.sprite = specialZonePrefabs.goldZoneSprite;
               newZoneText.color = superZoneTextColor;
           }
           else if (zoneNumber % 5 == 0)
           {
               newZoneBgImage.sprite = specialZonePrefabs.silverZoneSprite;
               newZoneText.color = safeZoneTextColor;
           }
           else
           {
               newZoneBgImage.sprite = specialZonePrefabs.bronzeZoneSprite;
           }
       }
    }
    

    private void NextZone()
    {
       if (currentZone < totalZones)
       {
           SpinnerStaticData.CurrentZone++;
           SlideToNextZone();
           UpdateSpecialZoneIndicators();
       }
    }

    private void UpdateSpecialZoneIndicators()
    {
        _nextSafeZoneText.text = (((SpinnerStaticData.CurrentZone/5)+1)*5).ToString();
        _nextSuperZoneText.text = (((SpinnerStaticData.CurrentZone/30)+1)*30).ToString();
    }

    private void SlideToNextZone()
    {
        // Calculate the current and target normalized position
        float contentWidth = scrollRectBackground.content.rect.width - scrollRectBackground.viewport.rect.width;
        float normalizedWidth = _preferredWidth / contentWidth;
    
        // Calculate the new position by moving left by one "zone" width in normalized terms
        float targetPosition = Mathf.Clamp(scrollRectBackground.horizontalNormalizedPosition + normalizedWidth, 0, 1);
    
        // Use DOTween to animate the scroll to the new position
        scrollRectBackground.DOKill(); // Stop any ongoing tweens
        scrollRectText.DOKill();       // Stop any ongoing tweens
        scrollRectBackground.DOHorizontalNormalizedPos(targetPosition, slideDuration).SetEase(Ease.OutQuad);
        scrollRectText.DOHorizontalNormalizedPos(targetPosition, slideDuration).SetEase(Ease.OutQuad);
    }

}