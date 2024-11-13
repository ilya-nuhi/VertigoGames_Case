using DataStruct;
using UnityEngine;


namespace DataStruct.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewRewardItem", menuName = "Item/Reward Item Data")]
    public class RewardItemSO : ScriptableObject
    {
        public string itemName;
        public Sprite itemIcon;
        public Rarity rarity;
        public ItemType itemType;
        public Range<int> amountRange;
    }
}

