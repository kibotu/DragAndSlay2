﻿using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    [RequireComponent(typeof (Life))]
    public class Defending : MonoBehaviour
    {
        [SerializeField] private Life _life;
        private GameObject _shield;

        public float ShieldDistance = 0.7f;

//        private bool _isExploding = false;

        public void Start()
        {
            _life = GetComponent<Life>();
        }

        public void Defend(Attacking attacking)
        {
            _life.CurrentHp -= attacking.AttackDamage;
            Debug.Log(string.Format("{0} deals {1} damage to {2}, {3} hp left", attacking.name, attacking.AttackDamage, name, _life.CurrentHp));
        }

//
//            Defend(rocket.AttackDamage);
//            // reduce hitpoints
//        {

//        public void Defend(Rocket rocket)
//            // spawn shield
//            if (_shield == null)
//            {
//                _shield = Prefabs.Instance.GetNewShield();
//                // _shield.renderer.material.SetColor("_Tint", Color.red);
//            }
//           // else
//             //   _shield.GetComponent<FadeOutAndDestroy>().Reset();
//
//            _shield.transform.forward = -rocket.Dir;
//            _shield.transform.position = transform.position - rocket.Dir * ShieldDistance;
//            _shield.transform.parent = transform;
//        }
//
//        public void Defend(float damage)
//        {
//            _life.CurrentHp -= damage;
//            if (_life.CurrentHp > 0 || _isExploding) return;
//
//            Destroy(gameObject);
//            var explosion = Prefabs.Instance.GetNewExplosion();
//            explosion.transform.position = transform.position;
//            //            explosion.GetComponent<DetonatorShockwave>().color = GetComponent<ShipData>().PlayerData.color;
//
//            Registry.Instance.Ships.Remove(GetComponent<Ship>());
//
//            //            Debug.Log(GetComponent<ShipData>().uid + " has been destroyed!");
//            //            _isExploding = true;
//        }
    }
}