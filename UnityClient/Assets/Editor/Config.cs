using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CreateAssetMenu(fileName = "Config", menuName = "Config", order = 1)]
    public class Config : ScriptableObject
    {
        public bool VirtualRealitySupported = PlayerSettings.virtualRealitySupported;
    }
}