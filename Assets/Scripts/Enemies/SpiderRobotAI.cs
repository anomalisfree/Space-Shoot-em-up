using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using VR.Guns;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class SpiderRobotAI : MonoBehaviourPunCallbacks
    {
        [SerializeField] private SpiderRobotController controller;
        [SerializeField] private NavMeshAgent navMeshAgent;
        
        [SerializeField] private GameObject muzzleFlash;
        [SerializeField] private GameObject bullet;
        [SerializeField] private List<Transform> muzzleFlashPoints;

        [SerializeField] private GameObject explosion;

        [SerializeField] private Health health;

        private Transform currentTarget;
        
        
        private void Start()
        {
            if (photonView.IsMine)
            {
                controller.SetMoveTarget(Vector3.zero);
                StartCoroutine(TargetChanger());
                StartCoroutine(ShootCoroutine());
                health.HealthChanged += HealthChanged;
            }
            else
            {
                controller.enabled = false;
                navMeshAgent.enabled = false;
            }
        }

        private void HealthChanged(float healthValue)
        {
            if (healthValue <= 0)
            {
                var transformThis = transform;
                PhotonNetwork.Instantiate(bullet.name, transformThis.position, transformThis.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        private IEnumerator TargetChanger()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1f, 5f));
                controller.SetMoveTarget(new Vector3(Random.Range(-6f, 6f), 0, Random.Range(-6f, 6f)));
                var targets = FindObjectsOfType<EnemyTarget>();
                // if (targets.Length > 0)
                // {
                //     currentTarget = targets[Random.Range(0, targets.Length - 1)].transform;
                //     controller.SetShootTarget(currentTarget.position);
                // }
                
                //controller.SetShootTarget(new Vector3(Random.Range(-2f,2f), Random.Range(0.5f,1.5f), 0));
            }
        }

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                Shoot();
                yield return new WaitForSeconds(0.3f);
                Shoot();
                yield return new WaitForSeconds(0.3f);
                Shoot();
                yield return new WaitForSeconds(0.3f);
                Shoot();
                yield return new WaitForSeconds(0.3f);
                Shoot();
            }
        }

        private void Shoot()
        {
            foreach (var muzzleFlashPoint in muzzleFlashPoints)
            {
                var position = muzzleFlashPoint.position;
                var rotation = muzzleFlashPoint.rotation;
                PhotonNetwork.Instantiate(muzzleFlash.name, position, rotation);
                PhotonNetwork.Instantiate(bullet.name, position, rotation);
            }
        }
    }
}
