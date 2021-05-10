using System;
using UnityEngine;

namespace ScrollShooter
{
    public class Health : MonoBehaviour
    {
        public Action<int> HealthIsChanged;

        [SerializeField] private int health = 100;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Color damageColor = Color.red;

        private float damageTimer;
        private Color defaultColor;

        private void Start()
        {
            if (meshRenderer != null)
            {
                defaultColor = meshRenderer.material.color;
            }
        }

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

            damageTimer = 0.1f;
            HealthIsChanged?.Invoke(health);
        }

        private void Update()
        {
            if (meshRenderer == null) return;
            
            if (damageTimer > 0)
            {
                damageTimer -= Time.deltaTime;
                meshRenderer.material.color = damageColor;
            }
            else
            {
                meshRenderer.material.color = defaultColor;
            }
        }
    }
}