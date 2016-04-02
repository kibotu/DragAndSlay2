using System.Linq;
using UnityEngine.Networking;

namespace Assets.Scripts.Models
{
    public class Ship : NetworkBehaviour
    {
        [SyncVar] public string PlayerUuid;
        [SyncVar] public string Uuid;

        public void Start()
        {
            Registry.Instance.Ships.Add(this);

            // PlayerData = Registry.Player [PlayerData.Uid].GetComponent<PlayerData> ();
            // GetComponentInChildren<Renderer>().material.color = PlayerData.Color;
        }

        public void OnDestroy()
        {
            if (!Registry.ApplicationIsQuitting)
                Registry.Instance.Ships.Remove(Registry.Instance.Ships.First(item => item == this));
        }
    }
}