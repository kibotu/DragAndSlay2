using System.Security.AccessControl;
using Assets.Scripts.Models;
using Assets.Scripts.Network;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
  public class Game : NetworkBehaviour
  {
    public void Start()
    {
      ((CustomNetworkManager) (NetworkManager.singleton)).OnReady += OnReady;
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
  }
}