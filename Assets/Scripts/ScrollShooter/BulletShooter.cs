using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScrollShooter
{
    public class BulletShooter : MonoBehaviour
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private List<Transform> bulletCreators;
        [SerializeField] private List<ParticleSystem> bulletCreatorParticleSystems;
        [SerializeField] private Color bulletColor;

        public void Shoot()
        {
            foreach (var bulletObj in bulletCreators.Select(bulletCreator =>
                Instantiate(bullet, bulletCreator.position, bulletCreator.rotation)))
            {
                bulletObj.layer = gameObject.layer;
                bulletObj.GetComponent<Bullet>().Initialize(bulletColor);
            }

            foreach (var bulletCreatorParticleSystem in bulletCreatorParticleSystems)
            {
                var mainModule = bulletCreatorParticleSystem.main;
                mainModule.startColor = bulletColor;
                bulletCreatorParticleSystem.Play();
            }
        }
    }
}