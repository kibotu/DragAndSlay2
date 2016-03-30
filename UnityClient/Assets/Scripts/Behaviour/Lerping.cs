using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Behaviour
{
  public class Lerping : NetworkBehaviour
  {
    [SyncVar] [SerializeField] private Vector3 _syncPosition;

    [SyncVar] private float _lerpRate = 15;

    void FixedUpdate()
    {
      CmdProvidePositionToServer(transform.position);
      LerpPosition();
    }

    void LerpPosition()
    {
      if (!isLocalPlayer)
      {
        // the 10 is a magic number. you probably want to set some sort of movement rate.
        this.transform.position = Vector3.Lerp(this.transform.position, _syncPosition, Time.deltaTime*_lerpRate);
      }
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 position)
    {
      _syncPosition = position;
    }
  }
}