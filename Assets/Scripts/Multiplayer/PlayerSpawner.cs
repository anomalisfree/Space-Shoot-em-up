using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Multiplayer
{
    public class PlayerSpawner : MonoBehaviour
    {
        public static PlayerSpawner Instance;

        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
        
        [SerializeField] private GameObject gunPrefab;
        [SerializeField] private List<Transform> gunPoints;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                SpawnPlayer();
            }
        }

        private void SpawnPlayer()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[0].position, spawnPoints[0].rotation);
                //PhotonNetwork.Instantiate(gunPrefab.name, gunPoints[0].position, gunPoints[0].rotation);
            }
            else
            {
                PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[1].position, spawnPoints[1].rotation);
                //PhotonNetwork.Instantiate(gunPrefab.name, gunPoints[1].position,  gunPoints[0].rotation);
            }
        }
    }
}