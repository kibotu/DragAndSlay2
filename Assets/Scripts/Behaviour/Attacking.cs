using Assets.Scripts.Models;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Behaviour
{
    [RequireComponent(typeof (Ship))]
    [RequireComponent(typeof (MoveToTarget))]
    [RequireComponent(typeof (Life))]
    public class Attacking : NetworkBehaviour
    {
        private MoveToTarget _moveToTarget;
        private Ship _ship;
        private Life _life;

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
            _life = GetComponent<Life>();
        }

        public void Update()
        {
            if (!isServer)
                return;

            if (IsMoving())
                return;

            if(!_life.IsAlive)
                return;

            if(_life.IsDying)
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

            Debug.Log(name + " starts to attack " + enemyShip.name);

            // 6) do actual combat and sets destroy flag if it is destroyed
            enemyShip.GetComponent<Defending>().Defend(this);

            // 7) send rocket which destroys enemy ship if flag is set
            var rocket = ((GameObject) Instantiate(WeaponType, transform.position, Quaternion.identity)).GetComponent<Rocket>();
            NetworkServer.Spawn(rocket.gameObject);
            RpcSearchAndDestroy(rocket.gameObject, _ship.Uuid, enemyShip.Uuid);
        }

        [ClientRpc]
        private void RpcSearchAndDestroy(GameObject rocketGameObject, string sourceShipUuid, string targetShipUuid)
        {
            Debug.Log("[RpcSearchAndDestroy] Sending rocket from " + sourceShipUuid + " to " + targetShipUuid);
            var rocket = rocketGameObject.GetComponent<Rocket>();
            var source = Registry.Instance.Ships.Find(ship => ship.Uuid.Equals(sourceShipUuid));
            rocket.Attacker = source.transform.position;
            var target = Registry.Instance.Ships.Find(ship => ship.Uuid.Equals(targetShipUuid));
            rocket.Defender = target;
            Debug.Log("[RpcSearchAndDestroy] Sending Rocket from " + source.name + " to " + target.name);
        }

        private static bool IsOnEnemyIsland(Island island, string playerUuid)
        {
            return !island.PlayerUuid.Equals(playerUuid);
        }
    }
}