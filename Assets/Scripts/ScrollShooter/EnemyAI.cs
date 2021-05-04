using UnityEngine;

namespace ScrollShooter
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Health health;

        private void Start()
        {
            health.HealthIsChanged += GetDamage;
        }

        private void GetDamage(int obj)
        {
            Debug.Log("Enemy get damage");
        }
    }
}
