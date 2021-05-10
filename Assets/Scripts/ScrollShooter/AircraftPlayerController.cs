using System.Collections.Generic;
using UnityEngine;

namespace ScrollShooter
{
    public class AircraftPlayerController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> aircraftBodies;
        [SerializeField] private List<Health> healths;
        [SerializeField] private ParticleSystem deadEffect;


        private void Start()
        {
            SwitchAircraft(0);
        }

        public void DestroyAircraft()
        {
            foreach (var aircraftBody in aircraftBodies)
            {
                aircraftBody.SetActive(false);
            }
            
            deadEffect.Play();
        }

        public void SwitchAircraft(int num)
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
