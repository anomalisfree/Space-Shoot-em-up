using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Multiplayer
{
    public class MatchManager : MonoBehaviour
    {
        public static MatchManager Instance;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
