using UnityEngine;

namespace ScrollShooter
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float destroyDistance = 10;
        [SerializeField] private GameObject explosion;
        
        private void Update()
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            
            if (transform.localPosition.z > destroyDistance)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var transformThis = transform;
            Instantiate(explosion, transformThis.position, transformThis.rotation);
            Destroy(gameObject);
        }
    }
}
