using Fusion;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner runner { get; private set; }
    [SerializeField] private NetworkRunner NetworkRunner;
    [SerializeField] private PlayerSpawner playerSpawner;

    private async void Start()
    {
        await StartGame(GameMode.AutoHostOrClient);
    }

    private async System.Threading.Tasks.Task StartGame(GameMode gameMode)
    {
        runner = gameObject.AddComponent<NetworkRunner>();
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
                GameMode = gameMode,
                SessionName = "TestRoom",
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });

            Debug.Log("[Fusion] runner Started: " + gameMode);

            runner.AddCallbacks(playerSpawner);
        }
        catch (Exception e)
        {
            Debug.LogError("[Fusion] Error Starting runner: " + e);
        }
    }
}