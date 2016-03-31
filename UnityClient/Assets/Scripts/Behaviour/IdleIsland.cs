using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class IdleIsland : MonoBehaviour
    {
        public Vector2 minRotation;
        public Vector2 minRotSpeed;
        private Vector3 originalPosition;

        private Quaternion originalRotation;
        private Vector3 rotation;
        public Vector3 rotSpeed;
        public Vector3 translation;
        public Vector3 transSpeed;

        public Vector2 xRot;

        public Vector2 xTrans;
        public Vector2 yRot;
        public Vector2 yTrans;
        public Vector2 zRot;
        public Vector2 zTrans;

        public void Start()
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;

            const float exclude = 0.3f;

            // translation
            xTrans = new Vector2(-0.6f, 0.6f);
            yTrans = new Vector2(-0.6f, 0.6f);
            zTrans = new Vector2(-0.6f, 0.6f);
            translation = new Vector3(
                VectorExtensions.Range(xTrans.x, xTrans.y, -exclude, exclude),
                VectorExtensions.Range(yTrans.x, yTrans.y, -exclude, exclude),
                VectorExtensions.Range(zTrans.x, zTrans.y, -exclude, exclude)
                );
            transSpeed = new Vector3(
                VectorExtensions.Range(minRotSpeed.x, minRotSpeed.y, -exclude, exclude),
                VectorExtensions.Range(minRotSpeed.x, minRotSpeed.y, -exclude, exclude),
                VectorExtensions.Range(minRotSpeed.x, minRotSpeed.y, -exclude, exclude)
                );

            // rotation
            xRot = new Vector2(-5f, 5f);
            yRot = new Vector2(-5f, 5f);
            zRot = new Vector2(-5f, 5f);
            minRotSpeed = new Vector2(-0.8f, 0.8f);
            minRotation = new Vector2(-5, 5);
            rotation.x = VectorExtensions.Range(xRot.x, xRot.y, minRotation.x, minRotation.y);
            rotation.y = VectorExtensions.Range(yRot.x, yRot.y, minRotation.x, minRotation.y);
            rotation.z = VectorExtensions.Range(zRot.x, zRot.y, minRotation.x, minRotation.y);
            rotSpeed = new Vector3(
                VectorExtensions.Range(minRotSpeed.x, minRotSpeed.y, -exclude, exclude),
                VectorExtensions.Range(minRotSpeed.x, minRotSpeed.y, -exclude, exclude),
                VectorExtensions.Range(minRotSpeed.x, minRotSpeed.y, -exclude, exclude)
                );
        }

        public void Update()
        {
            transform.localRotation = originalRotation*Quaternion.Euler(
                rotation.x*Mathf.Sin(Time.time*rotSpeed.x),
                rotation.y*Mathf.Sin(Time.time*rotSpeed.y),
                rotation.z*Mathf.Sin(Time.time*rotSpeed.z)
                );

            var pos = new Vector3(
                translation.x*Mathf.Sin(Time.time*transSpeed.x),
                translation.y*Mathf.Cos(Time.time*transSpeed.y),
                translation.z*Mathf.Sin(Time.time*transSpeed.z));

            transform.position = originalPosition + pos;
        }

        public void OnDestroy()
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;
        }
    }
}