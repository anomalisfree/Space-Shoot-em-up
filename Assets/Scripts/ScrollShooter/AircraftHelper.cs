using System;
using ScrollShooter.Data;
using UnityEngine;

namespace ScrollShooter
{
    public class AircraftHelper : MonoBehaviour
    {
        [SerializeField] private Transform pivot;
        [SerializeField] private HelperPosition helperPosition;
        [SerializeField] private float reaction = 40;
        [SerializeField] private float distanceToPivot = 1.5f;
        [SerializeField] private float rotationIntensity = 100;
        [SerializeField] private BulletShooter bulletShooter;
        [SerializeField] private Health health;
        [SerializeField] private GameObject deadParticle;

        public Action<AircraftHelper> IsDead;

        public void Initialize(Transform aircraftPivot, HelperPosition position)
        {
            pivot = aircraftPivot;
            helperPosition = position;
            health.HealthIsChanged += CheckHealth;
        }

        private void CheckHealth(int value)
        {
            if (value <= 0)
            {
                Dead();
            }
        }

        public void Dead()
        {
            IsDead?.Invoke(this);
            Instantiate(deadParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void Update()
        {
            var pivotPosition = pivot.position;
            var thisTransform = transform;

            var positionTarget = helperPosition switch
            {
                HelperPosition.Right => pivotPosition + Vector3.right * distanceToPivot,
                HelperPosition.Left => pivotPosition + Vector3.left * distanceToPivot,
                _ => Vector3.zero
            };

            var thisPosition = thisTransform.position;
            transform.position = Vector3.Lerp(thisPosition, positionTarget,
                Time.deltaTime * reaction);

            transform.rotation =
                Quaternion.Euler(new Vector3(0, 0, (thisPosition.x - positionTarget.x) * rotationIntensity));
        }

        public void Shoot()
        {
            bulletShooter.Shoot();
        }
    }
}