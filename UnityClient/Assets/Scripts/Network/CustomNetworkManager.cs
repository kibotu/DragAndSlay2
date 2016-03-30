using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

namespace Assets.Scripts.Network
{
  public class CustomNetworkManager : NetworkManager
  {
    public void StartupHost()
    {
      SetPort();
      NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
      SetIPAddress();
      SetPort();
      NetworkManager.singleton.StartClient();
    }

    public void SetIPAddress()
    {
      NetworkManager.singleton.networkAddress = "192.168.0.169";
      Debug.Log("[SetIPAddress] " + NetworkManager.singleton.networkAddress);
    }

    public void SetPort()
    {
      NetworkManager.singleton.networkPort = 1337;
      Debug.Log("[SetPort] " + NetworkManager.singleton.networkPort);
    }

    public override NetworkClient StartHost(ConnectionConfig config, int maxConnections)
    {
      Debug.Log("[StartHost] " + config);
      return base.StartHost(config, maxConnections);
    }

    public override NetworkClient StartHost(MatchInfo info)
    {
      Debug.Log("[StartHost] " + info);
      return base.StartHost(info);
    }

    public override NetworkClient StartHost()
    {
      Debug.Log("[StartHost]");
      return base.StartHost();
    }

    public override void ServerChangeScene(string newSceneName)
    {
      Debug.Log("[ServerChangeScene] " + newSceneName);
      base.ServerChangeScene(newSceneName);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
      Debug.Log("[OnServerConnect] " + conn);
      base.OnServerConnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
      Debug.Log("[OnServerDisconnect] " + conn);
      base.OnServerDisconnect(conn);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
      Debug.Log("[OnServerReady] " + conn);
      base.OnServerReady(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId,
      NetworkReader extraMessageReader)
    {
      base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
      Debug.Log("[OnServerAddPlayer] " + conn + " " + playerControllerId + " player: " +
                NetworkManager.singleton.numPlayers + " reader " + extraMessageReader);
    }


    public event Action OnReady;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
      base.OnServerAddPlayer(conn, playerControllerId);
      Debug.Log("[OnServerAddPlayer] " + conn + " " + playerControllerId + " player: " +
                NetworkManager.singleton.numPlayers);

      if (NetworkManager.singleton.numPlayers != 2) return;
      if (OnReady != null)
        OnReady.Invoke();
    }


    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
      Debug.Log("[OnServerRemovePlayer] " + conn + " " + player);
      base.OnServerRemovePlayer(conn, player);
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
      Debug.Log("[OnServerError] " + conn + " " + errorCode);
      base.OnServerError(conn, errorCode);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
      Debug.Log("[OnServerSceneChanged] " + sceneName);
      base.OnServerSceneChanged(sceneName);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
      Debug.Log("[OnClientConnect] " + conn);
      base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
      Debug.Log("[OnClientDisconnect] " + conn);
      base.OnClientDisconnect(conn);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
      Debug.Log("[OnClientError] " + conn + " " + errorCode);
      base.OnClientError(conn, errorCode);
    }

    public override void OnClientNotReady(NetworkConnection conn)
    {
      Debug.Log("[OnClientNotReady] " + conn);
      base.OnClientNotReady(conn);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
      Debug.Log("[OnClientSceneChanged] " + conn);
      base.OnClientSceneChanged(conn);
    }

    public override void OnStartHost()
    {
      Debug.Log("[OnStartHost]");
      base.OnStartHost();
    }

    public override void OnStartServer()
    {
      Debug.Log("[OnStartServer]");
      base.OnStartServer();
    }

    public override void OnStartClient(NetworkClient client)
    {
      Debug.Log("[OnStartClient] " + client);
      base.OnStartClient(client);
    }

    public override void OnStopServer()
    {
      Debug.Log("[OnStopServer]");
      base.OnStopServer();
    }

    public override void OnStopClient()
    {
      Debug.Log("[OnStopClient]");
      base.OnStopClient();
    }

    public override void OnStopHost()
    {
      Debug.Log("[OnStopHost]");
      base.OnStopHost();
    }

    public override void OnMatchCreate(CreateMatchResponse matchInfo)
    {
      Debug.Log("[OnMatchCreate] " + matchInfo);
      base.OnMatchCreate(matchInfo);
    }

    public override void OnMatchList(ListMatchResponse matchList)
    {
      Debug.Log("[OnMatchList] " + matchInfo);
      base.OnMatchList(matchList);
    }
  }
}