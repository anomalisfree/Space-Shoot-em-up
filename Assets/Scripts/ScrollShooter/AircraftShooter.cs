using UnityEngine;

namespace ScrollShooter
{
    public class AircraftShooter : MonoBehaviour
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform bulletCreator;
        [SerializeField] private new ParticleSystem particleSystem;

        public void Shoot()
        {
            var bulletObj = Instantiate(bullet, bulletCreator.position, bulletCreator.rotation);
            bulletObj.transform.parent = transform.parent;
            bulletObj.layer = gameObject.layer;
            particleSystem.Play();
        }
    }
}
