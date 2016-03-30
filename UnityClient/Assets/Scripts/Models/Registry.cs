using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Models
{
  public class Registry : Singleton<Registry>
  {
    public static class Levels
    {
      public const string MainMenuAndIntro = "MainMenuAndIntro";
      public const string TrainingsLevel = "TrainingsLevel";
      public const string MultiplayerLevel = "MultiplayerLedvel";
    }

    [SerializeField] public List<PlayerData> Player;
    [SerializeField] public List<IslandData> Islands;
    [SerializeField] public List<ShipData> Ships;

    public void Clear()
    {
      Player.Clear();
      Islands.Clear();
      Ships.Clear();
    }
  }
}