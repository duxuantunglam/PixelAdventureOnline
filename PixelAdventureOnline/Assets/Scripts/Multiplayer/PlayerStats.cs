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

    [Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_32> PlayerName { get; set; }

    [SerializeField] public TextMeshPro playerNameLabel;

    private void Start()
    {
        if (this.HasStateAuthority)
        {
            PlayerName = FusionManager.instance._playerName;
            if (instance == null)
            {
                instance = this;
            }
        }
    }

    protected static void UpdatePlayerName(Changed<PlayerStats> changed)
    {
        changed.Behaviour.playerNameLabel.text = changed.Behaviour.PlayerName.ToString();
    }
}