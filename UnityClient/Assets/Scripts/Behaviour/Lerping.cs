using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Behaviour
{
    public class Lerping : NetworkBehaviour
    {
        [SyncVar] private readonly float _lerpRate = 15;
        [SyncVar] [SerializeField] private Vector3 _syncPosition;

        private void FixedUpdate()
        {
            CmdProvidePositionToServer(transform.position);
            LerpPosition();
        }

        private void LerpPosition()
        {
            if (!isLocalPlayer)
            {
                // the 10 is a magic number. you probably want to set some sort of movement rate.
                transform.position = Vector3.Lerp(transform.position, _syncPosition, Time.deltaTime*_lerpRate);
            }
        }

        [Command]
        private void CmdProvidePositionToServer(Vector3 position)
        {
            _syncPosition = position;
        }
    }
}