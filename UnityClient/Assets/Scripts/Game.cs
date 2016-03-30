using System;
using System.Linq;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Models;
using Assets.Scripts.Network;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
  public class Game : NetworkBehaviour
  {
    [SyncVar] public readonly int MaxPopulationLimit = 50;
    [SerializeField] private int _playerCount = 2;

    public bool MaxPopulationLimitReached
    {
      get { return Registry.Instance.Ships.Count >= MaxPopulationLimit; }
    }

    public void Start()
    {
    }

    private void OnReady()
    {
      if (!isServer)
      {
        Debug.Log("[OnReady] Start");
      }
    }

    void OnDestroy()
    {
      NetworkManager.singleton.StopHost();
    }

    public void StartupHost()
    {
      NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
      NetworkManager.singleton.StartClient();
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

      foreach (var playerData in Registry.Instance.Player)
      {
        if (playerData.Spawned)
        {
          Debug.Log("[StartGame] Ready " + playerData.Uuid);
          allHaveSpawned = true;
        }
      }

      return allHaveSpawned;
    }
  }
}