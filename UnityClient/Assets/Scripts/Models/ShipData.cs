using System;
using Assets.Scripts.Models;
using Assets.Scripts.Utils;
using Microsoft.Win32;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Models
{
  public class ShipData : MonoBehaviour
  {
    public int ShipType;
    public float AttackSpeed;
    public float AttackDamage;

    public PlayerData PlayerData;

    private string _uuid;

    public string Uuid
    {
      get { return _uuid ?? (_uuid = Guid.NewGuid().ToString()); }
      set { _uuid = value; }
    }

    public void Start()
    {
      Registry.Instance.Ships.Add(new Registry.UniqueGameObject {Uuid = Uuid, GameObject = this.gameObject});
      AttackSpeed = 1f;
      Random.Range(1f, 2f);
      AttackDamage = 2f; // Random.Range(1.7f,2.5f);
      // PlayerData = Registry.Player [PlayerData.Uid].GetComponent<PlayerData> ();
      GetComponentInChildren<Renderer>().material.color = PlayerData.Color;
    }
  }
}