using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Ui
{
  public class Menu : MonoBehaviour {

    public void StartHost()
    {
      NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
      NetworkManager.singleton.StartClient();
    }
  }
}