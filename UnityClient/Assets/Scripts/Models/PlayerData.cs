using System;
using System.Linq;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Network;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Models
{
    public class PlayerData : CustomNetworkBehaviour
    {
        public enum PlayerType
        {
            Player,
            Offensive,
            Neutral
        }

        public Color Color;
        [SyncVar] public int Currancy;
        [SyncVar] public int FbId;
        public int[] Friendlist;
        [SyncVar] public int GamesLeft;
        [SyncVar] public int GamesLost;
        [SyncVar] public int GamesPlayed;
        [SyncVar] public int GamesWon;
        [SyncVar] public int HardCurrancy;
        [SyncVar] public int Level;

        [SyncVar] public bool Spawned;
        [SyncVar] public string Uuid;
        [SyncVar] public int Xp;

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

        public override string ToString()
        {
            return "Uuid=" + Uuid;
        }

        [Command]
        public void CmdSendUnits(string sourceIslandUuid, string targetIslandUuid)
        {
            Debug.Log("[CmdSendUnits] " + sourceIslandUuid + " to " + targetIslandUuid);

            var source = Registry.Instance.Islands.Find(data => data.Uuid.Equals(sourceIslandUuid));
            var target = Registry.Instance.Islands.Find(data => data.Uuid.Equals(targetIslandUuid));

            RpcSendUnits(source.gameObject, target.gameObject, Uuid);
        }

        [ClientRpc]
        private void RpcSendUnits(GameObject source, GameObject target, string playerUuid)
        {
            Debug.Log("[RpcSendUnits] " + source.name + " to " + target.name);
            Debug.Log("[RpcSendUnits] Player " + playerUuid);

            var ships = IslandData.GetFriendlyShips(source.GetComponent<IslandData>(), playerUuid);

            Debug.Log("ships: " + ships.Count);

            foreach (var moveToTarget in ships.Select(playerShip => playerShip.GetComponent<MoveToTarget>()))
            {
                moveToTarget.Target = target.gameObject;
                moveToTarget.enabled = true;
            }
        }
    }
}