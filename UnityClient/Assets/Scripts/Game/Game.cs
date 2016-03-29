using System.Security.AccessControl;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
  public class Game : MonoBehaviour
  {
    public void SearchPlayer()
    {
      Debug.Log("[Search Player]");

      SceneManager.LoadScene(1);
    }
  }
}