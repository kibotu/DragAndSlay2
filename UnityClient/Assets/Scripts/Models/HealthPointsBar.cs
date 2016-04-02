using UnityEngine;

namespace Assets.Scripts.Models
{
    public class HealthPointsBar : MonoBehaviour
    {
        [Range(0f, 1f)] [SerializeField] private float _percent = 1f;
        private float _startScale;

        public float Percent
        {
            get { return _percent; }
            set { _percent = Mathf.Clamp01(value); }
        }

        private void Start()
        {
            _startScale = transform.localScale.x;
        }

        private void Update()
        {
            var scale = transform.localScale;
            scale.x = Percent*_startScale;
            transform.localScale = scale;
        }
    }
}