using System;
using System.Linq;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Network;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Models
{
    public class Player : CustomNetworkBehaviour
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

            name = "Player " + uuid;

            foreach (
                var islandData in
                    Registry.Instance.Islands.Where(islandData => string.IsNullOrEmpty(islandData.PlayerUuid)))
            {
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
            // Debug.Log("[CmdSendUnits] " + sourceIslandUuid + " to " + targetIslandUuid);

            var source = Registry.Instance.Islands.Find(data => data.Uuid.Equals(sourceIslandUuid));
            var target = Registry.Instance.Islands.Find(data => data.Uuid.Equals(targetIslandUuid));

            RpcSendUnits(source.gameObject, target.gameObject, Uuid);
        }

        [ClientRpc]
        private void RpcSendUnits(GameObject source, GameObject target, string playerUuid)
        {
            Debug.Log("[RpcSendUnits] " + source.name + " to " + target.name);
            // Debug.Log("[RpcSendUnits] Player " + playerUuid);

            var ships = Island.GetFriendlyShips(source.GetComponent<Island>(), playerUuid);

            foreach (var moveToTarget in ships.Select(playerShip => playerShip.GetComponent<MoveToTarget>()))
            {
                moveToTarget.Target = target.gameObject;
                moveToTarget.enabled = true;
            }
        }

        public override void OnNetworkDestroy()
        {
            if(!Registry.ApplicationIsQuitting)
               Registry.Instance.Remove(this);
        }

        [ClientRpc]
        public void RpcShowExplosionAt(string shipUuid)
        {
            Debug.Log("[RpcShowExplosionAt] " + shipUuid);
            var defender = Registry.Instance.Ships.Find(ship => ship.Uuid.Equals(shipUuid));
            Debug.Log("[RpcShowExplosionAt] " + defender.name);
            var explosion = Prefabs.Instance.GetNewSmallExplosion();
            explosion.transform.position = defender.transform.position;
            Destroy(defender.gameObject);
        }
    }
}