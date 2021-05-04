using System;
using UnityEngine;

namespace ScrollShooter
{
    public class Health : MonoBehaviour
    {
        public Action<int> HealthIsChanged;

        [SerializeField] private int health = 100;

        public void Set(int value)
        {
            health = value;
        }

        public void Decrease(int value)
        {
            health -= value;

            if (health < 0)
            {
                health = 0;
            }
            
            HealthIsChanged?.Invoke(health);
        }
    }
}