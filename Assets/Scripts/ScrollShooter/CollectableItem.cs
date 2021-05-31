using ScrollShooter.Data;
using UnityEngine;

namespace ScrollShooter
{
    public class CollectableItem : MonoBehaviour
    {
        [SerializeField] private PowerUpType powerUpType;
        [SerializeField] private GameObject collectParticleSystem;
        private void OnTriggerEnter(Collider other)
        {
            var itemCollector = other.gameObject.GetComponent<ItemCollector>();
            
            if (itemCollector == null) return;
            
            itemCollector.GetItem(powerUpType);
            Instantiate(collectParticleSystem, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}