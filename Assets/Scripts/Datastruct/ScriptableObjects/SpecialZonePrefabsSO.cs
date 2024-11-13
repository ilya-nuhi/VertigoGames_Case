using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DataStruct.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpecialZonePrefabs", menuName = "SpecialZone/Prefabs")]
    public class SpecialZonePrefabsSO : ScriptableObject
    {
        public Sprite bronzeZoneSprite;
        public Sprite silverZoneSprite;
        public Sprite goldZoneSprite;
    }
}
