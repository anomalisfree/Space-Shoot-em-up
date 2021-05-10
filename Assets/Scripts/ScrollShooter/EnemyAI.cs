using System.Collections;
using UnityEngine;

namespace ScrollShooter
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private GameObject explosion;
        [SerializeField] private BulletShooter bulletShooter;
        [SerializeField] private float shootPeriod = 0.4f;

        private void Start()
        {
            health.HealthIsChanged += GetDamage;
            StartCoroutine(Shooting());
        }

        private void GetDamage(int healthValue)
        {
            if (healthValue <= 0)
            {
                Instantiate(explosion, health.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
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