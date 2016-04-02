﻿using System;
using Assets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Behaviour
{
    public class Orbiting : MonoBehaviour
    {
        public const float Epsilon = 0.001f;
        private float _heighttime;
        private float _lastyoffset;
        private float _randomHeight;
        private float _randomTwistChance;
        private float _randomTwistChanceTime;
        private float _time;
        private float _twistTime;
        public Transform Center;

        // twist
        public Vector2 ChanceOfRandomTwist;
        public float CirculationTerm;
        public float Height;
        public float Heightchangetime;

        // yoffset
        public Vector2 Heightvariance;

        // circulation
        public float Radius;
        public Quaternion Rotation;
        public float StrafeAngle;
        public float TwistDuration;
        public float Yoffset;

        public void Start()
        {
            if (Math.Abs(Yoffset) < Epsilon) Yoffset = Random.Range(-2f, 2f);
            if (Center == null) Center = transform.parent;
            if (Math.Abs(Radius) < Epsilon) Radius = Random.Range(3f, 7f);
            if (Math.Abs(CirculationTerm) < Epsilon) CirculationTerm = Random.Range(4f, 12f);
            if (Heightvariance == Vector2.zero) Heightvariance = new Vector2(-2f, 2f);
            if (Math.Abs(Heightchangetime) < Epsilon) Heightchangetime = CirculationTerm/2f;
            if (Math.Abs(StrafeAngle) < Epsilon) StrafeAngle = 15f;
            if (ChanceOfRandomTwist == Vector2.zero) ChanceOfRandomTwist = new Vector2(5f, 10f);
            if (Math.Abs(TwistDuration) < Epsilon) TwistDuration = 1.125f;
            _twistTime = TwistDuration;
            _randomTwistChance = Random.Range(ChanceOfRandomTwist.x, ChanceOfRandomTwist.y);
            _heighttime = Heightchangetime;
        }

        public void FixedUpdate()
        {
            if (Center == null) return;

            transform.position = GetFinalDestination();
            transform.rotation = GetFinalRotation();
            UpdateStrafe();
        }

        public Vector3 GetFinalDestination()
        {
            _time += Time.deltaTime;
            Height = ComputeHeightVariation();
            return RotateAroundCenterY(transform.position, _time, GetRadius(), Center.position, CirculationTerm, Height);
        }

        private float ComputeHeightVariation()
        {
            _heighttime += Time.deltaTime;
            if (_heighttime > Heightchangetime)
            {
                _heighttime = 0;
                _lastyoffset = _randomHeight;
                _randomHeight = Random.Range(Heightvariance.x, Heightvariance.y) + Yoffset;
            }
            return Mathf.Lerp(_lastyoffset, _randomHeight, Easing.Sinusoidal.easeInOut(_heighttime/Heightchangetime));
        }

        private void UpdateStrafe()
        {
            _twistTime += Time.deltaTime;
            if (_twistTime <= TwistDuration)
            {
                StrafeAngle = Mathf.Lerp(10, 370, Easing.Sinusoidal.easeInOut(_twistTime/TwistDuration));
            }
            else
            {
                _randomTwistChanceTime += Time.deltaTime;
                if (_randomTwistChanceTime > _randomTwistChance)
                {
                    _randomTwistChanceTime = 0;
                    _twistTime = 0;
                    _randomTwistChance = Random.Range(ChanceOfRandomTwist.x, ChanceOfRandomTwist.y);
                }
            }
        }

        public Quaternion GetFinalRotation()
        {
            // beware this is the most complicated way i could come up with to rotate the plane along the tangent of the circle
            var targetDir =
                RotateAroundCenterY(transform.position, (_time + CirculationTerm/360), GetRadius(), Center.position,
                    CirculationTerm, Height) - transform.position;
            var step = 1000*Time.deltaTime;
            var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            // Debug.DrawRay(transform.position, newDir, Color.red);
            return Quaternion.LookRotation(newDir)*Quaternion.Euler(0, 0, -StrafeAngle);
                //*Quaternion.Euler(-90f, 0, 0) * Quaternion.Euler(0,0,90) *;
        }

        public static Vector3 RotateAroundCenterY(Vector3 position, float time, float radius, Vector3 center,
            float circulationTerm, float yoffset)
        {
            var circle = RotoateAroundCenter(time, radius, new Vector2(center.x, center.z), circulationTerm);
            position.x = circle.x;
            position.y = center.y + yoffset;
            position.z = circle.y;
            return position;
        }

        public static Vector2 RotoateAroundCenter(float time, float radius, Vector2 center, float circulationTerm)
        {
            var dx = center.x + (-radius*Mathf.Cos(Phi(time, circulationTerm)));
            var dy = center.y + (radius*Mathf.Sin(Phi(time, circulationTerm)));
            return new Vector2(dx, dy);
        }

        public static float Phi(float t, float tu)
        {
            return (Mathf.Abs(tu) < Epsilon) ? 2f*Mathf.PI*t : 2f*Mathf.PI*t/tu;
        }

        public float GetRadius()
        {
            return Radius*transform.lossyScale.x;
        }
    }
}