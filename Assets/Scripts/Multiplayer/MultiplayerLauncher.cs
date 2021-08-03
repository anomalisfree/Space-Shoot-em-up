using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Multiplayer
{
    public class MultiplayerLauncher : MonoBehaviourPunCallbacks
    {
        [Header("Main menu")] 
        [SerializeField] private GameObject mainMenu;

        [Header("Loading screen")] 
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private TextMeshProUGUI loadingText;

        [Header("Create room screen")] 
        [SerializeField] private GameObject createRoomScreen;
        [SerializeField] private TextMeshProUGUI roomNameInputField;

        [Header("Room screen")]
        [SerializeField] private GameObject roomScreen;
        [SerializeField] private TextMeshProUGUI roomNameText;
        [SerializeField] private TextMeshProUGUI playerNameLabel;
        [SerializeField] private Transform playerNameLabelsContent;
        private readonly List<TextMeshProUGUI> allPlayerNames = new List<TextMeshProUGUI>();

        [Header("Error screen")] 
        [SerializeField] private GameObject errorScreen;
        [SerializeField] private TextMeshProUGUI errorText;

        [Header("Room browser screen")] 
        [SerializeField] private GameObject roomBrowserScreen;
        [SerializeField] private RoomButton roomButton;
        [SerializeField] private Transform roomButtonsContent;
        private readonly List<RoomButton> allRoomButtons = new List<RoomButton>();
        
        [Header("Input Name screen")] 
        [SerializeField] private GameObject inputNameScreen;
        [SerializeField] private TextMeshProUGUI nameInputField;
        private bool hasSatNickname;
        
        [Header("Saved params")] 
        [SerializeField] private string nicknameParam = "playerName";

        public static MultiplayerLauncher Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            CloseMenus();
            ShowLoadingMenu("Connecting to network...");
            PhotonNetwork.ConnectUsingSettings();
        }

        private void CloseMenus()
        {
            loadingScreen.SetActive(false);
            mainMenu.SetActive(false);
            createRoomScreen.SetActive(false);
            roomScreen.SetActive(false);
            errorScreen.SetActive(false);
            roomBrowserScreen.SetActive(false);
            inputNameScreen.SetActive(false);
        }

        private void ShowMainMenu()
        {
            CloseMenus();
            mainMenu.SetActive(true);
        }

        private void ShowLoadingMenu(string text)
        {
            loadingScreen.SetActive(true);
            loadingText.text = text;
        }

        private void ShowErrorMenu(string text)
        {
            errorScreen.SetActive(true);
            errorText.text = text;
        }

        public override void OnConnectedToMaster()
        {
            CloseMenus();
            ShowLoadingMenu("Joining lobby...");
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            if (!hasSatNickname)
            {
                CloseMenus();
                inputNameScreen.SetActive(true);

                if (PlayerPrefs.HasKey(nicknameParam))
                {
                    nameInputField.text = PlayerPrefs.GetString(nicknameParam);
                }
            }
            else
            {
                ShowMainMenu();
            }
        }

        public void SetNickname()
        {
            if (!string.IsNullOrEmpty(nameInputField.text))
            {
                PhotonNetwork.NickName = nameInputField.text;
                PlayerPrefs.SetString(nicknameParam, nameInputField.text);
                hasSatNickname = true;
                ShowMainMenu();
            }
        }
        
        public void OpenRoomCreateScreen()
        {
            CloseMenus();
            createRoomScreen.SetActive(true);
        }

        public void CreateRoom()
        {
            if (string.IsNullOrEmpty(roomNameInputField.text)) return;

            var roomOptions = new RoomOptions {MaxPlayers = 2};

            CloseMenus();
            ShowLoadingMenu("Creating room...");

            PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions);
        }

        public override void OnJoinedRoom()
        {
            CloseMenus();
            roomScreen.SetActive(true);
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
            ListAllPlayers();
        }

        private void ListAllPlayers()
        {
            foreach (var player in allPlayerNames)
            {
                Destroy(player.gameObject);
            }
            
            allPlayerNames.Clear();
            var players = PhotonNetwork.PlayerList;

            foreach (var player in players)
            {
                AddPlayer(player);
            }
        }

        private void AddPlayer(Player player)
        {
            var newPlayerLabel = Instantiate(playerNameLabel, playerNameLabelsContent);
            newPlayerLabel.text = player.NickName;
            allPlayerNames.Add(newPlayerLabel);
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            AddPlayer(newPlayer);
        }
        
        public override void OnPlayerLeftRoom(Player newPlayer)
        {
            ListAllPlayers();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            CloseMenus();
            ShowErrorMenu($"Failed to create room: {message}");
        }

        public void CloseErrorScreen()
        {
            ShowMainMenu();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            CloseMenus();
            ShowLoadingMenu("Leaving room...");
        }

        public override void OnLeftRoom()
        {
            //ShowMainMenu();
        }

        public void OpenRoomBrowser()
        {
            CloseMenus();
            roomBrowserScreen.SetActive(true);
        }

        public void CloseRoomBrowser()
        {
            ShowMainMenu();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomInfos)
        {
            foreach (var button in allRoomButtons)
            {
                Destroy(button.gameObject);
            }
            
            allRoomButtons.Clear();

            foreach (var info in roomInfos)
            {
                if (info.PlayerCount != info.MaxPlayers && info.RemovedFromList)
                {
                    var newButton = Instantiate(roomButton,roomButtonsContent);
                    newButton.SetButtonDetails(info);
                    allRoomButtons.Add(newButton);
                }
            }
        }

        public void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
            
            CloseMenus();
            ShowLoadingMenu("Joining room...");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}