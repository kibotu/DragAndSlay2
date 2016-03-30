using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Network
{
  /// http://docs.unity3d.com/Manual/class-NetworkBehaviour.html
  public class CustomNetworkBehaviour : NetworkBehaviour
  {
    private Game _game;

    protected Game Game
    {
      get { return _game ?? (_game = GameObject.Find("_GameRoot").GetComponentInChildren<Game>()); }
      private set { _game = value; }
    }
  }
}