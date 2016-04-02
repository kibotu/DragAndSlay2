using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Registry : Singleton<Registry>
    {
        [SerializeField] public List<Island> Islands;

        [SerializeField] public List<Player> Player;
        [SerializeField] public List<Ship> Ships;

        public void Clear()
        {
            Player.Clear();
            Islands.Clear();
            Ships.Clear();
        }

        public static class Levels
        {
            public const string MainMenuAndIntro = "MainMenuAndIntro";
            public const string TrainingsLevel = "TrainingsLevel";
            public const string MultiplayerLevel = "MultiplayerLedvel";
        }
    }
}