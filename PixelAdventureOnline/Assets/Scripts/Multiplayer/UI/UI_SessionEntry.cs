using UnityEngine;
using TMPro;
using Fusion;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
        NetworkRunnerHandler.instance.ConnectToSession(sessionName.text);
    }
}
