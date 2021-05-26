using ScrollShooter.Data;
using UnityEngine;

namespace ScrollShooter
{
    public class CollectableItem : MonoBehaviour
    {
        [SerializeField] private ItemType itemType;
        [SerializeField] private GameObject collectParticleSystem;
        private void OnTriggerEnter(Collider other)
        {
            var itemCollector = other.gameObject.GetComponent<ItemCollector>();
            
            if (itemCollector == null) return;
            
            itemCollector.GetItem(itemType);
            Instantiate(collectParticleSystem, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}