using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using PixelAdventureOnline.FusionBites;
using TMPro;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    public static PlayerStats instance;

    [Networked] public NetworkString<_32> PlayerName { get; set; }

    [SerializeField] public TextMeshProUGUI playerNameLabel;

    private NetworkString<_32> previousPlayerName;

    public override void FixedUpdateNetwork()
    {
        if (previousPlayerName != PlayerName)
        {
            UpdatePlayerName();
            previousPlayerName = PlayerName;
        }
    }

    private void UpdatePlayerName()
    {
        playerNameLabel.text = PlayerName.ToString();
    }

    public override void Spawned()
    {
        previousPlayerName = PlayerName;
        UpdatePlayerName();

        if (HasStateAuthority)
        {
            PlayerName = FusionManager.instance._playerName;
        }
    }
}