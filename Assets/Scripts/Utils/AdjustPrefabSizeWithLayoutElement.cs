using System;
using System.Collections;
using System.Collections.Generic;
using DataStruct;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    [RequireComponent(typeof(LayoutElement))]
    public class AdjustPrefabSizeWithLayoutElement : MonoBehaviour
    {
        // Cached LayoutElement component
        [SerializeField] private LayoutElement layoutElement;

        private void Awake()
        {
            // Get the LayoutElement component
            AdjustSizeToAspectRatio();
        }

        private void OnValidate()
        {
            AdjustSizeToAspectRatio();
        }


        void AdjustSizeToAspectRatio()
        {
            // Get the current screen aspect ratio
            double currentAspectRatio = (float)Screen.width / Screen.height;

            // Calculate the scale factor based on the difference in aspect ratios
            double scaleFactor = currentAspectRatio / SpinnerStaticData.referenceAspectRatio;

            // Adjust the LayoutElement preferred width and height
            layoutElement.preferredWidth = (float)(SpinnerStaticData.zoneReferenceWidth * scaleFactor) ;
            layoutElement.preferredHeight = (float)(SpinnerStaticData.zoneReferenceHeight * scaleFactor);
        }
    }

}
