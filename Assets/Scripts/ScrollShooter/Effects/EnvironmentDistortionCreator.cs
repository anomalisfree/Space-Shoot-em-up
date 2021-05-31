using UnityEngine;

namespace ScrollShooter.Effects
{
    public class EnvironmentDistortionCreator : MonoBehaviour
    {
        [SerializeField] private EnvironmentDistortion environmentDistortion;
        [SerializeField] private float scale = 1f;

        private void OnEnable()
        {
            Destroy(gameObject,
                Instantiate(environmentDistortion, transform.position, Quaternion.identity).Initialize(scale));
        }
    }
}