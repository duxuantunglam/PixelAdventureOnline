using Fusion;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner runner { get; private set; }
    [SerializeField] private NetworkRunner NetworkRunner;
    [SerializeField] private PlayerSpawner playerSpawner;


    // [Header("Scene & GameMode")]
    // [SerializeField] private GameMode gameMode = GameMode.Host;

    private async void StartGame(GameMode gameMode)
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

    private void OnGUI()
    {
        if (runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
}