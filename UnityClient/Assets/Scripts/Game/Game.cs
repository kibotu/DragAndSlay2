using System.Security.AccessControl;
using Assets.Scripts.Models;
using Assets.Scripts.Network;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
  public class Game : MonoBehaviour
  {
    public void SearchPlayer()
    {
      Debug.Log("[Search Player]");






      // SceneManager.LoadScene(1);
    }

    void OnDestroy()
    {
      NetworkManager.singleton.StopHost();
    }

    void test()
    {
      // LAN Host
      NetworkManager.singleton.StartHost();

      // LAN Server Only
      NetworkManager.singleton.StartServer();

      // Stop Host
      NetworkManager.singleton.StopHost();

      // Enable Matchmaker
      NetworkManager.singleton.StartMatchMaker();

      // pair this with NetworkMatch.CreateMatch
      // NetworkManager.OnMatchCreate()
      // pair this with NetworkMatch.JoinMatch
      // NetworkManager.OnMatchJoined();
      // pair this with NetworkMatch.ListMatches*/
      // NetworkManager.OnMatchList()
    }
  }
}