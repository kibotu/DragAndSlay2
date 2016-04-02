using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Registry : Singleton<Registry>
    {
        [SerializeField] public List<Island> Islands;

        [SerializeField] public List<Player> Player;
        [SerializeField] public List<Ship> Ships;

        public Player CurrentPlayer
        {
            get { return Instance.Player.FirstOrDefault(player => player.isLocalPlayer); }
        }

        public void Clear()
        {
            Player.Clear();
            Islands.Clear();
            Ships.Clear();
        }

        public void Remove(Island island)
        {
            Instance.Islands.Remove(Instance.Islands.First(item => item.Uuid.Equals(island.Uuid)));
        }

        public void Remove(Ship island)
        {
            Instance.Ships.Remove(Instance.Ships.First(item => item.Uuid.Equals(island.Uuid)));
        }

        public void Remove(Player player)
        {
            Instance.Player.Remove(Instance.Player.First(item => item.Uuid == player.Uuid));
        }

        public static class Levels
        {
            public const string MainMenuAndIntro = "MainMenuAndIntro";
            public const string TrainingsLevel = "TrainingsLevel";
            public const string MultiplayerLevel = "MultiplayerLedvel";
        }
    }
}