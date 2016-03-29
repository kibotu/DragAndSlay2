using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class PlayerData : MonoBehaviour
    {
        public string Uid;
        public int FbId;
        public Color Color;
        public int Level;
        public int Xp;
        public int Currancy;
        public int HardCurrancy;
        public int GamesPlayed;
        public int GamesWon;
        public int GamesLost;
        public int GamesLeft;
        public int[] Friendlist;
        public PlayerType Type;

        public enum PlayerType
        {
            Player, Offensive, Neutral
        }

        public static PlayerType GetPlayerType(string type)
        {
            switch (type)
            {
                case "player" : return PlayerType.Player;
                case "offensive": return PlayerType.Offensive;
                case "neutral":
                default: return PlayerType.Neutral;
            }
        }
    }
}