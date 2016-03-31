using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Models
{
    public class ShipData : NetworkBehaviour
    {
        [SyncVar] public float AttackDamage;

        [SyncVar] public float AttackSpeed;
        [SyncVar] public string PlayerUuid;
        [SyncVar] public string Uuid;

        public void Start()
        {
            Registry.Instance.Ships.Add(this);
            AttackSpeed = 1f;
            Random.Range(1f, 2f);
            AttackDamage = 2f; // Random.Range(1.7f,2.5f);
            // PlayerData = Registry.Player [PlayerData.Uid].GetComponent<PlayerData> ();
            // GetComponentInChildren<Renderer>().material.color = PlayerData.Color;
        }
    }
}