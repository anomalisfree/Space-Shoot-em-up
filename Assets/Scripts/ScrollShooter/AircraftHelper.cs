using System;
using ScrollShooter.Data;
using ScrollShooter.ScriptableObjects;
using UnityEngine;

namespace ScrollShooter
{
    public class AircraftHelper : MonoBehaviour
    {
        [SerializeField] private HelperPosition helperPosition;
        [SerializeField] private BulletShooter bulletShooter;
        [SerializeField] private Health health;
        [SerializeField] private GameObject deadParticle;

        public Action<AircraftHelper> IsDead;
        private Transform pivot;
        private float reaction;
        private float distanceToPivot;
        private float rotationIntensity;

        public void Initialize(Transform aircraftPivot, HelperPosition position, AircraftParams aircraftParams)
        {
            pivot = aircraftPivot;
            transform.position = aircraftPivot.position;
            helperPosition = position;
            reaction = aircraftParams.helperReaction;
            distanceToPivot = aircraftParams.helperDistanceToPivot;
            rotationIntensity = aircraftParams.helperRotationIntensity;
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

            // transform.rotation =
            //     Quaternion.Euler(new Vector3(0, 0, (thisPosition.x - positionTarget.x) * rotationIntensity));

            transform.rotation = pivot.rotation;
        }

        public void Shoot()
        {
            bulletShooter.Shoot();
        }
    }
}