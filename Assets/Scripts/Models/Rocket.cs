using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Models
{
    public class Rocket : NetworkBehaviour
    {
        /// <summary>
        /// Damage on hit.
        /// </summary>
        public float AttackDamage;

        /// <summary>
        /// Acceleration speed.
        /// </summary>
        public float Acceleration;

        /// <summary>
        /// Rocket Velocity.
        /// </summary>
        public float Velocity;

        /// <summary>
        /// Source.
        /// </summary>
        public Vector3 Attacker;

        /// <summary>
        /// Target.
        /// </summary>
        public Ship Defender;
        
        /// <summary>
        /// Direction towards from source to target.
        /// </summary>
        public Vector3 Dir;

        /// <summary>
        /// Interpolation helper variable.
        /// </summary>
        private float _startTime;

        public void Start()
        {
            transform.position = Attacker;
        }

        public void Update()
        {
            // another ship has destroyed the target; Note: potentially re-targeting
            if (Defender == null)
            {
//                Destroy(gameObject);
                return;
            }

            MoveTowards();

            RotateTowards();

            Billboard();

            Hit();
        }

        [Command]
        private void CmdDestroyIfDead()
        {
            if (!Defender.GetComponent<Life>().IsAlive())
                return;

            Destroy(Defender);
            RpcDestroyIfDead(Defender.gameObject);
        }

        [ClientRpc]
        private void RpcDestroyIfDead(GameObject defender)
        {
            Destroy(defender);
        }

        private void Hit()
        {
            // if reached, apply damage and destroy rocket
            if (!(Vector3.Distance(transform.position, Defender.transform.position) < 0.75f))
                return;

            //var hit = Prefabs.Instance.GetNewSmallExplosion();
            //hit.transform.position = gameObject.transform.position;
            //hit.transform.parent = Defender.transform;
            CmdDestroyIfDead();
            Destroy(gameObject);
        }

        private void Billboard()
        {
            // transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            // however we want the rocket pointing its up vector towards the target position
            transform.LookAt(transform.position + Camera.main.transform.rotation*Vector3.forward,
                Camera.main.transform.rotation*Dir);
        }

        private void RotateTowards()
        {
            // rotate along forward axe of camera towards target
            Dir = transform.position.Direction(Defender.transform.position);
        }

        private void MoveTowards()
        {
            // transform.position = Vector3.MoveTowards(transform.position, Defender.transform.position, Time.delta * Velocity); // insufficiant interpolation
            _startTime += Time.deltaTime;
            // http://whydoidoit.com/2012/04/06/unity-curved-path-following-with-easing/
            transform.position = new Vector3
            {
                x =
                    Mathf.Lerp(Attacker.x, Defender.transform.position.x,
                        Easing.Sinusoidal.easeInOut(_startTime*Velocity)),
                y =
                    Mathf.Lerp(Attacker.y, Defender.transform.position.y,
                        Easing.Sinusoidal.easeInOut(_startTime*Velocity)),
                z =
                    Mathf.Lerp(Attacker.z, Defender.transform.position.z,
                        Easing.Sinusoidal.easeInOut(_startTime*Velocity))
            };
        }
    }
}