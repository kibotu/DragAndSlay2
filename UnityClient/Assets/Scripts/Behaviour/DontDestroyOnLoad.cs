using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void OnApplicationQuit()
        {
            DestroyImmediate(gameObject);
        }
    }
}