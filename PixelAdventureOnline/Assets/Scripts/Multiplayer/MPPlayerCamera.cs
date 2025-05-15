using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;

public class MPPlayerCamera : MonoBehaviour
{
    [SerializeField] Transform playerCameraRoot;

    private void Start()
    {
        NetworkObject networkObject = GetComponent<NetworkObject>();

        if (networkObject.HasInputAuthority)
        {
            GameObject virtualCamera = GameObject.Find("FollowCamera");
            virtualCamera.GetComponent<CinemachineCamera>().Follow = playerCameraRoot;
        }
    }
}