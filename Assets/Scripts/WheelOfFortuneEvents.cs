using System;
using System.Collections;
using System.Collections.Generic;
using DataStruct.ScriptableObjects;
using UnityEngine;
using Utils;

public class WheelOfFortuneEvents : MonoSingleton<WheelOfFortuneEvents>
{
    public Action<int> OnRewardSelected;
    public Action<RewardItemSO, int> OnItemCardSpawnRequested;
    public Action<RewardItemSO> OnScaleUpItemCardComplete;
    public Action<RewardItemSO, Transform> OnFlyOutItemRequested;
    public Action<RewardItemSO> OnFlyOutAnimationComplete;
    public Action OnReviveSelected;
    public Action DeathCardPicked;
    public Action OnNextLevelRequested;
}
