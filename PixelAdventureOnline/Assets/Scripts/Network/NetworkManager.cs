// using Fusion;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class NetworkManager : MonoBehaviour
// {
//     private NetworkRunner _runner;
//     public static NetworkManager Instance { get; private set; }

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private async void StartGame(GameMode mode)
//     {
//         // Tạo NetworkRunner
//         _runner = gameObject.AddComponent<NetworkRunner>();
//         _runner.ProvideInput = true;

//         // Bắt đầu game
//         await _runner.StartGame(new StartGameArgs()
//         {
//             GameMode = mode,
//             SessionName = "PixelAdventure",
//             Scene = SceneManager.GetActiveScene().buildIndex,
//             SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
//         });
//     }

//     public void StartHost()
//     {
//         StartGame(GameMode.Host);
//     }

//     public void StartClient()
//     {
//         StartGame(GameMode.Client);
//     }
// }