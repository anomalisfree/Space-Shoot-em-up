using System.Collections.Generic;
using ScrollShooter.Data;
using ScrollShooter.ScriptableObjects;
using UnityEngine;

namespace ScrollShooter
{
    public class AircraftPlayerController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> aircraftBodies;
        [SerializeField] private List<Health> healths;
        [SerializeField] private ParticleSystem deadEffect;
        [SerializeField] private AircraftHelper helper;
        [SerializeField] private BulletShooter bulletShooter;
        [SerializeField] private AircraftMovement aircraftMovement;
        [SerializeField] private List<ItemCollector> itemCollectors;
        [SerializeField] private GameObject shield;

        private readonly List<AircraftHelper> helpers = new List<AircraftHelper>();
        private float shieldTimer;
        private bool inShield;
        private int oldHealth;
        private bool canShoot;
        private float shieldTime;
        
        public void Initialize(AircraftParams aircraftParams)
        {
            oldHealth = aircraftParams.healthOnStart;
            
            foreach (var healthComponent in healths)
            {
                healthComponent.Set(oldHealth);
            }

            shieldTime = aircraftParams.shieldTime;
            
            aircraftMovement.Initialize(aircraftParams);
            SwitchAircraft((int)aircraftParams.aircraftBody);
            AddHelpers();
        }
        
        private void Start()
        {
            foreach (var itemCollector in itemCollectors)
            {
                itemCollector.GETItemAction += GetItem;
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

        private void GetItem(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Shield:
                    shieldTimer = shieldTime;
                    shield.SetActive(true);
                    inShield = true;
                    break;
                case ItemType.Force:
                    break;
                case ItemType.Helpers:
                    break;
            }
        }

        private void AddHelpers()
        {
            ClearHelpers();

            var rightHelper = Instantiate(helper);
            rightHelper.Initialize(transform, HelperPosition.Right);
            rightHelper.IsDead += DestroyHelper;
            helpers.Add(rightHelper);

            var leftHelper = Instantiate(helper);
            leftHelper.Initialize(transform, HelperPosition.Left);
            leftHelper.IsDead += DestroyHelper;
            helpers.Add(leftHelper);
        }

        private void DestroyHelper(AircraftHelper aircraftHelper)
        {
            helpers.Remove(aircraftHelper);
        }

        private void DestroyAircraft()
        {
            foreach (var aircraftBody in aircraftBodies)
            {
                aircraftBody.SetActive(false);
            }

            canShoot = false;
            ClearHelpers();
            deadEffect.Play();
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