using System;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class PhysicData
    {
        public float Acceleration;
        public float Mass;

        public Vector3 Position;
        public Quaternion Rotation;
        public float RotationDistance;
        public float RotationSpeed;
        public Vector3 Scalling;

        public float Speed(int t, int v0)
        {
            return Acceleration*t + v0;
        }
    }
}