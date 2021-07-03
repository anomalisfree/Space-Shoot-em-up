using System.Collections.Generic;
using ScrollShooter.Data;
using ScrollShooter.Effects;
using ScrollShooter.ScriptableObjects;
using UnityEngine;

namespace ScrollShooter
{
    public class AircraftPlayerController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> aircraftBodies;
        [SerializeField] private List<Health> healths;
        [SerializeField] private ParticleSystem deadEffect;
        [SerializeField] private EnvironmentDistortion environmentDistortion;
        [SerializeField] private AircraftHelper helper;
        [SerializeField] private BulletShooter bulletShooter;
        [SerializeField] private AircraftMovement aircraftMovement;
        [SerializeField] private List<ItemCollector> itemCollectors;
        [SerializeField] private GameObject shield;
        
        [SerializeField] private List<Transform> bulletCreatorsLowGroup;
        [SerializeField] private List<Transform> bulletCreatorsMediumGroup;

        private readonly List<AircraftHelper> helpers = new List<AircraftHelper>();
        private AircraftParams aircraftParams;
        private float shieldTimer;
        private bool inShield;
        private int oldHealth;
        private bool canShoot;
        private float shieldTime;
        
        public void Initialize(AircraftParams aircraftData)
        {
            aircraftParams = aircraftData;
            oldHealth = aircraftData.healthOnStart;
            
            foreach (var healthComponent in healths)
            {
                healthComponent.Set(oldHealth);
            }

            shieldTime = aircraftData.shieldTime;
            
            aircraftMovement.Initialize(aircraftData);
            SwitchAircraft((int)aircraftData.aircraftBody);
        }
        
        private void Start()
        {
            foreach (var itemCollector in itemCollectors)
            {
                itemCollector.GETPowerUpAction += GetPowerUp;
            }
        }

        private void Update()
        {
            UpdateShieldTimer();
        }

        private void UpdateShieldTimer()
        {
            if (!shield.activeSelf) return;

            if (shieldTimer > 0)
            {
                shieldTimer -= Time.deltaTime;
            }
            else
            {
                shield.SetActive(false);
                inShield = false;
            }
        }

        private void GetPowerUp(PowerUpType powerUpType)
        {
            switch (powerUpType)
            {
                case PowerUpType.Shield:
                    shieldTimer = shieldTime;
                    shield.SetActive(true);
                    inShield = true;
                    break;
                case PowerUpType.Force:
                    bulletShooter.UpdateBulletCreatorsGroup(bulletCreatorsMediumGroup);
                    break;
                case PowerUpType.Helpers:
                    AddHelpers();
                    break;
            }
        }

        private void AddHelpers()
        {
            ClearHelpers();

            var rightHelper = Instantiate(helper);
            rightHelper.Initialize(transform, HelperPosition.Right, aircraftParams);
            rightHelper.IsDead += DestroyHelper;
            helpers.Add(rightHelper);

            var leftHelper = Instantiate(helper);
            leftHelper.Initialize(transform, HelperPosition.Left, aircraftParams);
            leftHelper.IsDead += DestroyHelper;
            helpers.Add(leftHelper);
        }

        private void DestroyHelper(AircraftHelper aircraftHelper)
        {
            helpers.Remove(aircraftHelper);
        }

        private void DestroyAircraft()
        {
            bulletShooter.UpdateBulletCreatorsGroup(bulletCreatorsLowGroup);

            foreach (var aircraftBody in aircraftBodies)
            {
                aircraftBody.SetActive(false);
            }

            canShoot = false;
            ClearHelpers();
            deadEffect.Play();
            //Instantiate(environmentDistortion, transform.position, Quaternion.identity).Initialize(3);
        }

        private void ClearHelpers()
        {
            foreach (var aircraftHelper in helpers)
            {
                aircraftHelper.IsDead -= DestroyHelper;
                aircraftHelper.Dead();
            }

            helpers.Clear();
        }

        private void SwitchAircraft(int num)
        {
            canShoot = true;
            
            for (var i = 0; i < aircraftBodies.Count; i++)
            {
                aircraftBodies[i].SetActive(i == num);

                if (i == num)
                {
                    healths[i].HealthIsChanged += HealthIsChanged;
                }
                else
                {
                    healths[i].HealthIsChanged -= HealthIsChanged;
                }
            }
        }

        private void HealthIsChanged(int health)
        {
            if (!inShield)
            {
                oldHealth = health;
            }

            foreach (var healthComponent in healths)
            {
                healthComponent.Set(oldHealth);
            }

            if (oldHealth <= 0)
            {
                DestroyAircraft();
            }
        }

        public void Shoot()
        {
            if (!canShoot) return;
            
            bulletShooter.Shoot();

            foreach (var aircraftHelper in helpers)
            {
                aircraftHelper.Shoot();
            }
        }

        public void Movement(Vector2 direction, float blindArea = 0)
        {
            aircraftMovement.HorizontalMoving(direction.x, blindArea);
            aircraftMovement.VerticalMoving(direction.y, blindArea);
        }
    }
}