using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStruct
{
    public enum SpinType
    {
        Bronze,
        Silver,
        Gold
    }

    public static class RarityProbabilities
    {
        public static readonly Dictionary<Rarity, float> Bronze = new Dictionary<Rarity, float>
        {
            { Rarity.Common, 0.6f },
            { Rarity.Uncommon, 0.2f },
            { Rarity.Rare, 0.1f },
            { Rarity.Epic, 0.08f },
            { Rarity.Legendary, 0.02f }
        };

        public static readonly Dictionary<Rarity, float> Silver = new Dictionary<Rarity, float>
        {
            { Rarity.Common, 0.5f },
            { Rarity.Uncommon, 0.2f },
            { Rarity.Rare, 0.15f },
            { Rarity.Epic, 0.1f },
            { Rarity.Legendary, 0.05f }
        };

        public static readonly Dictionary<Rarity, float> Gold = new Dictionary<Rarity, float>
        {
            { Rarity.Common, 0.2f },
            { Rarity.Uncommon, 0.25f },
            { Rarity.Rare, 0.25f },
            { Rarity.Epic, 0.2f },
            { Rarity.Legendary, 0.1f }
        };
    }

    public static class SpinnerStaticData
    {
        public static int CurrentZone = 1;
        public static SpinType CurrentSpinType = SpinType.Bronze;
        public static float zonePrefabWidth;
        public static double referenceAspectRatio = 16f / 9f;
        public static double zoneReferenceWidth = 46f;
        public static double zoneReferenceHeight = 47f;
    }
}

