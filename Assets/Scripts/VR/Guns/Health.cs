using System;
using UnityEngine;

namespace VR.Guns
{
    public class Health : MonoBehaviour
    {
       [SerializeField] private float health = 100;
       public Action<float> HealthChanged;

       public void GetDamage(float damage)
       {
           health -= damage;
           HealthChanged?.Invoke(health);
       }
    }
}
