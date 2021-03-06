﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Utils
{
    public static class VectorExtensions
    {
        public static Vector3 Direction(this Vector3 source, Vector3 target)
        {
            return (target - source).normalized;
        }

        public static float Range(float min, float max, float excludeRangeMin, float excludeRangeMax)
        {
            return Random.Range(0, 2) == 0 ? Random.Range(min, excludeRangeMin) : Random.Range(excludeRangeMax, max);
        }

        public static float ComputeVelocity(float acceleration, float startVelocity = 0f)
        {
            return startVelocity + acceleration*Time.time;
        }

        /// <summary>
        ///     @return the angle in degrees of this vector (point) relative to the x-axis. Angles are towards the positive y-axis
        ///     (typically
        ///     counter-clockwise) and between 0 and 360.
        /// </summary>
        public static float Angle(this Vector2 v)
        {
            var angle = Mathf.Atan2(v.y, v.x)*Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            return angle;
        }

        public static bool IsNullOrEmpty(this ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static T GetRandom<T>(this IList<T> collection)
        {
            return collection[Random.Range(0, collection.Count)];
        }

        public static IEnumerator Delay(this Action action, float duration)
        {
            yield return new WaitForSeconds(duration);
            action.Invoke();
        }
    }
}