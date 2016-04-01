using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Config", menuName = "Config", order = 1)]
    public class Config : ScriptableObject
    {
        public bool VirtualRealitySupported = false /* PlayerSettings.virtualRealitySupported */;
    }
}