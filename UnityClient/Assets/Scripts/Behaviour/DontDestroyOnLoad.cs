using UnityEngine;

namespace Assets.Scripts.Behaviour
{
  public class DontDestroyOnLoad : MonoBehaviour
  {
    void Awake()
    {
      DontDestroyOnLoad(this);
    }
  }
}