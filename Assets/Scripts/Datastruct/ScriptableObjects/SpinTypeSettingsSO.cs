using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStruct.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpinTypeSettings", menuName = "Spin/Wheel Spin Settings")]
    public class SpinTypeSettingsSO : ScriptableObject
    {
        public SpinType spinType;
        public Sprite baseSprite;
        public Sprite indicatorSprite;
        public string spinnerText;
        public Color textColor;
        public string spinnerInfoText;
    }
}
