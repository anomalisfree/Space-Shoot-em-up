using UnityEngine;

namespace ScrollShooter.Effects
{
    public class EnvironmentDistortion : MonoBehaviour
    {
        private const float LifeTime = 1f;

        public float Initialize(float scale)
        {
            transform.localScale = Vector3.one * scale;
            Destroy(gameObject, LifeTime);
            return LifeTime;
        }
    }
}
