using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Multiplayer
{
    public class RoomButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI buttonText;
        
        private RoomInfo info;

        public void SetButtonDetails(RoomInfo roomInfo)
        {
            info = roomInfo;
            buttonText.text = info.Name;
        }

        public void OpenRoom()
        {
            MultiplayerLauncher.Instance.JoinRoom(info);
        }
    }
}
