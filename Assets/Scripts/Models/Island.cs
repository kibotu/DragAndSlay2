using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Models
{
    public class Island : NetworkBehaviour
    {
        public int _friendlyShips;

        public int _unfriendlyShips;

        [SyncVar] public float CurrentRespawnRate;

        public Renderer IslandRenderer;

        /// <summary>
        ///     Max amount of supported units per island.
        /// </summary>
        [SyncVar] public int MaxSpawn;

        [SyncVar] public string PlayerUuid;

        public GameObject ShipType;

        /// <summary>
        ///     Ship spawn per second.
        /// </summary>
        [SyncVar] public float SpawnRate;

        [SyncVar] public string Uuid;

        public List<Ship> FriendlyShips
        {
            get { return GetFriendlyShips(this, PlayerUuid); }
        }

        public List<Ship> UnfriendlyShips
        {
            get { return GetEnemyShips(this, PlayerUuid); }
        }

        public void Awake()
        {
            Registry.Instance.Islands.Add(this);
            CurrentRespawnRate = SpawnRate;
            Dye();
            Uuid = Guid.NewGuid().ToString();
        }

        public void Update()
        {
            _friendlyShips = FriendlyShips.Count;
            _unfriendlyShips = UnfriendlyShips.Count;
        }

        public void Convert(string uuid)
        {
            PlayerUuid = uuid;
            Dye();
            var shockwave = Prefabs.Instance.GetNewShockwave();
            shockwave.transform.position = transform.position;
//      shockwave.GetComponent<DetonatorShockwave>().color = PlayerData.Color;
        }

        public void Dye()
        {
            // todo set new material
            // IslandRenderer.material =
        }

        /// <returns>Respawn rate in seconds.</returns>
        public float ShipBuildTime()
        {
            var dominance = DominancePercentage(this);
            CurrentRespawnRate = Mathf.Abs(dominance - 1f) < Mathf.Epsilon
                ? SpawnRate
                : SpawnRate + (SpawnRate*(1 - dominance));
            return CurrentRespawnRate;
        }

        /// <summary>
        ///     Computes current respawn timer based on dominance.
        ///     100% if no enemy ships are present.
        ///     0% if at least one enemy ship but no own ships are present.
        /// </summary>
        /// <returns>Respawn rate in percent. Value between 0f and 1f.</returns>
        public static float DominancePercentage(Island island, bool onlyLiving = true)
        {
            var friendly = 0;
            var foes = 0;

            for (var i = 0; i < island.transform.childCount; ++i)
            {
                var ship = island.transform.GetChild(i).gameObject;

                var otherShipData = ship.GetComponent<Ship>(); // possibly cachable
                if (otherShipData == null) continue; // skip non ship gameobjects

                var life = ship.GetComponent<Life>();
                if (onlyLiving && !life.IsAlive && !life.IsDying)
                    continue;

                if (otherShipData.Uuid == island.PlayerUuid)
                {
                    ++friendly;
                }
                else
                {
                    ++foes;
                }
            }

            var sum = foes + friendly;

            return sum == 0
                ? 1f
                : friendly/(float) sum;
        }

        public static List<Ship> GetFriendlyShips(Island island, string playUuid, bool onlyLiving = true)
        {
            var enemyShips = new List<Ship>(island.transform.childCount - 1);
            for (var i = 0; i < island.transform.childCount; ++i)
            {
                var ship = island.transform.GetChild(i).gameObject;

                var shipData = ship.GetComponent<Ship>(); // possibly cachable
                if (shipData == null) continue; // skip non ship gameobjects

                var life = ship.GetComponent<Life>();
                if (onlyLiving && !life.IsAlive && !life.IsDying)
                    continue;

                if (shipData.PlayerUuid == playUuid)
                    enemyShips.Add(shipData);
            }
            return enemyShips;
        }

        public static List<Ship> GetEnemyShips(Island island, string thisUid, bool onlyLiving = true)
        {
            var enemyShips = new List<Ship>(island.transform.childCount - 1);
            for (var i = 0; i < island.transform.childCount; ++i)
            {
                var ship = island.transform.GetChild(i).gameObject;

                var shipData = ship.GetComponent<Ship>(); // possibly cachable
                if (shipData == null) continue; // skip non ship gameobjects

                var life = ship.GetComponent<Life>();
                if (onlyLiving && !life.IsAlive && !life.IsDying)
                    continue;

                if (shipData.PlayerUuid != thisUid)
                    enemyShips.Add(shipData);
            }
            return enemyShips;
        }

        public static int AmountFriendlyUnits(Island island)
        {
            return island.FriendlyShips.Count;
        }

        public void OnDestroy()
        {
            if (!Registry.ApplicationIsQuitting)
                Registry.Instance.Remove(this);
        }
    }
}