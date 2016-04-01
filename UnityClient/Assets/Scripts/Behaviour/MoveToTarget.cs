using Assets.Scripts.Models;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class MoveToTarget : MonoBehaviour
    {
        private Vector3 _finalDestination;

        private Orbiting _orbiting;
        public float RotationVelocity = 50f;

        public GameObject Target;
        public float ThreshholdDistance = 0.01f;
        public float Velocity = 5f;

        private void OnEnable()
        {
            _orbiting = gameObject.GetComponent<Orbiting>();
            _orbiting.Center = Target.transform;
            _finalDestination = _orbiting.GetFinalDestination();
            _orbiting.enabled = false;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _finalDestination, Time.deltaTime*Velocity);

            var targetDir = transform.position.Direction(_finalDestination);
            var step = RotationVelocity*Time.deltaTime;
            var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);

            Debug.DrawRay(transform.position, newDir, Color.red);

            transform.rotation = Quaternion.LookRotation(newDir);

            if (Vector3.Distance(transform.position, _finalDestination) < ThreshholdDistance)
            {
                transform.parent = Target.transform;
                _orbiting.enabled = true;
                enabled = false;
                Arrive();
            }
        }

        private void Arrive()
        {
            var shipData = GetComponent<Ship>();
            Debug.Log("[Arrive] " + shipData.Uuid + " for player " + shipData.PlayerUuid);
//
//            var shipData 
        }
    }
}