using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace VR
{
    public class WallDetector : MonoBehaviour
    {
        [SerializeField] private PostProcessVolume postProcessVolume;
        private ColorGrading colorGradingLayer = null;

        private void Start()
        {
            postProcessVolume.profile.TryGetSettings(out colorGradingLayer);
        }

        private void Update()
        {
            var dist = Vector3.Distance(Vector3.zero, transform.localPosition);
            dist *= 20;
            if (dist > 1) dist = 1;
            colorGradingLayer.colorFilter.value = new Color(1 - dist, 1 - dist, 1 - dist);
        }
    }
}