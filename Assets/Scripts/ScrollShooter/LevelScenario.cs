using System.Collections;
using ScrollShooter.ScriptableObjects;
using UnityEngine;

namespace ScrollShooter
{
    public class LevelScenario : MonoBehaviour
    {
        [SerializeField] private AircraftParams aircraftParams;
        [SerializeField] private AircraftPlayerController aircraftPlayerController;
        [SerializeField] private Transform field;
        [SerializeField] private GameObject ufoPrefab;

        private void Start()
        {
            aircraftPlayerController.Initialize(aircraftParams);
            StartCoroutine(UFOWaveOne());
        }

        private IEnumerator UFOWaveOne()
        {
            yield return new WaitForSeconds(3f);
            
            var count = 3;
            while (count > 0)
            {
                yield return new WaitForSeconds(0.3f);
                Instantiate(ufoPrefab, field);
                count--;
            }
        }
    }
}