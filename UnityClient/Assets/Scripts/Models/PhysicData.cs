using System;
using UnityEngine;

namespace Assets.Scripts.Models
{
  	[Serializable]
    public class PhysicData {
	
        public Vector3 Position;
        public Vector3 Scalling;
        public Quaternion Rotation;
	
        public float Acceleration;
        public float Mass;
        public float RotationSpeed;
        public float RotationDistance;

        public float Speed(int t, int v0) {
            return Acceleration * t + v0;
        }
    }
}

