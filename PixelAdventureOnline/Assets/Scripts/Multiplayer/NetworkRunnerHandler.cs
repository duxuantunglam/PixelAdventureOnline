using Fusion;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour
{
    public static NetworkRunnerHandler instance { get; private set; }

    public NetworkRunner runner { get; private set; }

    [SerializeField] private NetworkRunner NetworkRunner;
    [SerializeField] private PlayerSpawner playerSpawner;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // private async void Start()
    // {
    //     await StartGame(GameMode.AutoHostOrClient);
    // }

    public async void ConnectToSession(string sessionName)
    {
        if (runner != null)
        {
           runner = gameObject.AddComponent<NetworkRunner>();
        }
        
        runner.ProvideInput = true;

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        try
        {
            await runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = sessionName,
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });

            Debug.Log("[Fusion] runner Started: " + GameMode.AutoHostOrClient);

            runner.AddCallbacks(playerSpawner);
        }
        catch (Exception e)
        {
            Debug.LogError("[Fusion] Error Starting runner: " + e);
        }
    }
}