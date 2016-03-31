using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Models
{
    public class IslandData : NetworkBehaviour
    {
        [SyncVar] public float CurrentRespawnRate;

        public Renderer IslandRenderer;

        [SyncVar] public int MaxSpawn;
        [SyncVar] public string PlayerUuid;

        public GameObject ShipType;

        /// spawn per second
        [SyncVar] public float SpawnRate;

        [SyncVar] public string Uuid;

        public List<ShipData> FriendlyShips
        {
            get { return GetFriendlyShips(this, PlayerUuid); }
        }

        public List<ShipData> UnfriendlyShips
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

        public int Friendly_Ships;
        public int Unfriendly_Ships;

        void Update()
        {
            Friendly_Ships = FriendlyShips.Count;
            Unfriendly_Ships = UnfriendlyShips.Count;
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
        public static float DominancePercentage(IslandData island)
        {
            var friendly = 0;
            var foes = 0;

            for (var i = 0; i < island.transform.childCount; ++i)
            {
                var ship = island.transform.GetChild(i).gameObject;
                var otherShipData = ship.GetComponent<ShipData>(); // possibly cachable
                if (otherShipData == null) continue; // skip non ship gameobjects
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

            return sum == 0 ? 1f : friendly/(float) sum;
        }

        public static List<ShipData> GetFriendlyShips(IslandData island, string playUuid)
        {
            var enemyShips = new List<ShipData>(island.transform.childCount - 1);
            for (var i = 0; i < island.transform.childCount; ++i)
            {
                var ship = island.transform.GetChild(i).gameObject;
                var shipData = ship.GetComponent<ShipData>(); // possibly cachable
                if (shipData == null) continue; // skip non ship gameobjects
                if (shipData.PlayerUuid == playUuid)
                    enemyShips.Add(shipData);
            }
            return enemyShips;
        }

        public static List<ShipData> GetEnemyShips(IslandData island, string thisUid)
        {
            var enemyShips = new List<ShipData>(island.transform.childCount - 1);
            for (var i = 0; i < island.transform.childCount; ++i)
            {
                var ship = island.transform.GetChild(i).gameObject;
                var shipData = ship.GetComponent<ShipData>(); // possibly cachable
                if (shipData == null) continue; // skip non ship gameobjects
                if (shipData.PlayerUuid != thisUid)
                    enemyShips.Add(shipData);
            }
            return enemyShips;
        }

        public static int AmountFriendlyUnits(IslandData island)
        {
            var friendly = 0;

            for (var i = 0; i < island.transform.childCount; ++i)
            {
                var ship = island.transform.GetChild(i).gameObject;
                var otherShipData = ship.GetComponent<ShipData>(); // possibly cachable
                if (otherShipData == null) continue; // skip non ship gameobjects
                if (otherShipData.PlayerUuid == island.PlayerUuid)
                {
                    ++friendly;
                }
            }

            return friendly;
        }
    }
}