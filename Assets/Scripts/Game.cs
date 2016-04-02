using System;
using System.Collections;
using System.Linq;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class Game : NetworkBehaviour
    {
        [SyncVar] public readonly int MaxPopulationLimit = 50;
        [SerializeField] private readonly int _playerCount = 2;

        public bool MaxPopulationLimitReached
        {
            get { return Registry.Instance.Ships.Count >= MaxPopulationLimit; }
        }

        private void OnDestroy()
        {
            if (isServer)
                NetworkManager.singleton.StopHost();
        }

        public void StartGame()
        {
            if (!isServer)
                return;

            if (!AllClientsHaveSpawned())
                return;

            StartSpawning();
        }

        private void StartSpawning()
        {
            Debug.Log("[StartSpawning]");

            foreach (var islandData in Registry.Instance.Islands)
            {
                islandData.GetComponent<SpawnUnits>().enabled = true;
            }
        }

        private bool AllClientsHaveSpawned()
        {
            if (Registry.Instance.Player.Count != _playerCount)
                return false;

            var allHaveSpawned = false;

            foreach (var playerData in Registry.Instance.Player.Where(playerData => playerData.Spawned))
            {
                Debug.Log("[StartGame] Ready " + playerData.Uuid);
                allHaveSpawned = true;
            }

            return allHaveSpawned;
        }
    }
}