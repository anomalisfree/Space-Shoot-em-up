using System.Collections.Generic;
using UnityEngine;

namespace ScrollShooter
{
    public class AircraftPlayerController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> aircraftBodies;
        [SerializeField] private List<Health> healths;
        [SerializeField] private ParticleSystem deadEffect;
        [SerializeField] private AircraftHelper helper;


        private readonly List<AircraftHelper> helpers = new List<AircraftHelper>();
        
        private void Start()
        {
            SwitchAircraft(0);
            
            var rightHelper = Instantiate(helper);
            rightHelper.Initialize(transform, HelperPosition.Right);
            helpers.Add(rightHelper);
            
            var leftHelper = Instantiate(helper);
            leftHelper.Initialize(transform, HelperPosition.Left);
            helpers.Add(leftHelper);
        }

        private void DestroyAircraft()
        {
            foreach (var aircraftBody in aircraftBodies)
            {
                aircraftBody.SetActive(false);
            }
            
            deadEffect.Play();
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
    }
}
