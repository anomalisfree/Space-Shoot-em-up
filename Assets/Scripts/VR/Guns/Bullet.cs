using Photon.Pun;
using UnityEngine;

namespace VR.Guns
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float lifeTime;
        [SerializeField] private GameObject explosion;
        [SerializeField] private float damage;
        

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            if (photonView != null)
            {
                if (Equals(photonView.Owner, PhotonNetwork.LocalPlayer))
                   transform.Translate(Vector3.forward * bulletSpeed);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Health>() != null)
            {
                collision.gameObject.GetComponent<Health>().GetDamage(damage);
            }
            
            PhotonNetwork.Instantiate(explosion.name, collision.contacts[0].point, transform.rotation);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
