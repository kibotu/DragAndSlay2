using System;
using System.Collections;
using Assets.Scripts.Network;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Models
{
  public class IslandData : NetworkBehaviour
  {
    [SyncVar] public string Uuid;
    [SyncVar] public string PlayerUuid;

    public GameObject ShipType;

    /// spawn per second
    [SyncVar] public float SpawnRate;

    [SyncVar] public int MaxSpawn;
    [SyncVar] public float CurrentRespawnRate;

    public Renderer IslandRenderer;

    public void Awake()
    {
      Registry.Instance.Islands.Add(this);
      CurrentRespawnRate = SpawnRate;
      Dye();
    }

    public void Convert(string uuid)
    {
      PlayerUuid = uuid;
      Dye();
      var shockwave = Prefabs.Instance.GetNewShockwave();
      shockwave.transform.position = transform.position;
//      shockwave.GetComponent<DetonatorShockwave>().color = PlayerData.Color;
    }

    public void Dye()
    {
      // todo set new material
      // IslandRenderer.material =
    }

    /// <returns>Respawn rate in seconds.</returns>
    public float ShipBuildTime()
    {
      var dominance = DominancePercentage(this);
      CurrentRespawnRate = Mathf.Abs(dominance - 1f) < Mathf.Epsilon
        ? SpawnRate
        : SpawnRate + (SpawnRate*(1 - dominance));
      return CurrentRespawnRate;
    }

    /// <summary>
    /// Computes current respawn timer based on dominance.
    ///
    /// 100% if no enemy ships are present.
    /// 0% if at least one enemy ship but no own ships are present.
    ///
    /// </summary>
    /// <returns>Respawn rate in percent. Value between 0f and 1f.</returns>
    public static float DominancePercentage(IslandData island)
    {
      var friendly = 0;
      var foes = 0;

      for (var i = 0; i < island.transform.childCount; ++i)
      {
        var ship = island.transform.GetChild(i).gameObject;
        var otherShipData = ship.GetComponent<ShipData>(); // possibly cachable
        if (otherShipData == null) continue; // skip non ship gameobjects
        if (otherShipData.Uuid == island.PlayerUuid)
        {
          ++friendly;
        }
        else
        {
          ++foes;
        }
      }

      var sum = foes + friendly;

      return sum == 0 ? 1f : friendly/(float) sum;
    }

    public static ArrayList GetFriendlyShips(IslandData island, string thisUid)
    {
      var enemyShips = new ArrayList(island.transform.childCount - 1);
      for (var i = 0; i < island.transform.childCount; ++i)
      {
        var ship = island.transform.GetChild(i).gameObject;
        var otherShipData = ship.GetComponent<ShipData>(); // possibly cachable
        if (otherShipData == null) continue; // skip non ship gameobjects
        if (otherShipData.Uuid == thisUid)
          enemyShips.Add(ship);
      }
      return enemyShips;
    }

    public static ArrayList GetEnemyShips(IslandData island, string thisUid)
    {
      var enemyShips = new ArrayList(island.transform.childCount - 1);
      for (var i = 0; i < island.transform.childCount; ++i)
      {
        var ship = island.transform.GetChild(i).gameObject;
        var otherShipData = ship.GetComponent<ShipData>(); // possibly cachable
        if (otherShipData == null) continue; // skip non ship gameobjects
        if (otherShipData.Uuid != thisUid)
          enemyShips.Add(ship);
      }
      return enemyShips;
    }

    public static int AmountFriendlyUnits(IslandData island)
    {
      var friendly = 0;

      for (var i = 0; i < island.transform.childCount; ++i)
      {
        var ship = island.transform.GetChild(i).gameObject;
        var otherShipData = ship.GetComponent<ShipData>(); // possibly cachable
        if (otherShipData == null) continue; // skip non ship gameobjects
        if (otherShipData.PlayerUuid == island.PlayerUuid)
        {
          ++friendly;
        }
      }

      return friendly;
    }
  }
}