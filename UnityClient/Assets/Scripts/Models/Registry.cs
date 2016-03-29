using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Models
{
  public class Registry : MonoBehaviour
  {
    public void Awake()
    {
      Instance = this;
    }

    public static Registry Instance { get; private set; }

    public static class Levels
    {
      public const string MainMenuAndIntro = "MainMenuAndIntro";
      public const string TrainingsLevel = "TrainingsLevel";
      public const string MultiplayerLevel = "MultiplayerLevel";
    }

    [Serializable]
    public struct UniqueGameObject
    {
      public string Uuid;
      public GameObject GameObject;
    }

    [SerializeField] public List<UniqueGameObject> Player;
    [SerializeField] public List<UniqueGameObject> Islands;
    [SerializeField] public List<UniqueGameObject> Ships;

    public void Clear()
    {
      Player.Clear();
      Islands.Clear();
      Ships.Clear();
    }
  }
}