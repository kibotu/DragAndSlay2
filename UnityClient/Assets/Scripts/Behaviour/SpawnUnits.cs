using System;
using Assets.Scripts.Models;
using Assets.Scripts.Network;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Behaviour
{
    public class SpawnUnits : CustomNetworkBehaviour
    {
        private Island _island;
        [SerializeField] private float _startTime;

        private void Awake()
        {
            _island = GetComponent<Island>();
        }

        private void Update()
        {
            if (!isServer)
                return;

            // 1) check against polulation limits
            if (HasReachedMaxSpawn())
            {
                _startTime = 0;
                return;
            }

            // 2) check against spawn time
            _startTime += Time.deltaTime;
            if (_startTime < 1/_island.ShipBuildTime()) return;

            _startTime -= 1/_island.ShipBuildTime();

            // 3) trigger spawn
            CmdSpawn();
        }

        [Command]
        private void CmdSpawn()
        {
            // 1) create ship by type
            var ship = Instantiate(_island.ShipType, transform.position, Quaternion.identity) as GameObject;

            // 2) set ship data
            var shipData = ship.GetComponent<Ship>();
            shipData.PlayerUuid = _island.PlayerUuid;
            shipData.Uuid = Guid.NewGuid().ToString();

            // 3) spawn on all clients
            NetworkServer.Spawn(ship);

            // 4) append ship at island
            RpcParenting(ship, transform.gameObject);
        }

        [ClientRpc]
        private void RpcParenting(GameObject child, GameObject parent)
        {
            child.transform.parent = parent.transform;

            var orbitting = child.GetComponent<Orbiting>();
            orbitting.Center = parent.transform;
            orbitting.enabled = true;
        }

        private bool HasReachedMaxSpawn()
        {
            return Game.MaxPopulationLimitReached || Island.AmountFriendlyUnits(_island) >= _island.MaxSpawn;
        }
    }
}