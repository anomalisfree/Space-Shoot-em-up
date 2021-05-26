using System.Collections.Generic;
using ScrollShooter.Data;
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


        private readonly List<AircraftHelper> helpers = new List<AircraftHelper>();
        
        private void Start()
        {
            foreach (var itemCollector in itemCollectors)
            {
                itemCollector.GETItemAction += GetItem;
            }
            
            SwitchAircraft(0);
            AddHelpers();
        }

        private void GetItem(ItemType itemType)
        {
            
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
            if (health <= 0)
            {
                DestroyAircraft();
            }
        }

        public void Shoot()
        {
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
