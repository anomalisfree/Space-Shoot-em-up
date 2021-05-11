using System;
using UnityEngine;

namespace ScrollShooter
{
   public class Spike : MonoBehaviour
   {
      [SerializeField] private int damage;

      public Action SpikeActivated;

      private void OnCollisionEnter(Collision other)
      {
         if (other.gameObject.GetComponent<Health>() != null)
         {
            other.gameObject.GetComponent<Health>().Decrease(damage);
         }
         
         SpikeActivated?.Invoke();
      }
   }
}
