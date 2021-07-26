using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScrollShooter
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private GameObject explosion;
        [SerializeField] private BulletShooter bulletShooter;
        [SerializeField] private float shootPeriod;
        [SerializeField] private List<Spike> spikes;

        private void Start()
        {
            health.HealthIsChanged += GetDamage;

            foreach (var spike in spikes)
            {
                spike.SpikeActivated += DestroyEnemy;

            }
            
            StartCoroutine(Shooting());
        }

        private void GetDamage(int healthValue)
        {
            if (healthValue <= 0)
            {
                DestroyEnemy();
            }
        }

        private void DestroyEnemy()
        {
            Instantiate(explosion, health.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private IEnumerator Shooting()
        {
            while (true)
            {
                yield return new WaitForSeconds(shootPeriod);
                bulletShooter.Shoot();
            }
        }
    }
}