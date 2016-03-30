using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class WorldData : MonoBehaviour
    {

        public void Awake()
        {
            Registry.Instance.Clear();

//            if (Game.IsSinglePlayer())
//            {
//                var player = GameObject.Find("Player");
//                player.GetComponent<PlayerData>().playerType = PlayerData.PlayerType.Player;
//                player.GetComponent<PlayerData>().Color = new Color(0f / 255, 0, 155f / 255) + new Color(0.4f, 0.4f, 0.4f);
//                Registry.Player.Add(player.GetComponent<PlayerData>().Uid, player);
//
//                player = GameObject.Find("Neutral");
//                player.GetComponent<PlayerData>().playerType = PlayerData.PlayerType.Neutral;
//                player.GetComponent<PlayerData>().Color = new Color(155f/255, 140f/255, 60f/255) + new Color(0.4f,0.4f,0.4f);
//                Registry.Player.Add(player.GetComponent<PlayerData>().Uid, player);
//
//                player = GameObject.Find("Offensive");
//                player.GetComponent<PlayerData>().playerType = PlayerData.PlayerType.Offensive;
//                player.GetComponent<PlayerData>().Color = new Color(155f/255, 0f/255, 0f/255) + new Color(0.6f,0.6f,0.6f);
//                Registry.Player.Add(player.GetComponent<PlayerData>().Uid, player);
//
//                // GameObject.Find("Menu").GetComponent<Menu>().ShowGameHud();
//            }
        }

        public IEnumerable<Color> GetPlayerColor()
        {
            yield return Color.green;
            yield return Color.red;
            yield return Color.yellow;
        }

        private static int _playerColorNumber = 0;

        public Color GetNextPlayerColor()
        {
            switch (_playerColorNumber)
            {
                case 0:
                    _playerColorNumber++;
                    return Color.green;
                case 1:
                    _playerColorNumber++;
                    return Color.red;
                case 2:
                    _playerColorNumber++;
                    return Color.yellow;
                default:
                    return Color.clear;
            }
        }
    }
}