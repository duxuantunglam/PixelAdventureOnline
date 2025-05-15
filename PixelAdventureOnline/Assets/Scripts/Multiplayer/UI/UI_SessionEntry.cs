using System.Collections;
using System.Collections.Generic;
using Fusion;
using PixelAdventureOnline.FusionBites;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SessionEntry : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI sessionName;
    [SerializeField] public TextMeshProUGUI playerCount;
    [SerializeField] public Button joinButton;

    private void Awake()
    {
        joinButton.onClick.AddListener(JoinRoom);
    }

    private void Start()
    {
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
    }

    private void JoinRoom()
    {
        FusionManager.instance.ConnectToSession(sessionName.text);
    }
}
