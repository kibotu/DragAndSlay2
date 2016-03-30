using System;
using System.Linq;
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
  }
}