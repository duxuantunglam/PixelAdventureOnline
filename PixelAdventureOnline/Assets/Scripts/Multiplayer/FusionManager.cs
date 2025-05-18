using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.UI;

namespace PixelAdventureOnline.FusionBites
{
    public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static FusionManager instance { get; private set; }

        public NetworkRunner runner;

        [SerializeField] private NetworkObject playerPrefab;

        public string _playerName = null;

        [SerializeField] private GameObject userMainMenu;

        [Header("Session List")]
        [SerializeField] private GameObject roomListCanvas;
        private List<SessionInfo> sessions = new List<SessionInfo>();
        [SerializeField] private Button refreshButton;
        [SerializeField] private Transform sessionListContent;
        [SerializeField] private GameObject sessionEntry;

        private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void OnPlayGameButtonClicked()
        {
            userMainMenu.SetActive(false);
            roomListCanvas.SetActive(true);
        }

        public void ConnectToLobby(string playerName)
        {
            roomListCanvas.SetActive(true);
            _playerName = playerName;

            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            runner.JoinSessionLobby(SessionLobby.Shared);
        }

        public async void CreateSession()
        {
            roomListCanvas.SetActive(false);

            int randomInt = UnityEngine.Random.Range(1000, 9999);
            string randomSessionName = "Room" + randomInt.ToString();

            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            var sceneRef = SceneRef.FromIndex(1);

            await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = randomSessionName,
                PlayerCount = 2,
                Scene = sceneRef
            });
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            sessions.Clear();
            sessions = sessionList;
        }

        public void RefreshSessionListUI()
        {
            foreach (Transform child in sessionListContent)
            {
                Destroy(child.gameObject);
            }

            foreach (SessionInfo session in sessions)
            {
                if (session.IsVisible)
                {
                    GameObject entry = GameObject.Instantiate(sessionEntry, sessionListContent);
                    UI_SessionEntry sessionEntryScript = entry.GetComponent<UI_SessionEntry>();
                    sessionEntryScript.sessionName.text = session.Name;
                    sessionEntryScript.playerCount.text = $"{session.PlayerCount}/{session.MaxPlayers}";

                    if (session.IsOpen == false || session.PlayerCount >= session.MaxPlayers)
                    {
                        sessionEntryScript.joinButton.interactable = false;
                    }
                    else
                    {
                        sessionEntryScript.joinButton.interactable = true;
                        // sessionEntryScript.joinButton.onClick.AddListener(() => NetworkRunnerHandler.instance.ConnectToSession(session.Name));
                    }
                }
            }
        }

        public async void ConnectToSession(string sessionName)
        {
            roomListCanvas.SetActive(false);

            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = sessionName,
                PlayerCount = 2,
            });
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("OnConnectedToServer");

            NetworkObject playerObject = runner.Spawn(playerPrefab, Vector3.zero);

            runner.SetPlayerObject(runner.LocalPlayer, playerObject);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer)
                return;

            Vector3 spawnPosition = new Vector3((player.RawEncoded % 4) * 3f, 2f, 0f);

            NetworkObject networkPlayerObject = runner.Spawn(
                playerPrefab,
                spawnPosition,
                Quaternion.identity,
                player
            );

            if (networkPlayerObject != null)
            {
                spawnedCharacters.Add(player, networkPlayerObject);
                Debug.Log($"[Fusion] Spawned player: {player}");
            }
            else
            {
                Debug.LogError("[Fusion] Failed to spawn player!");
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (spawnedCharacters.TryGetValue(player, out NetworkObject obj))
            {
                runner.Despawn(obj);
                spawnedCharacters.Remove(player);
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();

            if (Input.GetKey(KeyCode.A))
                data.direction.x = -1;
            if (Input.GetKey(KeyCode.D))
                data.direction.x = 1;

            data.jump = Input.GetKeyDown(KeyCode.Space);

            input.Set(data);
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {

        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {

        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {

        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {

        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {

        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {

        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {

        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {

        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {

        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {

        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {

        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {

        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {

        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {

        }
    }
}