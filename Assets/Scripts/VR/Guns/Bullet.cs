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

        private void OnCollisionEnter(Collision other)
        {
            var transformThis = transform;
            PhotonNetwork.Instantiate(explosion.name, transformThis.position, transformThis.rotation);
            Destroy(gameObject);
        }
    }
}
