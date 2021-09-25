using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace VR
{
    public class WallDetector : MonoBehaviour
    {
        [SerializeField] private PostProcessVolume postProcessVolume;
        private ColorGrading colorGradingLayer;

        private const float DistanceScaler = 20;
        private const float VisionRecoveryRate = 10;

        private void Start()
        {
            postProcessVolume.profile.TryGetSettings(out colorGradingLayer);
        }

        private void Update()
        {
            var dist = Vector3.Distance(Vector3.zero, transform.localPosition);
            if (dist > 0.01f)
            {
                dist *= DistanceScaler;
                if (dist > 1) dist = 1;
                colorGradingLayer.colorFilter.value = new Color(1 - dist, 1 - dist, 1 - dist);
            }
            else
            {
                colorGradingLayer.colorFilter.value =
                    Color.Lerp(colorGradingLayer.colorFilter.value, Color.white, Time.deltaTime * VisionRecoveryRate);
            }
        }

        public void SetScreenDark()
        {
            colorGradingLayer.colorFilter.value = Color.black;
        }
    }
}