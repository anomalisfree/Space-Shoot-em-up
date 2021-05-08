using UnityEngine;

namespace ScrollShooter
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private GameObject explosion;

        private void Start()
        {
            health.HealthIsChanged += GetDamage;
        }

        private void GetDamage(int healthValue)
        {
            if (healthValue <= 0)
            {
                Instantiate(explosion, health.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}