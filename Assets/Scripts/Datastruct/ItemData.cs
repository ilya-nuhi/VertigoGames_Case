using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStruct
{
    public enum Rarity
    {
        None,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum ItemType
    {
        Upgrade,
        WeaponSkin,
        Cosmetic,
        Chest,
        Currency,
        Death
    }

    [Serializable]
    public struct Range<T>
    {
        public T Min;
        public T Max;

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }
    }


    public static class ItemName
    {
        public const string Death = "Death";
        public const string AviatorGlasses = "Aviator Glasses";
        public const string BaseballCap = "Baseball Cap";
        public const string Cash = "Cash";
        public const string ChestBig = "Big Chest";
        public const string ChestBronze = "Bronze Chest";
        public const string ChestGold = "Gold Chest";
        public const string ChestSilver = "Silver Chest";
        public const string ChestSmall = "Small Chest";
        public const string ChestStandart = "Standart Chest";
        public const string ChestSuper = "Super Chest";
        public const string Gold = "Gold";
        public const string HelmetPumpkin = "Pumpkin Helmet";
        public const string BayonetEasterTime = "Bayonet-Easter Time";
        public const string BayonetSummerVice = "Bayonet-Summer Vice";
        public const string GrenadeM26 = "Grenade-M26";
        public const string GrenadeM67 = "Grenade-M67";
        public const string HealthsotNeurostim = "Healthsot Neurostim";
        public const string HealthsotRegenerator = "Healthsot Regenerator";
        public const string Molotov = "Molotov";
        public const string ShotgunT1 = "Tier1 Shotgun";
        public const string MleT2 = "Tier2 Mle";
        public const string RifleT2 = "Tier2 Rifle";
        public const string ShotgunT3 = "Tier3 shotgun";
        public const string SmgT3 = "Tier3 Smg";
        public const string SniperT3 = "Tier3 sniper";
        public const string ArmorPoints = "Armor Points";
        public const string KnifePoints = "Knife Points";
        public const string PistolPoints = "Pistol Points";
        public const string RiflePoints = "Rifle Points";
        public const string ShotgunPoints = "Shotgun Points";
        public const string SmgPoints = "Smg Points";
        public const string SniperPoints = "Sniper Points";
        
        // Add new items here as needed
    }
}


