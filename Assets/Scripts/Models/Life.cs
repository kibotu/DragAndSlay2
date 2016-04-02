using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Models
{
    public class Life : NetworkBehaviour
    {
        #region initial values

        /// <summary>
        ///     Max hitpoints.
        /// </summary>
        public float MaxHp;

        /// <summary>
        ///     Hitpoint regeneration per second.
        /// </summary>
        public float HpRegen;

        /// <summary>
        ///     Armor translates into effective damage reduction. 1 <c>Armor</c> means 1 less <c>CurrentHp</c> decreased.
        /// </summary>
        public float Armor;

        /// <summary>
        ///     Max shields.
        /// </summary>
        public float MaxShield;

        /// <summary>
        ///     Amount of damage units it can absorb.
        /// </summary>
        public float ShieldStrength;

        /// <summary>
        ///     Amount of shield points regenation per second.
        /// </summary>
        public float ShieldRegen;

        #endregion

        #region runtime values

        /// <summary>
        ///     Current hitpoints. Unit dies if it reaches <c>less equal than 0</c>.
        /// </summary>
        public float CurrentHp;

        /// <summary>
        ///     Current amount of shield points. Unit doesn't lose <c>CurrentHp</c> as long as there are <c>CurrentShield</c>.
        /// </summary>
        public float CurrentShield;

        #endregion

        private float _hpRegenTime;
        private float _shieldRegenTime;

        public void Start()
        {
            CurrentHp = MaxHp;
            CurrentShield = MaxShield;
            _hpRegenTime = 0;
            _shieldRegenTime = 0;
        }

        public void Update()
        {
            if (!isServer)
                return;

            RegenerateHp();

            RegenerateShields();

            CmdUpdateHitPointsBar();
        }

        [Command]
        private void CmdUpdateHitPointsBar()
        {
            RpcUpdateHealthPointsBar(gameObject, CurrentHp / MaxHp);
        }

        [ClientRpc]
        void RpcUpdateHealthPointsBar(GameObject ship, float percent)
        {
            ship.GetComponentInChildren<HealthPointsBar>().Percent = percent;
        }
        
        private void RegenerateShields()
        {
            if (!(CurrentShield < MaxShield))
                return;

            _shieldRegenTime += Time.deltaTime;
            if (!(_shieldRegenTime > 1/ShieldRegen))
                return;

            _shieldRegenTime -= 1/ShieldRegen;

            CurrentShield += ShieldRegen;
        }

        private void RegenerateHp()
        {
            if (!(CurrentHp < MaxHp))
                return;

            _hpRegenTime += Time.deltaTime;
            if (!(_hpRegenTime > 1/HpRegen))
                return;

            _hpRegenTime -= 1/HpRegen;

            CurrentHp += HpRegen;
        }

        public bool IsAlive()
        {
            return CurrentHp > 0;
        }
    }
}