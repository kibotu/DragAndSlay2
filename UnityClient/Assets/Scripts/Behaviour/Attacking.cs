using Assets.Scripts.Models;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Behaviour
{
    [RequireComponent(typeof (Ship))]
    [RequireComponent(typeof (MoveToTarget))]
    public class Attacking : NetworkBehaviour
    {
        private MoveToTarget _moveToTarget;
        private Ship _ship;

        /// <summary>
        ///     Attack speed helper variable.
        /// </summary>
        private float _startTime;

        /// <summary>
        ///     Damage Per Hit.
        /// </summary>
        [SyncVar] public float AttackDamage = 1f;

        /// <summary>
        ///     Attacks per second.
        /// </summary>
        [SyncVar] public float AttackSpeed = 1f;

        /// <summary>
        ///     Weapon Type.
        /// </summary>
        public GameObject WeaponType;

        public void Start()
        {
            _moveToTarget = GetComponent<MoveToTarget>();
            _ship = GetComponent<Ship>();
        }

        public void Update()
        {
            if (!isServer)
                return;

            if (IsMoving())
                return;

            CmdSearchAndDestroy();
        }

        private bool IsMoving()
        {
            return _moveToTarget.enabled;
        }

        [Command]
        private void CmdSearchAndDestroy()
        {
            // 1) attack speed
            _startTime += Time.deltaTime;
            if (_startTime < 1/AttackSpeed) return;

            _startTime -= 1/AttackSpeed;

            // 2) current island
            var island = transform.parent.GetComponent<Island>();

            // 3) get all enemy ships
            var enemyShips = Island.GetEnemyShips(island, _ship.PlayerUuid);

            // 4) no enemy ships => convert
            if (IsOnEnemyIsland(island, _ship.PlayerUuid) && enemyShips.IsNullOrEmpty())
            {
                island.Convert(_ship.PlayerUuid);
                return;
            }

            // 5) attack every enemy ship
            if (enemyShips.IsNullOrEmpty())
                return;

            var enemyShip = enemyShips.GetRandom();

            var rocket = ((GameObject) Instantiate(WeaponType, transform.position, Quaternion.identity)).GetComponent<Rocket>();
            rocket.AttackDamage = AttackDamage;

            // server does combat and sets flag if rockets should destroy ship
            // then spawns a network rocket that destroy ships if flag is set
            NetworkServer.Spawn(rocket.gameObject);

            RpcSearchAndDestroy(rocket.gameObject, _ship.Uuid, enemyShip.Uuid);
        }

        [ClientRpc]
        private void RpcSearchAndDestroy(GameObject rocketGameObject, string sourceShipUuid, string targetShipUuid)
        {
            var rocket = rocketGameObject.GetComponent<Rocket>();
            rocket.Attacker = Registry.Instance.Ships.Find(ship => ship.Uuid.Equals(sourceShipUuid)).transform.position;
            rocket.Defender = Registry.Instance.Ships.Find(ship => ship.Uuid.Equals(targetShipUuid));
        }

        private static bool IsOnEnemyIsland(Island island, string playerUuid)
        {
            return !island.PlayerUuid.Equals(playerUuid);
        }
    }
}