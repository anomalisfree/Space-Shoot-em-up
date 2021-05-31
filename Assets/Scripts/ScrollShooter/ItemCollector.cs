using System;
using ScrollShooter.Data;
using UnityEngine;

namespace ScrollShooter
{
    public class ItemCollector : MonoBehaviour
    {
        public Action<PowerUpType> GETPowerUpAction;
        public void GetItem(PowerUpType powerUpType)
        {
            GETPowerUpAction?.Invoke(powerUpType);
        }
    }
}
