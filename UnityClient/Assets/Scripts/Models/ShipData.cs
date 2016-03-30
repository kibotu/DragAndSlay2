using System;
using System.Runtime.InteropServices;
using Assets.Scripts.Models;
using Assets.Scripts.Utils;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Models
{
  public class ShipData : NetworkBehaviour
  {
    [SyncVar] public string Uuid;
    [SyncVar] public string PlayerUuid;

    [SyncVar] public float AttackSpeed;
    [SyncVar] public float AttackDamage;

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