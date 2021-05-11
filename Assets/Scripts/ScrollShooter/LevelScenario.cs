using System.Collections;
using UnityEngine;

namespace ScrollShooter
{
    public class LevelScenario : MonoBehaviour
    {
        [SerializeField] private GameObject ufoPrefab;
        [SerializeField] private Transform field;

        private void Start()
        {
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