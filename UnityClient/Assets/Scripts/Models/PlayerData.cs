using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Network;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Models
{
  public class PlayerData : CustomNetworkBehaviour
  {
    [SyncVar] public string Uuid;
    public Color Color;
    [SyncVar] public int FbId;
    [SyncVar] public int Level;
    [SyncVar] public int Xp;
    [SyncVar] public int Currancy;
    [SyncVar] public int HardCurrancy;
    [SyncVar] public int GamesPlayed;
    [SyncVar] public int GamesWon;
    [SyncVar] public int GamesLost;
    [SyncVar] public int GamesLeft;
    public int[] Friendlist;

    [SyncVar] public bool Spawned;

    public enum PlayerType
    {
      Player,
      Offensive,
      Neutral
    }

    public static PlayerType GetPlayerType(string type)
    {
      switch (type)
      {
        case "player":
          return PlayerType.Player;
        case "offensive":
          return PlayerType.Offensive;
        case "neutral":
        default:
          return PlayerType.Neutral;
      }
    }

    public void Awake()
    {
      Registry.Instance.Player.Add(this);
    }

    public override void OnStartLocalPlayer()
    {
      CmdSpawn(Guid.NewGuid().ToString());
    }

    [Command]
    public void CmdSpawn(string uuid)
    {
      Debug.Log("[CmdSpawn] " + gameObject.name + " " + uuid);
      Uuid = uuid;

      foreach (var islandData in Registry.Instance.Islands)
      {
        if (!string.IsNullOrEmpty(islandData.PlayerUuid)) continue;

        islandData.PlayerUuid = uuid;
        Spawned = true;

        Game.StartGame();

        return;
      }

      // Registry.Instance.Islands.First(data => !string.IsNullOrEmpty(data.PlayerUuid)).PlayerUuid = uuid;
    }
  }
}