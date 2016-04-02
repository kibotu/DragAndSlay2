using UnityEngine;

namespace Assets.Scripts
{
    public class Settings : MonoBehaviour
    {
        /// http://answers.unity3d.com/questions/545637/graphics-quality-settings-on-android.html
        [Range(0, 5)] [SerializeField] private int _quality;

        private void Update()
        {
            QualitySettings.SetQualityLevel(_quality, true);
        }
    }
}